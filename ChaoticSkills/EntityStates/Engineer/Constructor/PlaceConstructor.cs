using System;
using EntityStates.Engi.EngiWeapon;

namespace ChaoticSkills.EntityStates.Engineer {
    public class PlaceConstructor : PlaceTurret {
        public override void OnEnter()
        {
            if (!base.characterBody.isPlayerControlled) {
                outer.SetNextStateToMain();
                return;
            }
            base.wristDisplayPrefab = Utils.Paths.GameObject.EngiTurretWristDisplay.Load<GameObject>();
            base.blueprintPrefab = Utils.Paths.GameObject.EngiTurretBlueprints.Load<GameObject>();
            base.turretMasterPrefab = Content.Engineer.Constructor.ConstructorTurretMaster;
            base.OnEnter();
        }
    }
}