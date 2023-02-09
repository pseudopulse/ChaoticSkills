using System;

namespace ChaoticSkills.Content.Engineer {
    public class ConstructClone : SkillBase<ConstructClone> {
        public override SerializableEntityStateType ActivationState => ContentAddition.AddEntityState<EntityStates.Engineer.ConstructClone>(out bool _);
        public override float Cooldown => 10;
        public override bool DelayCooldown => true;
        public override string Description => "Fire Bolt";
        public override bool Agile => true;
        public override bool IsCombat => true;
        public override string LangToken => "ConstructBolt";
        public override int MaxStock => 1;
        public override int StockToConsume => 1;
        public override UnlockableDef Unlock => null;
        public override string Machine => "Weapon";
        public override Sprite SkillIcon => null;
        public override SkillSlot Slot => SkillSlot.Primary;
        public override bool AutoApply => false;
        public override string Survivor => Utils.Paths.GameObject.EngiBody;
        public override string Name => "Construct Clone";
    }
}