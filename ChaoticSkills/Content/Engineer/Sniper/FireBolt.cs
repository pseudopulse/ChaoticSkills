using System;

namespace ChaoticSkills.Content.Engineer {
    public class FireBolt : SkillBase<FireBolt> {
        public override SerializableEntityStateType ActivationState => ContentAddition.AddEntityState<EntityStates.Engineer.FireBolt>(out bool _);
        public override float Cooldown => 0;
        public override bool DelayCooldown => false;
        public override string Description => "Fire Bolt";
        public override bool Agile => true;
        public override bool IsCombat => true;
        public override string LangToken => "SniperBolt";
        public override int MaxStock => 1;
        public override int StockToConsume => 0;
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