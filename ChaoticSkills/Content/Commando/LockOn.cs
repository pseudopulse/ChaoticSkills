using System;

namespace ChaoticSkills.Content.Commando {
    public class LockOn : SkillBase<LockOn> {
        public override SerializableEntityStateType ActivationState => ContentAddition.AddEntityState<EntityStates.Commando.LockOn>(out bool _);
        public override float Cooldown => 6;
        public override bool DelayCooldown => true;
        public override string Description => "<style=cIsDamage>Stunning.</style> Fire a barrage of <style=cIsUtility>targeted rockets</style> for <style=cIsDamage>400% damage</style>.";
        public override bool Agile => false;
        public override bool IsCombat => true;
        public override string LangToken => "LockOn";
        public override int StockToConsume => 1;
        public override int MaxStock => 1;
        public override bool SprintCancelable => false;
        public override UnlockableDef Unlock => null;
        public override string Machine => "Weapon";
        public override Sprite SkillIcon => Main.Assets.LoadAsset<Sprite>("Assets/Icons/Commando/LockOn.png");
        public override SkillSlot Slot => SkillSlot.Secondary;
        public override string Survivor => Utils.Paths.GameObject.CommandoBody;
        public override string Name => "Barrage";
        public static GameObject MissileProjectile;
        public static GameObject MissileProjectileGhost;
        public override void PostCreation()
        {
            MissileProjectile = Utils.Paths.GameObject.ToolbotGrenadeLauncherProjectile.Load<GameObject>().InstantiateClone("LockOnRocket");
            MissileProjectileGhost = Utils.Paths.GameObject.ToolbotGrenadeGhost.Load<GameObject>().InstantiateClone("LockOnRocketGhost");
            MissileProjectile.AddComponent<ProjectileTargetComponent>();
            QuaternionPID pid = MissileProjectile.AddComponent<QuaternionPID>();
            pid.PID.x = 10;
            pid.PID.y = 0.3f;
            pid.PID.z = 0f;
            pid.gain = 20;
            MissileController controller = MissileProjectile.AddComponent<MissileController>();
            controller.maxVelocity = 25f;
            controller.rollVelocity = 0f;
            controller.acceleration = 3f;
            controller.delayTimer = 0.3f;
            controller.giveupTimer = 8f;
            controller.deathTimer = 16;
            controller.maxSeekDistance = 5f;
            controller.turbulence = 0f;

            ProjectileSteerTowardTarget steer = MissileProjectile.AddComponent<ProjectileSteerTowardTarget>();
            steer.rotationSpeed = 60f;
            steer.targetComponent = MissileProjectile.GetComponent<ProjectileTargetComponent>();

            MissileProjectile.GetComponent<ProjectileImpactExplosion>().falloffModel = BlastAttack.FalloffModel.None;

            MissileProjectile.GetComponent<ProjectileDamage>().damageType = DamageType.Stun1s;

            MissileProjectile.GetComponent<ProjectileController>().ghostPrefab = MissileProjectileGhost;

            PrefabAPI.RegisterNetworkPrefab(MissileProjectile);

            ContentAddition.AddProjectile(MissileProjectile);

            On.RoR2.CharacterBody.Start += (orig, self) => {
                orig(self);
                bool hasSkill = false;
                foreach (GenericSkill skill in self.GetComponents<GenericSkill>()) {
                    if (skill.skillDef == SkillDef) {
                        hasSkill = true;
                        break;
                    }
                }

                if (hasSkill) {
                    self.AddComponent<LockOnController>(); 
                }
            };
        }

        public class LockOnController : MonoBehaviour {
            private Indicator indicator;
            private GenericSkill secondary;
            private bool isReady => secondary.IsReady();
            public HurtBox target;
            private float stopwatch = 0f;
            private float searchDelay = 0.55f;

            private void Start() {
                indicator = new Indicator(base.gameObject, Utils.Paths.GameObject.EngiMissileTrackingIndicator.Load<GameObject>());
                secondary = GetComponent<SkillLocator>().secondary;
            }

            private void FixedUpdate() {
                stopwatch += Time.fixedDeltaTime;
                if (stopwatch >= searchDelay && isReady) {
                    stopwatch = 0f;
                    BullseyeSearch search = new();
                    search.searchOrigin = GetComponent<CharacterBody>().corePosition;
                    search.maxDistanceFilter = 400;
                    search.maxAngleFilter = 15;
                    search.searchDirection = GetComponent<CharacterBody>().inputBank.aimDirection;
                    search.sortMode = BullseyeSearch.SortMode.Distance;
                    
                    search.RefreshCandidates();
                    search.FilterOutGameObject(base.gameObject);
                    HurtBox tmp = null;
                    foreach (HurtBox box in search.GetResults()) {
                        if (box.teamIndex == TeamIndex.Player) continue;
                        tmp = box;
                        indicator.targetTransform = tmp.transform;
                        indicator.active = true;
                        break;
                    }
                    target = tmp;
                }

                if (!isReady) {
                    indicator.active = false;
                }
            }
        }
    }
}