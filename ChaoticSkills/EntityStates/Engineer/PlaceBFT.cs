using System;
using EntityStates.Engi.EngiWeapon;

namespace ChaoticSkills.EntityStates.Engineer {
    public class PlaceBFT : PlaceTurret {
        public override void OnEnter()
        {
            base.wristDisplayPrefab = Utils.Paths.GameObject.EngiTurretWristDisplay.Load<GameObject>();
            base.blueprintPrefab = Utils.Paths.GameObject.EngiTurretBlueprints.Load<GameObject>();
            base.turretMasterPrefab = Content.Engineer.SBFT.BFTMaster;
            base.OnEnter();
        }
    }
}