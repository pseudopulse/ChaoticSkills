using System;
using EntityStates.Engi.EngiWeapon;

namespace ChaoticSkills.EntityStates.Engineer {
    public class PlaceMedic : PlaceTurret {
        public override void OnEnter()
        {
            base.wristDisplayPrefab = Utils.Paths.GameObject.EngiTurretWristDisplay.Load<GameObject>();
            base.blueprintPrefab = Utils.Paths.GameObject.EngiWalkerTurretBlueprints.Load<GameObject>();
            base.turretMasterPrefab = Content.Engineer.Medic.MedicTurretMaster;
            base.OnEnter();
        }
    }
}