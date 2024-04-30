using System;
using ChaoticSkills.Content.REX;
using RoR2.UI;

namespace ChaoticSkills.EntityStates.REX {
    public class Execute : EntityState {
        public GameObject uiInstance;
        public override void OnEnter()
        {
            base.OnEnter();

            GameObject uiInstance = GameObject.Instantiate(Content.REX.Execute.UIPrefab, HUD.instancesList[0].mainContainer.transform);
            uiInstance.GetComponent<Content.REX.Execute.ExecuteUIController>().whoSpawnedUs = characterBody;
        }

        public void Exit() {
            outer.SetNextStateToMain();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (!uiInstance) {
                Exit();
            }
        }

        public override void OnExit()
        {
            if (uiInstance) {
                Destroy(uiInstance);
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Death;
        }
    }
}