using System;
using EntityStates.CaptainSupplyDrop;
using EntityStates.Captain.Weapon;

namespace ChaoticSkills.EntityStates.Captain {
    public class CallVoidBeacon : CallSupplyDropBase {
        public override void OnEnter()
        {
            base.supplyDropPrefab = Content.Captain.VoidBeacon.VoidProjectile;
            base.muzzleflashEffect = Utils.Paths.GameObject.MuzzleflashSupplyDropHealing.Load<GameObject>();
            base.OnEnter();
        }
    }
}