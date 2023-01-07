using System;
using EntityStates.Merc.Weapon;

namespace ChaoticSkills.EntityStates.Engineer {
    public class FireBolt : BaseState {
        private float damageCoeff = 7f;
        private float procCoefficient = 1f;
        private float duration = 2.1f;
        private int shotsFired = 0;
        private float delay = 0.3f;
        private float stopwatch = 0f;
        private GameObject tracer = Utils.Paths.GameObject.TracerRailgunCryo.Load<GameObject>();

        public override void OnEnter()
        {
            base.OnEnter();
            duration = duration / base.attackSpeedStat;
        }

        private void FireBullet() {
            AkSoundEngine.PostEvent(Events.Play_railgunner_R_alt_fire, base.gameObject);

            if (base.isAuthority) {
                BulletAttack attack = new();
                attack.procCoefficient = procCoefficient;
                attack.aimVector = base.GetAimRay().direction;
                attack.muzzleName = "Muzzle";
                attack.origin = base.GetAimRay().origin;
                attack.damage = base.damageStat * damageCoeff;
                attack.tracerEffectPrefab = tracer;
                attack.stopperMask = LayerIndex.world.mask;
                attack.radius = 1f;
                attack.owner = base.gameObject;
                attack.weapon = base.gameObject;
                attack.damageType = DamageType.SlowOnHit;
                attack.isCrit = base.RollCrit();

                attack.Fire();
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.fixedAge >= duration) {
                outer.SetNextStateToMain();
            }

            if (base.isAuthority) {
                stopwatch += Time.fixedDeltaTime;

                if (stopwatch >= delay && shotsFired < 3) {
                    FireBullet();
                    shotsFired++;
                    stopwatch = 0f;
                }
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}