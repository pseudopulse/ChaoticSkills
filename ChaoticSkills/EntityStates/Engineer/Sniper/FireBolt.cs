using System;
using EntityStates.Merc.Weapon;

namespace ChaoticSkills.EntityStates.Engineer {
    public class FireBolt : BaseState {
        private float damageCoeff = 9f;
        private float procCoefficient = 1f;
        private float duration = 2.1f;
        private int shotsFired = 0;
        private float stopwatch = 0f;
        private GameObject tracer = Utils.Paths.GameObject.TracerRailgunCryo.Load<GameObject>();

        public override void OnEnter()
        {
            base.OnEnter();
            BulletAttack attack = new();
            attack.procCoefficient = procCoefficient;
            attack.aimVector = base.GetAimRay().direction;
            attack.muzzleName = "Muzzle";
            attack.origin = base.transform.position;
            attack.damage = base.damageStat * damageCoeff;
            attack.tracerEffectPrefab = tracer;
            attack.smartCollision = false;
            attack.owner = base.gameObject;
            attack.weapon = base.gameObject;
            attack.damageType = DamageType.SlowOnHit;
            attack.isCrit = base.RollCrit();
            AkSoundEngine.PostEvent(Events.Play_railgunner_R_alt_fire, base.gameObject);

            attack.Fire();

            duration /= attackSpeedStat;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.fixedAge >= duration) {
                outer.SetNextStateToMain();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}