using System;
using EntityStates.Captain.Weapon;
using RoR2.UI;
namespace ChaoticSkills.EntityStates.Artificer {
    public class CallTank : BaseState {
        public override void OnEnter()
        {
            if (NetworkServer.active) {
                GameObject guh = GameObject.Instantiate(Content.Artificer.Tank.TankPrefab, base.characterBody.corePosition + new Vector3(0, 5f, 0), Quaternion.identity);
                guh.GetComponent<TeamComponent>().teamIndex = GetTeam();
                guh.GetComponent<VehicleSeat>().AssignPassenger(base.gameObject);
            }
            outer.SetNextStateToMain();
        }

    }
}