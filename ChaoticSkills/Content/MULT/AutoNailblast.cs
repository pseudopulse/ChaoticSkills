using System;

namespace ChaoticSkills.Content.MULT {
    public class AutoNailblast : SkillBase<AutoNailblast> {
        public override SerializableEntityStateType ActivationState => ContentAddition.AddEntityState<EntityStates.MULT.AutoNailblast>(out bool _);
        public override float Cooldown => 0f;
        public override bool DelayCooldown => false;
        public override string Description => "<style=cDeath>Overdriven</style>. Fire a stream of <style=cIsUtility>shotgun blasts</style> for <style=cIsDamage>6x90%</style> damage.";
        public override bool Agile => false;
        public override bool IsCombat => true;
        public override string LangToken => "AutoNailblast";
        public override int StockToConsume => 0;
        public override int MaxStock => 1;
        public override bool SprintCancelable => false;
        public override UnlockableDef Unlock => null;
        public override string Machine => "Weapon";
        public override Sprite SkillIcon => Main.Assets.LoadAsset<Sprite>("Assets/Icons/MUL-T/AutoNailblast.png");
        public override SkillSlot Slot => SkillSlot.Primary;
        public override string Survivor => Utils.Paths.GameObject.ToolbotBody;
        public override bool AutoApply => false;
        public override string Name => "Auto-Nailblast";
        public override List<string> Keywords => new() { "KEYWORD_OVERDRIVE" };

        public override void PostCreation()
        {
            GameObject surv = Survivor.Load<GameObject>();
            GenericSkill s1 = surv.GetComponents<GenericSkill>().First(x => x.skillName != null && x.skillName == "FireNailgun");
            GenericSkill s2 = surv.GetComponents<GenericSkill>().First(x => x.skillName != null && x.skillName == "FireSpear");
            
            SkillFamily family = s1.skillFamily;

            Array.Resize(ref family.variants, family.variants.Length + 1);
                
            family.variants[family.variants.Length - 1] = new SkillFamily.Variant {
                skillDef = SkillDef,
                unlockableDef = Unlock,
                viewableNode = new ViewablesCatalog.Node(SkillDef.skillNameToken, false, null)
            };

            family = s2.skillFamily;

            Array.Resize(ref family.variants, family.variants.Length + 1);
                
            family.variants[family.variants.Length - 1] = new SkillFamily.Variant {
                skillDef = SkillDef,
                unlockableDef = Unlock,
                viewableNode = new ViewablesCatalog.Node(SkillDef.skillNameToken, false, null)
            };
        }
    }
}