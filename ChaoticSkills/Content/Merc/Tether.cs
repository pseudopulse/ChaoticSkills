using System;

namespace ChaoticSkills.Content.Merc {
    public class Tether : SkillBase<Tether> {
        public override SerializableEntityStateType ActivationState => ContentAddition.AddEntityState<EntityStates.Merc.Tether>(out bool _);
        public override float Cooldown => 9f;
        public override bool DelayCooldown => true;
        public override string Description => "<style=cDeath>Overdriven</style>. Drag targets towards you, dealing <style=cIsDamage>15% damage</style> repeatedly and <style=cIsUtility>slowing</style> them.";
        public override bool Agile => true;
        public override bool IsCombat => true;
        public override string LangToken => "TETHER";
        public override int MaxStock => 1;
        public override UnlockableDef Unlock => null;
        public override string Machine => "Weapon";
        public override Sprite SkillIcon => Main.Assets.LoadAsset<Sprite>("Assets/Icons/Merc/Tether.png");
        public override SkillSlot Slot => SkillSlot.Secondary;
        public override string Survivor => Utils.Paths.GameObject.MercBody;
        public override string Name => "Fluorescence";
        public override List<string> Keywords => new() { "KEYWORD_OVERDRIVE" };

        public override void PostCreation()
        {
            "KEYWORD_OVERDRIVE".Add("<style=cKeywordName>Overdriven</style>Using <style=cDeath>overdriven</style> skills slows you by <style=cDeath>50%</style>");
        }
    }
}