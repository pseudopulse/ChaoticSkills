using System;

namespace ChaoticSkills.Content.MULT {
    public class CancelCM : SkillBase<CancelCM> {
        public override SerializableEntityStateType ActivationState => ContentAddition.AddEntityState<Idle>(out bool _);
        public override float Cooldown => 0f;
        public override bool DelayCooldown => false;
        public override string Description => "Cancel your <style=cIsUtility>Combat Mode</style>.";
        public override bool Agile => false;
        public override bool IsCombat => false;
        public override string LangToken => "CancelCM";
        public override int StockToConsume => 0;
        public override int MaxStock => 1;
        public override bool SprintCancelable => false;
        public override UnlockableDef Unlock => null;
        public override string Machine => "ModeSwitch";
        public override Sprite SkillIcon => Utils.Paths.SkillDef.ToolbotCancelDualWield.Load<SkillDef>().icon;
        public override SkillSlot Slot => SkillSlot.Special;
        public override string Survivor => Utils.Paths.GameObject.ToolbotBody;
        public override bool AutoApply => false;
        public override string Name => "Cancel";
        public override int StockToRecharge => 1;
        public override InterruptPriority Priority => InterruptPriority.PrioritySkill;
        public override bool MustKeyPress => true;

        public override bool ForceOff => false;

        public override void PostCreation()
        {
            
        }
    }
}