using System;

namespace ChaoticSkills.Content.Captain {
    public class Gatling : SkillBase<Gatling> {
        public override SerializableEntityStateType ActivationState => ContentAddition.AddEntityState<EntityStates.Captain.ShotgunCharge>(out bool _);
        public override float Cooldown => 0f;
        public override bool DelayCooldown => false;
        public override string Description => "Charge up a devastating blast of <style=cIsVoid>v??oid da??rts</style> for <style=cIsDamage>12x100%</style> damage.";
        public override bool Agile => false;
        public override bool IsCombat => true;
        public override string LangToken => "Gatling";
        public override int StockToConsume => 0;
        public override int MaxStock => 1;
        public override UnlockableDef Unlock => null;
        public override string Machine => "Weapon";
        public override Sprite SkillIcon => Main.Assets.LoadAsset<Sprite>("Assets/Icons/Captain/VoidGaze.png");
        public override SkillSlot Slot => SkillSlot.Primary;
        public override string Survivor => Utils.Paths.GameObject.CaptainBody;
        public override bool MustKeyPress => true;
        public override string Name => "<style=cIsVoid>Gaze of the Void</style>";
    }
}