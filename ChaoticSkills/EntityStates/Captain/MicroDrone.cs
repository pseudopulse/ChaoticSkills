/*using System;
using ChaoticSkills.Content.Captain;

namespace ChaoticSkills.EntityStates.Captain {
    public class MicroDrone : BaseState {
        public override void OnEnter()
        {
            base.OnEnter();
            CaptainDroneTargeter targeter = base.GetComponent<CaptainDroneTargeter>();
            if (targeter && targeter.target) {
                targeter.Fire();
            }
            outer.SetNextStateToMain();
        }
    }
}*/