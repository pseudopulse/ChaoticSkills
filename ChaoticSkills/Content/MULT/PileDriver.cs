using System;
using RoR2;

namespace ChaoticSkills.Content.MULT {
    public class PileDriver : SkillBase<PileDriver> {
        public override SerializableEntityStateType ActivationState => ContentAddition.AddEntityState<EntityStates.MULT.ChargePileDriver>(out bool _);
        public override float Cooldown => 6f;
        public override bool DelayCooldown => true;
        public override string Description => "<style=cIsDamage>Stunning</style>. Charge up and then drive steel rebar into an enemy for <style=cIsDamage>600%-2000% damage</style> with an extremely high force.";
        public override bool Agile => false;
        public override bool IsCombat => false;
        public override string LangToken => "PileDriver";
        public override int StockToConsume => 1;
        public override int MaxStock => 1;
        public override bool SprintCancelable => false;
        public override UnlockableDef Unlock => null;
        public override string Machine => "Weapon";
        public override Sprite SkillIcon => null;
        public override SkillSlot Slot => SkillSlot.Secondary;
        public override string Survivor => Utils.Paths.GameObject.ToolbotBody;
        public override bool AutoApply => true;
        public override string Name => "Puncture";
        public override int StockToRecharge => 1;
        public override bool ResetStockOnOverride => false;
        public override bool ForceOff => false;
        public override List<string> Keywords => new() { Utils.Keywords.Stun };
        public static int animWeaponIndexForSpear;

        public override void PostCreation()
        {
            animWeaponIndexForSpear = Utils.Paths.ToolbotWeaponSkillDef.ToolbotBodyFireSpear.Load<ToolbotWeaponSkillDef>().animatorWeaponIndex;
        }
    }
}