/*using System;

namespace ChaoticSkills.EntityStates.Loader {
    public class FirePortalBase : BaseState {
        public virtual GameObject portalPrefab { get; }

        public override void OnEnter()
        {
            base.OnEnter();
            if (base.isAuthority) {
                FireProjectileInfo info = new();
                info.owner = base.gameObject;
                info.position = base.characterBody.aimOrigin;
                info.rotation = Util.QuaternionSafeLookRotation(base.GetAimRay().direction);
                info.damage = 0;
                info.crit = false;
                info.projectilePrefab = portalPrefab;

                ProjectileManager.instance.FireProjectile(info);

            }
            AkSoundEngine.PostEvent(Events.Play_item_use_BFG_fire, base.gameObject);
            outer.SetNextStateToMain();
        }
    }

    public class FireYellowPortal : FirePortalBase {
        public override GameObject portalPrefab => Content.Loader.PortalGun.PortalOrangePrefab;
    }

    public class FireBluePortal : FirePortalBase {
        public override GameObject portalPrefab => Content.Loader.PortalGun.PortalBluePrefab;
    }
}*/