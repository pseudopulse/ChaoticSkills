using System;
using RoR2;

namespace ChaoticSkills.Content.MULT {
    public class CombatMode : SkillBase<CombatMode> {
        public override SerializableEntityStateType ActivationState => ContentAddition.AddEntityState<EntityStates.MULT.CombatMode>(out bool _);
        public override float Cooldown => 10f;
        public override bool DelayCooldown => true;
        public override string Description => "Take a <style=cIsUtility>mobile stance</style>, increasing your <style=cIsUtility>movement speed</style> and <style=cDeath>lowering defense</style>. <style=cIsDamage>Cycle primaries</style> and replace your utility with <style=cIsUtility>Integrated Jets</style>.";
        public override bool Agile => false;
        public override bool IsCombat => false;
        public override string LangToken => "CombatMode";
        public override int StockToConsume => 1;
        public override int MaxStock => 1;
        public override bool SprintCancelable => false;
        public override UnlockableDef Unlock => null;
        public override string Machine => "ModeSwitch";
        public override Sprite SkillIcon => null;
        public override SkillSlot Slot => SkillSlot.Special;
        public override string Survivor => Utils.Paths.GameObject.ToolbotBody;
        public override bool AutoApply => true;
        public override string Name => "Target Engaged";
        public override int StockToRecharge => 1;
        public override bool ResetStockOnOverride => false;
        public static BuffDef CMBuff;
        public override bool ForceOff => true;

        public override void PostCreation()
        {
            AddEntityStateMachine<Idle>(GetSurvivor(), "ModeSwitch");

            CMBuff = ScriptableObject.CreateInstance<BuffDef>();
            CMBuff.buffColor = Color.red;
            CMBuff.name = "CombatMode";
            CMBuff.isDebuff = false;
            CMBuff.canStack = false;
            CMBuff.iconSprite = Utils.Paths.BuffDef.bdHiddenInvincibility.Load<BuffDef>().iconSprite;

            ContentAddition.AddBuffDef(CMBuff);

            On.RoR2.CharacterBody.RecalculateStats += (orig, self) => {
                orig(self);

                if (self.HasBuff(CMBuff))
                {
                    self.moveSpeed *= 1.4f;
                    self.armor -= 50;
                }
            };
        }
    }
}