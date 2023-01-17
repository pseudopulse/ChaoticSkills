using System;

namespace ChaoticSkills.EntityStates.Commando {
    public class IceGrenade : BaseState {
        private float duration = 0.5f;

        public override void OnEnter()
        {
            base.OnEnter();
            PlayAnimation("Gesture, Additive", "ThrowGrenade", "FireFMJ.playbackRate", duration);
			PlayAnimation("Gesture, Override", "ThrowGrenade", "FireFMJ.playbackRate", duration);
            if (base.isAuthority) {
                FireProjectileInfo info = new();
                info.projectilePrefab = Content.Commando.IceGrenade.IceGrenadeProjectile;
                info.damage = 0f;
                info.crit = false;
                info.owner = base.gameObject;
                info.rotation = Util.QuaternionSafeLookRotation(base.GetAimRay().direction);
                info.position = base.characterBody.corePosition;

                ProjectileManager.instance.FireProjectile(info);
            }

            AkSoundEngine.PostEvent(Events.Play_commando_M2_grenade_throw, base.gameObject);
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