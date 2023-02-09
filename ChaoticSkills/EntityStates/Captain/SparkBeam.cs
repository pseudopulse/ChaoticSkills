using System;
using EntityStates.CaptainSupplyDrop;
using EntityStates.Captain.Weapon;

namespace ChaoticSkills.EntityStates.Captain {
    public class SparkBeam : BaseState {
        private float damagePerSecond = 3.4f;
        private float radius = 1.5f;
        private int tickRate = 9;
        private float damagePerTick => damagePerSecond / tickRate;
        private float delay => 1f / tickRate;
        private float minDuration = 0.2f;
        private float stopwatch = 0f;
        private float procCoefficient = 0.7f;
        private GameObject vfxInstance;
        private GameObject vfxPrefab => Content.Captain.Spark.BeamPrefab;
        private float critDelay = 3f;

        public override void OnEnter()
        {
            base.OnEnter();
            Transform muzzle = GetModelChildLocator().FindChild("MuzzleGun");
            vfxInstance = GameObject.Instantiate(vfxPrefab, muzzle);
            PlayCrossfade("Gesture, Override", "ChargeCaptainShotgun", "ChargeCaptainShotgun.playbackRate", 2, 0.1f);
		    PlayCrossfade("Gesture, Additive", "ChargeCaptainShotgun", "ChargeCaptainShotgun.playbackRate", 2, 0.1f);
            AkSoundEngine.PostEvent(Events.Play_loader_R_active_loop, base.gameObject);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (ShouldEnd()) {
                outer.SetNextStateToMain();
                return;
            }

            base.characterBody.aimTimer = 0.5f;

            if (vfxInstance) {
                vfxInstance.transform.forward = Vector3.Lerp(vfxInstance.transform.forward, base.GetAimRay().direction, 20 * Time.fixedDeltaTime);
            }

            stopwatch += Time.fixedDeltaTime;
            if (stopwatch >= delay) {
                stopwatch = 0f;

                BulletAttack attack = new();
                attack.origin = vfxInstance.transform.position;
                attack.muzzleName = "MuzzleGun";
                attack.damage = base.damageStat * damagePerTick;
                attack.procChainMask = new();
                attack.procCoefficient = procCoefficient;
                attack.falloffModel = BulletAttack.FalloffModel.None;
                attack.maxDistance = 20;
                attack.radius = radius;
                attack.aimVector = base.GetAimRay().direction;
                attack.isCrit = base.fixedAge >= 3f ? true : base.RollCrit();
                attack.weapon = base.gameObject;
                attack.owner = base.gameObject;
                attack.stopperMask = LayerIndex.world.collisionMask;

                attack.Fire();
                AkSoundEngine.PostEvent(Events.Play_loader_R_shock, base.gameObject);
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            Destroy(vfxInstance);
            PlayAnimation("Gesture, Additive", "FireCaptainShotgun");
		    PlayAnimation("Gesture, Override", "FireCaptainShotgun");
            AkSoundEngine.PostEvent(Events.Stop_loader_R_active_loop, base.gameObject);
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }

        internal bool ShouldEnd() {
            return base.fixedAge >= minDuration && !base.inputBank.skill1.down;
        }
    }
}