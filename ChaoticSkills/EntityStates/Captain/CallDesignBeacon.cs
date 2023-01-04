using System;
using EntityStates.CaptainSupplyDrop;
using EntityStates.Captain.Weapon;

namespace ChaoticSkills.EntityStates.Captain {
    public class CallDesignBeacon : CallSupplyDropBase {
        public override void OnEnter()
        {
            base.supplyDropPrefab = Content.Captain.DesignBeacon.DesignProjectile;
            base.muzzleflashEffect = Utils.Paths.GameObject.MuzzleflashSupplyDropHealing.Load<GameObject>();
            base.OnEnter();
        }
    }
}