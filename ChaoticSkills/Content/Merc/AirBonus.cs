using System;
using RoR2.Orbs;

namespace ChaoticSkills.Content.Merc {
    public class AirBonus : SkillBase<AirBonus> {
        public override SerializableEntityStateType ActivationState => new SerializableEntityStateType(typeof(Idle));
        public override float Cooldown => 0f;
        public override bool DelayCooldown => true;
        public override string Description => "Kills while airborne grant a stacking <style=cIsDamage>10% damage boost</style>. All stacks reset upon touching the ground.";
        public override bool Agile => true;
        public override bool AgileAddKeyword => false;
        public override bool IsCombat => false;
        public override string LangToken => "AirBonus";
        public override int StockToConsume => 0;
        public override int MaxStock => 1;
        public override UnlockableDef Unlock => null;
        public override string Machine => "Weapon";
        public override Sprite SkillIcon => Main.Assets.LoadAsset<Sprite>("Assets/Icons/Merc/Momentum.png");
        public override SkillSlot Slot => SkillSlot.Primary;
        public override string Survivor => Utils.Paths.GameObject.MercBody;
        public override bool MustKeyPress => false;
        public override string Name => "Momentum Capacitors";
        public override bool Passive => true;
        public static BuffDef AirBonusStack;
        public static CharacterBody.BodyFlags HasAirBonusPassive = (CharacterBody.BodyFlags)92;

        public override void PostCreation()
        {
            base.PostCreation();

            AirBonusStack = ScriptableObject.CreateInstance<BuffDef>();
            AirBonusStack.canStack = true;
            AirBonusStack.isCooldown = false;
            AirBonusStack.isDebuff = false;
            AirBonusStack.buffColor = Color.blue;
            AirBonusStack.iconSprite = Main.Assets.LoadAsset<Sprite>("Assets/Icons/Merc/AirBonus.png");

            ContentAddition.AddBuffDef(AirBonusStack);
            
            On.RoR2.CharacterBody.Start += (orig, self) => {
                orig(self);
                foreach (GenericSkill skill in self.GetComponents<GenericSkill>()) {
                    if (skill.skillName.ToLower().Contains("passive") && skill.skillDef == SkillDef) {
                        self.baseJumpCount--;
                        self.bodyFlags |= HasAirBonusPassive;
                    }
                }
            };

            On.RoR2.GlobalEventManager.OnCharacterDeath += (orig, self, report) => {
                orig(self, report);
                if (NetworkServer.active && report.attackerBody && report.attackerBody.bodyFlags.HasFlag(HasAirBonusPassive)) {
                    if (report.attackerBody.characterMotor && !report.attackerBody.characterMotor.isGrounded) {
                        report.attackerBody.AddBuff(AirBonusStack);
                    }
                }
            };

            On.RoR2.GlobalEventManager.OnCharacterHitGroundServer += (orig, self, body, vel) => {
                orig(self, body, vel);
                for (int i = 0; i < body.GetBuffCount(AirBonusStack); i++) {
                    body.RemoveBuff(AirBonusStack);
                }
            };

            On.RoR2.CharacterBody.RecalculateStats += (orig, self) => {
                orig(self);
                if (NetworkServer.active) {
                    float bonus = 1f + (0.1f * self.GetBuffCount(AirBonusStack));
                    self.damage *= bonus;
                }
            };
        }
    }
}