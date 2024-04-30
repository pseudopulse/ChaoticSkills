using System;
using ChaoticSkills.Content.MULT;
using RoR2.UI;

namespace ChaoticSkills.EntityStates.MULT {
    public class CraftingTable : EntityState {
        public GameObject uiInstance;
        public override void OnEnter()
        {
            base.OnEnter();

            GameObject uiInstance = GameObject.Instantiate(Content.MULT.CraftingTable.CraftingUI, HUD.instancesList[0].mainContainer.transform);

            uiInstance.GetComponent<CraftingHandler>().whoUsedUs = characterBody;
            uiInstance.GetComponent<CraftingHandler>().onExit.AddListener(Exit);
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
    }
}