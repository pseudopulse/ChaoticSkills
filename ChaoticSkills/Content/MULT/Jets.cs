using System;
using RoR2;

namespace ChaoticSkills.Content.MULT {
    public class Jets : SkillBase<Jets> {
        public override SerializableEntityStateType ActivationState => ContentAddition.AddEntityState<EntityStates.MULT.Jets>(out bool _);
        public override float Cooldown => 5f;
        public override bool DelayCooldown => false;
        public override string Description => "Enable your <style=cIsUtility>Integrated Jets</style> to <style=cIsUtility>fly</style> for a short time.";
        public override bool Agile => true;
        public override bool IsCombat => false;
        public override string LangToken => "Jets";
        public override int StockToConsume => 0;
        public override int MaxStock => 40;
        public override bool SprintCancelable => false;
        public override UnlockableDef Unlock => null;
        public override string Machine => "Thrusters";
        public override Sprite SkillIcon => null;
        public override SkillSlot Slot => SkillSlot.Utility;
        public override string Survivor => Utils.Paths.GameObject.ToolbotBody;
        public override bool AutoApply => false;
        public override string Name => "Integrated Jets";
        public override int StockToRecharge => 40;
        public override bool ResetStockOnOverride => true;

        public override bool ForceOff => false;
        public int animWeaponIndexForSpear;

        public override void PostCreation()
        {
            AddEntityStateMachine<Idle>(GetSurvivor(), "Thrusters");
        }
    }
}