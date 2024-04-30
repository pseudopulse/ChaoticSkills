using System;
using HarmonyLib;
using HG.GeneralSerializer;
using RoR2.UI;
using CoinBehavior = ChaoticSkills.Content.Railgunner.Coin.CoinBehaviour;

namespace ChaoticSkills.Content.Railgunner {
    public class Coin : SkillBase<Coin> {
        public override SerializableEntityStateType ActivationState => ContentAddition.AddEntityState<EntityStates.Railgunner.FireCoin>(out bool _);
        public override float Cooldown => 4f;
        public override bool DelayCooldown => false;
        public override string Description => "Toss a <style=cIsUtility>device</style> that <style=cIsDamage>reflects shots into nearby enemies</style> when struck, <style=cIsUtility>amplifying their damage</style>.";
        public override bool Agile => true;
        public override bool IsCombat => true;
        public override string LangToken => "Coin";
        public override int MaxStock => 4;
        public override int StockToConsume => 1;
        public override UnlockableDef Unlock => null;
        public override string Machine => "Weapon";
        public override Sprite SkillIcon => Main.Assets.LoadAsset<Sprite>("Coin.png");
        public override SkillSlot Slot => SkillSlot.Primary;
        public override string Survivor => Utils.Paths.GameObject.RailgunnerBody;
        public override string Name => "HR-30 Redirector";

        public static GameObject CoinPrefab;

        public override void PostCreation()
        {
            base.PostCreation();

            CoinPrefab = PrefabAPI.InstantiateClone(Utils.Paths.GameObject.EngiBubbleShield.Load<GameObject>(), "RailgunnerCoin");
            CoinPrefab.RemoveComponent<BeginRapidlyActivatingAndDeactivating>();
            CoinPrefab.RemoveComponent<ProjectileStickOnImpact>();
            CoinPrefab.RemoveComponent<EntityStateMachine>();
            CoinPrefab.RemoveComponent<NetworkStateMachine>();
            CoinPrefab.RemoveComponent<Deployable>();
            CoinPrefab.RemoveComponent<EventFunctions>();

            CoinPrefab.AddComponent<CoinBehavior>();
            CoinPrefab.GetComponent<ProjectileSimple>().desiredForwardSpeed *= 1f;
            CoinPrefab.GetComponent<ProjectileSimple>().lifetime = 10f;
            CoinPrefab.GetComponent<Rigidbody>().mass = 0.5f;

            CoinPrefab.layer = LayerIndex.projectile.intVal;

            GameObject hb = new("HurtBox");
            hb.transform.SetParent(CoinPrefab.transform);
            SphereCollider col = hb.AddComponent<SphereCollider>();
            col.radius = 2f;
            hb.layer = LayerIndex.entityPrecise.intVal;
            hb.transform.position = Vector3.zero;
            hb.transform.localPosition = Vector3.zero;

            Rigidbody rb = hb.AddComponent<Rigidbody>();
            rb.isKinematic = true;

            HurtBox hurt = hb.AddComponent<HurtBox>();
            hurt.isSniperTarget = true;

            hb.SetActive(false);

            ContentAddition.AddProjectile(CoinPrefab);

            On.RoR2.BulletAttack.DefaultHitCallbackImplementation += HandleCoinShot;
            On.RoR2.UI.SniperTargetViewer.SetDisplayedTargets += ForceShowCoins;
        }

        private void ForceShowCoins(On.RoR2.UI.SniperTargetViewer.orig_SetDisplayedTargets orig, SniperTargetViewer self, IReadOnlyList<HurtBox> newDisplayedTargets)
        {
            List<HurtBox> targets = newDisplayedTargets.ToList();
            foreach (HurtBox box in HurtBox.readOnlySniperTargetsList) {
                if (box.GetComponentInParent<CoinBehavior>()) {
                    targets.Add(box);
                }
            }

            orig(self, targets);
        }

        public bool HandleCoinShot(On.RoR2.BulletAttack.orig_DefaultHitCallbackImplementation orig, BulletAttack attack, ref BulletAttack.BulletHit info) {
            if (info.collider && info.collider.GetComponentInParent<CoinBehavior>()) {
                Debug.Log("handling coin shot");
                info.collider.GetComponentInParent<CoinBehavior>().ProcessCoinHit(attack, info);
                return true;
            }

            return orig(attack, ref info);
        }

        public class CoinBehaviour : MonoBehaviour, IProjectileImpactBehavior
        {
            public void Start() {
                Physics.IgnoreCollision(transform.Find("HurtBox").GetComponent<SphereCollider>(), GetComponent<SphereCollider>(), true);
                transform.Find("HurtBox").gameObject.SetActive(true);
                transform.Find("HurtBox").GetComponent<HurtBox>().enabled = true;
            }
            public void OnProjectileImpact(ProjectileImpactInfo impactInfo)
            {
                EffectManager.SimpleEffect(Utils.Paths.GameObject.ExplosionSolarFlare.Load<GameObject>(), base.transform.position, Quaternion.identity, false);
                Destroy(base.gameObject);
            }

            public void ProcessCoinHit(BulletAttack attack, BulletAttack.BulletHit hitInfo) {
                Vector3 point = hitInfo.point;
                
                SphereSearch search = new();
                search.radius = 60f;
                search.origin = point;
                search.mask = LayerIndex.entityPrecise.mask;
                search.RefreshCandidates();
                search.FilterCandidatesByDistinctHurtBoxEntities();
                search.FilterCandidatesByHurtBoxTeam(TeamMask.GetUnprotectedTeams(TeamIndex.Player));
                search.OrderCandidatesByDistance();
                HurtBox box = search.GetHurtBoxes().FirstOrDefault(x => x.enabled);

                HurtBox nextCoin = HurtBox.readOnlySniperTargetsList.FirstOrDefault(x => x.GetComponentInParent<CoinBehaviour>() && x.transform.root != base.transform);

                if (nextCoin) {
                    box = nextCoin;
                }
                
                if (box == null) {
                    EffectManager.SimpleEffect(Utils.Paths.GameObject.ExplosionSolarFlare.Load<GameObject>(), point, Quaternion.identity, false);
                    GameObject.Destroy(base.gameObject);
                    return;
                }

                attack.aimVector = (box.transform.position - point).normalized;
                attack.damage *= 1.25f;
                attack.origin = point;
                attack.weapon = base.gameObject;
                attack.muzzleName = null;
                if (attack.owner && attack.owner.GetComponent<CharacterBody>()) {
                    CharacterBody body = attack.owner.GetComponent<CharacterBody>();
                    if (body.teamComponent.teamIndex != TeamIndex.Player) {
                        attack.owner = GetComponent<ProjectileController>().owner;
                    }
                }
                
                EffectManager.SimpleEffect(Utils.Paths.GameObject.ExplosionSolarFlare.Load<GameObject>(), point, Quaternion.identity, false);

                gameObject.SetActive(false);

                GameObject.Destroy(base.gameObject);

                attack.Fire();
            }
        }
    }
}