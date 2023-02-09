/*using System;
using RoR2.Orbs;

namespace ChaoticSkills.Content.Viend {
    public class WatcherVoid : SkillBase<WatcherVoid> {
        public override SerializableEntityStateType ActivationState => ContentAddition.AddEntityState<EntityStates.Viend.SpawnWatcherVoid>(out bool _);
        public override float Cooldown => 22f;
        public override bool DelayCooldown => false;
        public override string Description => "Deploy a <style=cIsVoid>vo??id watch??er</style> that <style=cIsUtility>siphons</style> <style=cIsVoid>corr??uption</style> from you, and redistributes it as damage to nearby <style=cIsDamage>enemies</style>.";
        public override bool Agile => false;
        public override bool IsCombat => true;
        public override string LangToken => "WatcherVoid";
        public override int StockToConsume => 1;
        public override int MaxStock => 1;
        public override bool SprintCancelable => false;
        public override UnlockableDef Unlock => null;
        public override string Machine => "Weapon";
        public override Sprite SkillIcon => null;
        public override SkillSlot Slot => SkillSlot.Special;
        public override string Survivor => Utils.Paths.GameObject.VoidSurvivorBody;
        public override string Name => "Wa?tche??r";
        public override bool AutoApply => false;
        public static GameObject WatcherVoidPrefab;
        public static GameObject OrbEffectPrefab;

        public override void PostCreation()
        {
            base.PostCreation();
            OrbEffectPrefab = PrefabAPI.InstantiateClone(Utils.Paths.GameObject.ArrowOrbEffect.Load<GameObject>(), "WatcherVoidOrb");
            OrbEffectPrefab.transform.Find("TrailParent").Find("Trail").GetComponent<TrailRenderer>().startColor = Color.red;
            OrbEffectPrefab.transform.Find("TrailParent").Find("Trail").GetComponent<TrailRenderer>().endColor = Color.red;
            OrbEffectPrefab.transform.Find("Quad").GetComponent<MeshRenderer>().material = Utils.Paths.Material.matBloodSiphon.Load<Material>();
            OrbEffectPrefab.transform.Find("Quad").Find("Quad").GetComponent<MeshRenderer>().material = Utils.Paths.Material.matBloodSiphon.Load<Material>();

            WatcherVoidPrefab = Main.Assets.LoadAsset<GameObject>("Assets/Prefabs/Viend/WatcherVoid/WatcherVoid.prefab");
            WatcherVoidPrefab.AddComponent<WatcherVoidBehavior>();

            ContentAddition.AddEffect(OrbEffectPrefab);
        }

        public class WatcherVoidBehavior : MonoBehaviour {
            private VoidSurvivorController ownerVSController;
            private CharacterBody owner;
            public SphereZone zone;
            private bool hasBeenSet = false;
            private float interval = 1f;
            private float lifetime = 13f;
            private float stopwatchInterval = 0f;
            private float stopwatchLifetime = 0f;
            private GameObject deathVfx;

            private void Start() {
                zone = GetComponent<SphereZone>();
                deathVfx = Utils.Paths.GameObject.ExplodeOnDeathVoidExplosionEffect.Load<GameObject>();
            }

            public void SetOwner(CharacterBody body, VoidSurvivorController vsc) {
                ownerVSController = vsc;
                owner = body;
                hasBeenSet = true;
            }

            private void Explode() {
                EffectManager.SpawnEffect(deathVfx, new EffectData {
                    origin = base.transform.position,
                    scale = 3.5f
                }, true);
                Destroy(base.gameObject);
            }

            private void FixedUpdate() {
                stopwatchInterval += Time.fixedDeltaTime;
                stopwatchLifetime += Time.fixedDeltaTime;

                if (stopwatchLifetime >= lifetime) {
                    Explode();
                    return;
                }

                if (!hasBeenSet) return;

                if (stopwatchInterval >= interval) {
                    stopwatchInterval = 0f;
                    HandleSiphon();
                }
            }

            private void HandleSiphon() {
                VoidSiphonOrb orb = new(ownerVSController, 60f, this);
                orb.target = base.GetComponent<HurtBox>();
                orb.origin = ownerVSController.characterBody.corePosition;
                
                OrbManager.instance.AddOrb(orb);
            }
        }

        public class VoidSiphonOrb : Orb {
            private VoidSurvivorController vsc;
            private float speed;
            private WatcherVoidBehavior wvb;
            public override void Begin()
            {
                base.duration = base.distanceToTarget / speed;
                EffectData data = new EffectData {
                    scale = 1f,
                    origin = base.origin,
                    genericFloat = base.duration
                };
                data.SetHurtBoxReference(base.target);
                EffectManager.SpawnEffect(OrbEffectPrefab, data, true);
                vsc.AddCorruption(-25f);
            }

            public VoidSiphonOrb(VoidSurvivorController _vsc, float _speed, WatcherVoidBehavior _wvb) {
                vsc = _vsc;
                speed = _speed;
                wvb = _wvb;
            }

            public override void OnArrival()
            {
                base.OnArrival();
                if (!wvb || !wvb.zone) return;
                TeamComponent[] coms = GameObject.FindObjectsOfType<TeamComponent>().Where(x => wvb.zone.IsInBounds(x.transform.position)).ToArray();

                foreach (TeamComponent com in coms) {
                    if (com.teamIndex != vsc.characterBody.teamComponent.teamIndex && com.body && com.body.mainHurtBox) {
                        VoidDamageOrb orb = new(vsc.characterBody.damage * 4f, 60, vsc.gameObject);
                        orb.origin = target.transform.position;
                        orb.target = com.body.mainHurtBox;

                        OrbManager.instance.AddOrb(orb);
                    }
                }
            }
        }

        public class VoidDamageOrb : Orb {
            private VoidSurvivorController vsc;
            private float speed;
            private float damage;
            private GameObject attacker;
            public override void Begin()
            {
                base.duration = base.distanceToTarget / speed;
                EffectData data = new EffectData {
                    scale = 1f,
                    origin = base.origin,
                    genericFloat = base.duration
                };
                data.SetHurtBoxReference(base.target);
                EffectManager.SpawnEffect(OrbEffectPrefab, data, true);
            }

            public VoidDamageOrb(float _damage, float _speed, GameObject _attacker) {
                speed = _speed;
                attacker = _attacker;
                damage = _damage;
            }

            public override void OnArrival()
            {
                if (target.healthComponent) {
                    DamageInfo info = new();
                    info.damage = damage;
                    info.damageColorIndex = DamageColorIndex.Void;
                    info.procChainMask = new();
                    info.procCoefficient = 1;
                    info.crit = false;
                    info.attacker = attacker;
                    info.position = target.transform.position;

                    target.healthComponent.TakeDamage(info);
                    AkSoundEngine.PostEvent(Events.Play_voidman_m2_explode, target.gameObject);
                    GlobalEventManager.instance.OnHitAll(info, target.healthComponent.gameObject);
                    GlobalEventManager.instance.OnHitEnemy(info, target.healthComponent.gameObject);
                }
            }
        }
    }
}*/