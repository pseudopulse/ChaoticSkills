using System;
using EntityStates.Commando.CommandoWeapon;

namespace ChaoticSkills.EntityStates.Commando {
    public class SFG : BaseState {
        private GameObject projectilePrefab => Content.Commando.SFG.SFGProjectile;
        private float duration = 1.4f;
        private float damageCoeff = 7f;

        public override void OnEnter()
        {
            base.OnEnter();
            AkSoundEngine.PostEvent(Events.Play_item_use_BFG_fire, base.gameObject);
            duration = duration / base.attackSpeedStat;

            if (base.isAuthority) {
                FireProjectileInfo info = new FireProjectileInfo{
                    damage = base.damageStat * damageCoeff,
                    procChainMask = new(),
                    projectilePrefab = projectilePrefab,
                    position = base.FindModelChild("MuzzleRight").position,
                    crit = base.RollCrit(),
                    rotation = Util.QuaternionSafeLookRotation(base.GetAimRay().direction),
                    owner = base.gameObject
                };

                ProjectileManager.instance.FireProjectile(info);

                if (base.characterMotor) {
                    base.characterMotor.ApplyForce(base.GetAimRay().direction * -350f, true, false);
                }
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.fixedAge >= duration) {
                outer.SetNextStateToMain();
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}