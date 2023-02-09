using System;

namespace ChaoticSkills.Content.Loader {
    public class FirePortalYellow : SkillBase<FirePortalYellow> {
        public override SerializableEntityStateType ActivationState => ContentAddition.AddEntityState<EntityStates.Loader.FireYellowPortal>(out bool _);
        public override float Cooldown => 30f;
        public override bool DelayCooldown => true;
        public override string Description => "Launch a orange portal.";
        public override bool Agile => true;
        public override bool IsCombat => false;
        public override string LangToken => "PORTALYELLOW";
        public override int MaxStock => 1;
        public override UnlockableDef Unlock => null;
        public override int StockToConsume => 1;
        public override string Machine => "Weapon";
        public override Sprite SkillIcon => null;
        public override SkillSlot Slot => SkillSlot.Secondary;
        public override string Survivor => Utils.Paths.GameObject.LoaderBody;
        public override string Name => "Orange Portal";
        public override bool AutoApply => false;

        public override void PostCreation()
        {
            
        }
    }
}