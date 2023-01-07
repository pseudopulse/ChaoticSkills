using System;

namespace ChaoticSkills.EntityStates.Engineer {
    public class BFTDeath : BaseState {
        public override void OnEnter() {
            Destroy(base.GetModelBaseTransform().gameObject);
            Destroy(base.GetModelTransform().gameObject);
            Destroy(base.characterBody.master.gameObject);
            Destroy(base.characterBody.gameObject);
        }
    }
}