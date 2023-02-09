using System;

namespace ChaoticSkills.EntityStates.Viend {
    public class Disrupt : BaseState {
        private GameObject vfxInstance;
        private float damageCoeffPerSecond = 5f;
        private float coeffPerTick => damageCoeffPerSecond / ticks;
        private int ticks = 10;
        private float procCoeffPerSecond = 5;
        private float procCoeffPerTick => procCoeffPerSecond / ticks;
        private float stopwatch = 0f;
        private float delay => 1f / ticks;
        private float radius = 2f;
        private float distance = 20f;
        private float force = -400f;
        private Transform muzzle => FindModelChild("MuzzleHandBeam");

        public override void OnEnter()
        {
            base.OnEnter();
            vfxInstance = GameObject.Instantiate(Utils.Paths.GameObject.VoidJailerCaptureAttackIndicator.Load<GameObject>(), muzzle);
            PlayAnimation("LeftArm, Override", "FireCorruptHandBeam");
            AkSoundEngine.PostEvent(Events.Play_voidman_m1_corrupted_start, base.gameObject);
        }

        public override void OnExit()
        {
            base.OnExit();
            if (vfxInstance) {
                Destroy(vfxInstance);
            }
            PlayAnimation("LeftArm, Override", "ExitHandBeam");
            AkSoundEngine.PostEvent(Events.Play_voidman_m1_corrupted_end, base.gameObject);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            base.characterDirection.forward = base.GetAimRay().direction;
            if (vfxInstance) {
                vfxInstance.transform.forward = base.GetAimRay().direction;
                vfxInstance.transform.Find("GameObject").gameObject.SetActive(true);
            }

            stopwatch += Time.fixedDeltaTime;
            if (stopwatch >= delay) {
                stopwatch = 0f;
                BulletAttack attack = new();
                attack.damage = base.damageStat * coeffPerTick;
                attack.procCoefficient = procCoeffPerTick;
                attack.radius = radius;
                attack.maxDistance = distance;
                attack.origin = muzzle.transform.position;
                attack.muzzleName = "MuzzleHandBeam";
                attack.stopperMask = LayerIndex.noCollision.mask;
                attack.owner = base.gameObject;
                attack.weapon = base.gameObject;
                attack.aimVector = base.GetAimRay().direction;
                attack.isCrit = base.RollCrit();
                attack.force = force;
                
                if (base.isAuthority) {
                    attack.Fire();
                }
            }

            if (!inputBank.skill1.down) {
                outer.SetNextStateToMain();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}