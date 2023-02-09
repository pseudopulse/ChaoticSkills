using System;
using ChaoticSkills.Content.Commando;
namespace ChaoticSkills.EntityStates.Commando {
    public class LockOn : BaseState {
        private float delay = 0.15f;
        private float stopwatch = 0f;
        private float damageCoeff = 3f;
        private bool hasAcquiredTarget = false;
        private int fired = 0;
        private Content.Commando.LockOn.LockOnController controller;
        public override void OnEnter()
        {
            base.OnEnter();
            controller = GetComponent<Content.Commando.LockOn.LockOnController>();
            if (controller && controller.target) {
                hasAcquiredTarget = true;
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            stopwatch += Time.fixedDeltaTime;
            if (stopwatch >= delay) {
                stopwatch = 0f;
                fired++;

                FireProjectileInfo info = new();
                info.owner = base.gameObject;
                if (hasAcquiredTarget) info.target = controller.target.gameObject;
                Debug.Log(hasAcquiredTarget);
                info.damage = base.damageStat * damageCoeff;
                info.crit = base.RollCrit();
                info.position = GetModelChildLocator().FindChild("MuzzleRight").transform.position;
                info.rotation = Util.QuaternionSafeLookRotation(base.GetAimRay().direction);
                info.projectilePrefab = Content.Commando.LockOn.MissileProjectile;
                
                if (base.isAuthority) ProjectileManager.instance.FireProjectile(info);

                PlayAnimation("Gesture", "FireFMJ", "FireFMJ.playbackRate", 1f);
                AkSoundEngine.PostEvent(Events.Play_MULT_m1_grenade_launcher_shoot, base.gameObject);
            }

            if (fired >= 3) {
                outer.SetNextStateToMain();
            }
        }
    }
}