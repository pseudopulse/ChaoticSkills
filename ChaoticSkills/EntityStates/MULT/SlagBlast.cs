using System;
using System.Collections;
using System.Diagnostics.Tracing;
using System.Threading.Tasks;
using EntityStates.Toolbot;
using RoR2.DirectionalSearch;

namespace ChaoticSkills.EntityStates.MULT {
    public class SlagBlast : BaseToolbotPrimarySkillState {
        public float BaseDuration = 2f;
        public float DamageCoefficient = 1.8f;
        public float FireDelay = 0.2f;
        public int ShotCount = 3;
        private float ShotStopwatch = 0f;

        public override string baseMuzzleName => "MuzzleSpear";

        public override void OnEnter()
        {
            base.OnEnter();
            FireDelay /= base.attackSpeedStat;
            BaseDuration /= base.attackSpeedStat;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            ShotStopwatch -= Time.fixedDeltaTime;

            if (ShotStopwatch <= 0 && ShotCount > 0) {
                ShotStopwatch = FireDelay;
                ShotCount--;
                FireShot();

                if (ShotCount == 0) {
                    PlayAnimation("Gesture, Additive", "FireGrenadeLauncher", "FireGrenadeLauncher.playbackRate", BaseDuration - (FireDelay * ShotCount) * 2);
                }
            }
            
            if (base.fixedAge >= BaseDuration) {
                outer.SetNextStateToMain();
            }
        }

        private void FireShot() {
            FireProjectileInfo info = new();
            info.projectilePrefab = Content.MULT.ElfMelter.ElfMelterSlagOrb;
            info.damage = base.damageStat * DamageCoefficient;
            info.crit = base.RollCrit();
            info.owner = base.gameObject;
            info.rotation = Util.QuaternionSafeLookRotation(Util.ApplySpread(base.GetAimRay().direction, -3.6f, 3.6f, 1f, 1f));
            info.position = GetModelChildLocator().FindChild("MuzzleSpear").position;

            PlayAnimation("Gesture, Additive", "FireGrenadeLauncher", "FireGrenadeLauncher.playbackRate", FireDelay);

            AkSoundEngine.PostEvent(Events.Play_MULT_m1_grenade_launcher_shoot, base.gameObject);
            AkSoundEngine.PostEvent(Events.Play_fireballsOnHit_shoot, base.gameObject);

            if (base.isAuthority) ProjectileManager.instance.FireProjectile(info);
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}