using System;
using EntityStates.CaptainSupplyDrop;
using EntityStates.Captain.Weapon;

namespace ChaoticSkills.EntityStates.Captain {
    public class ShotgunCharge : BaseState {
        private GameObject prefab => Utils.Paths.GameObject.VoidSurvivorChargeCrabCannon.Load<GameObject>();
        private float duration = 1.5f;
        private GameObject vfxInstance;

        public override void OnEnter()
        {
            base.OnEnter();
            vfxInstance = GameObject.Instantiate(prefab, FindModelChild("MuzzleGun"));
            AkSoundEngine.PostEvent(Events.Play_voidJailer_m1_chargeUp, base.gameObject);
            PlayCrossfade("Gesture, Override", "ChargeCaptainShotgun", "ChargeCaptainShotgun.playbackRate", duration, 0.1f);
		    PlayCrossfade("Gesture, Additive", "ChargeCaptainShotgun", "ChargeCaptainShotgun.playbackRate", duration, 0.1f);
        }

        public override void OnExit()
        {
            base.OnExit();
            PlayAnimation("Gesture, Additive", "FireCaptainShotgun");
		    PlayAnimation("Gesture, Override", "FireCaptainShotgun");
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.fixedAge >= duration) {
                Destroy(vfxInstance);
                outer.SetNextState(new ShotgunFire());
            }
            base.characterDirection.forward = base.GetAimRay().direction;
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}