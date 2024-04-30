using System;
using RoR2.UI;
using UnityEngine.UI;

namespace ChaoticSkills.Content.MULT {
    public class CraftingTable : SkillBase<CraftingTable> {
        public override SerializableEntityStateType ActivationState => ContentAddition.AddEntityState<EntityStates.MULT.CraftingTable>(out bool _);
        public override float Cooldown => 30f;
        public override bool DelayCooldown => false;
        public override string Description => "<style=cIsUtility>Combine</style> sequences of items to make a <style=cIsDamage>new result</style>.";
        public override bool Agile => false;
        public override bool IsCombat => false;
        public override string LangToken => "CraftingTable";
        public override int StockToConsume => 1;
        public override int MaxStock => 1;
        public override bool SprintCancelable => false;
        public override UnlockableDef Unlock => null;
        public override string Machine => "Body";
        public override Sprite SkillIcon => null;
        public override SkillSlot Slot => SkillSlot.Special;
        public override string Survivor => Utils.Paths.GameObject.ToolbotBody;
        public override bool AutoApply => true;
        public override string Name => "Reassemble";

        public static GameObject CraftingUI;

        public override bool ForceOff => true;

        public override void PostCreation()
        {
            CraftingUI = Utils.Paths.GameObject.ScrapperPickerPanel.Load<GameObject>().InstantiateClone("CraftingPanel");

            CraftingUI.RemoveComponent<PickupPickerPanel>();

            //
            Transform MainPanel = CraftingUI.transform.Find("MainPanel");
            Transform Juice = MainPanel.Find("Juice");
            Transform Icons = Juice.Find("IconContainer");
            Transform Template = Juice.Find("PickupButtonTemplate");
            LanguageTextMeshController Text = Juice.Find("Label").GetComponent<LanguageTextMeshController>();
            //

            CraftingHandler handler = CraftingUI.AddComponent<CraftingHandler>();
            Text.token = "Reassembly Station";

            RecipeManager.CollectRecipes();
        }
    }
}