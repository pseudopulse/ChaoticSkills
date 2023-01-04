using System;

namespace ChaoticSkills.Content.Engineer {
    public class Flamethrower : SkillBase<Flamethrower> {
        public override SerializableEntityStateType ActivationState => ContentAddition.AddEntityState<EntityStates.Engineer.Flamethrower>(out bool _);
        public override float Cooldown => 0f;
        public override bool DelayCooldown => false;
        public override string Description => "<style=cIsDamage>Ignite</style>. Fire <style=cIsUtility>dual flamethrowers</style> for <style=cIsDamage>600% damage</style>.";
        public override bool Agile => true;
        public override bool IsCombat => true;
        public override string LangToken => "Flamethrower";
        public override int MaxStock => 1;
        public override int StockToConsume => 0;
        public override UnlockableDef Unlock => null;
        public override string Machine => "Weapon";
        public override Sprite SkillIcon => Main.Assets.LoadAsset<Sprite>("Assets/Icons/Engineer/Flamethrower.png");
        public override SkillSlot Slot => SkillSlot.Primary;
        public override string Survivor => Utils.Paths.GameObject.EngiBody;
        public override List<string> Keywords => new() { Utils.Keywords.Ignite };
        public override string Name => "Thermal Blast";

    }
}