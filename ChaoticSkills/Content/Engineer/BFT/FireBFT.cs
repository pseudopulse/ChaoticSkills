using System;

namespace ChaoticSkills.Content.Engineer {
    public class FireBFT : SkillBase<FireBFT> {
        public override SerializableEntityStateType ActivationState => ContentAddition.AddEntityState<EntityStates.Engineer.FireBFT>(out bool _);
        public override float Cooldown => 9;
        public override bool DelayCooldown => false;
        public override string Description => "Fire Bolt";
        public override bool Agile => true;
        public override bool IsCombat => true;
        public override string LangToken => "BFTBolt";
        public override int MaxStock => 1;
        public override int StockToConsume => 1;
        public override UnlockableDef Unlock => null;
        public override string Machine => "Weapon";
        public override Sprite SkillIcon => null;
        public override SkillSlot Slot => SkillSlot.Primary;
        public override bool AutoApply => false;
        public override string Survivor => Utils.Paths.GameObject.EngiBody;
        public override string Name => "Fire Bolt";
        public override bool Configurable => false;
    }
}