using System;

namespace ChaoticSkills.Content.Engineer {
    public class Thruster : SkillBase<Thruster> {
        public override SerializableEntityStateType ActivationState => ContentAddition.AddEntityState<EntityStates.Engineer.Thruster>(out bool _);
        public override float Cooldown => 7f;
        public override bool DelayCooldown => false;
        public override string Description => "<style=cIsDamage>Stunning</style>. <style=cIsDamage>Resilient</style>. Launch into the air and burst forward after a delay, dealing <style=cIsDamage>100%-2500%</style> damage upon impact scaling with <style=cIsUtility>fall speed</style>.";
        public override bool Agile => true;
        public override bool IsCombat => true;
        public override string LangToken => "Thruster";
        public override int MaxStock => 2;
        public override UnlockableDef Unlock => null;
        public override string Machine => "Weapon";
        public override Sprite SkillIcon => Main.Assets.LoadAsset<Sprite>("Assets/Icons/Engineer/Thruster.png");
        public override SkillSlot Slot => SkillSlot.Utility;
        public override string Survivor => Utils.Paths.GameObject.EngiBody;
        public override string Name => "Overdrive Boost";
        public override bool MustKeyPress => true;
        public override List<string> Keywords => new() { Utils.Keywords.Stun, "KEYWORD_RESILIENT" };

        public override void PostCreation()
        {
            base.PostCreation();
            "KEYWORD_RESILIENT".Add("<style=cKeywordName>Resilient</style>You are <style=cIsUtility>immune to fall damage</style> during a <style=cIsDamage>Resilient</style> skill.");
        }

    }
}