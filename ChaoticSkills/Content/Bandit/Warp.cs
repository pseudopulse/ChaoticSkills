using System;
using RoR2.Orbs;

namespace ChaoticSkills.Content.Bandit {
    public class Warp : SkillBase<Warp> {
        public override SerializableEntityStateType ActivationState => new SerializableEntityStateType(typeof(EntityStates.Bandit.WarpActive));
        public override float Cooldown => 8f;
        public override bool DelayCooldown => true;
        public override string Description => "Negate the next instance of damage, and warp behind the attacker. <style=cIsUtility>Your next attack deals quadruple damage</style>.";
        public override bool Agile => true;
        public override bool AgileAddKeyword => true;
        public override bool IsCombat => false;
        public override string LangToken => "WarpStrike";
        public override int StockToConsume => 1;
        public override int MaxStock => 1;
        public override UnlockableDef Unlock => null;
        public override string Machine => "Weapon";
        public override Sprite SkillIcon => Main.Assets.LoadAsset<Sprite>("Feign.png");
        public override SkillSlot Slot => SkillSlot.Utility;
        public override string Survivor => Utils.Paths.GameObject.Bandit2Body;
        public override bool MustKeyPress => false;
        public override string Name => "Feign";

        public static BuffDef cloakBuff;
        public static BuffDef counterStrike;

        public override void PostCreation()
        {
            base.PostCreation();

            cloakBuff = ScriptableObject.CreateInstance<BuffDef>();
            cloakBuff.buffColor = Color.black;
            cloakBuff.canStack = false;
            cloakBuff.iconSprite = Utils.Paths.BuffDef.bdPulverizeBuildup.Load<BuffDef>().iconSprite;

            counterStrike = ScriptableObject.CreateInstance<BuffDef>();
            counterStrike.buffColor = Color.black;
            counterStrike.canStack = false;
            counterStrike.iconSprite = Utils.Paths.BuffDef.bdFullCrit.Load<BuffDef>().iconSprite;

            ContentAddition.AddBuffDef(counterStrike);
            ContentAddition.AddBuffDef(cloakBuff);

            On.RoR2.HealthComponent.TakeDamage += (orig, self, info) => {
                if (self.body.HasBuff(cloakBuff)) {
                    info.rejected = true;
                    
                    if (info.attacker && info.attacker.GetComponent<CharacterBody>()) {
                        CharacterBody cb = info.attacker.GetComponent<CharacterBody>();

                        Vector3 warpPos = cb.corePosition + (5 * (-cb.transform.forward));

                        AkSoundEngine.PostEvent(Events.Play_bandit2_shift_enter, self.gameObject);
                        EffectManager.SpawnEffect(Utils.Paths.GameObject.Bandit2SmokeBomb.Load<GameObject>(), new EffectData {
                            origin = self.transform.position
                        }, true);

                        EffectManager.SpawnEffect(Utils.Paths.GameObject.Bandit2SmokeBomb.Load<GameObject>(), new EffectData {
                            origin = warpPos
                        }, true);

                        Vector3? vec = cb.isFlying ? warpPos : TeleportHelper.FindSafeTeleportDestination(warpPos, self.body, Run.instance.spawnRng);

                        TeleportHelper.TeleportBody(self.body, vec.Value);
                        self.body.AddBuff(counterStrike);
                        self.body.RemoveBuff(cloakBuff);
                    }
                }

                if (info.attacker && info.attacker.GetComponent<CharacterBody>()) {
                    if (info.attacker.GetComponent<CharacterBody>().HasBuff(counterStrike)) {
                        info.attacker.GetComponent<CharacterBody>().RemoveBuff(counterStrike);
                        info.damage *= 4f;
                        info.damageColorIndex = DamageColorIndex.WeakPoint;
                    }
                }

                orig(self, info);
            };

        }
    }
}