using System;
using RoR2.Orbs;

namespace ChaoticSkills.Content.Acrid {
    public class Pathosis : SkillBase<Pathosis> {
        public override SerializableEntityStateType ActivationState => new SerializableEntityStateType(typeof(Idle));
        public override float Cooldown => 0f;
        public override bool DelayCooldown => true;
        public override string Description => "Poisonous attacks instead apply <style=cIsDamage>Pathosis</style>. Enemies afflicted with Pathosis <style=cDeath>share damage taken</style> with other nearby afflicted enemies.";
        public override bool Agile => true;
        public override bool AgileAddKeyword => false;
        public override bool IsCombat => false;
        public override string LangToken => "Pathosis";
        public override int StockToConsume => 0;
        public override int MaxStock => 1;
        public override UnlockableDef Unlock => null;
        public override string Machine => "Weapon";
        public override Sprite SkillIcon => Main.Assets.LoadAsset<Sprite>("Assets/Icons/Acrid/Pathosis.png");
        public override SkillSlot Slot => SkillSlot.Primary;
        public override string Survivor => Utils.Paths.GameObject.CrocoBody;
        public override bool MustKeyPress => false;
        public override string Name => "Pathosis";
        public override bool Passive => true;
        public static BuffDef PathosisBuff;
        public static DamageTypeCombo PathosisDamageType = (ulong)DamageType.OutOfBounds * 2;
        public static ProcType PathosisMask = (ProcType)((int)ProcType.Count * 39);
        public override void PostCreation()
        {
            base.PostCreation();
            PathosisBuff = ScriptableObject.CreateInstance<BuffDef>();
            PathosisBuff.name = "Pathosis";
            PathosisBuff.buffColor = Color.magenta;
            PathosisBuff.canStack = false;
            PathosisBuff.isCooldown = false;
            PathosisBuff.isDebuff = true;
            PathosisBuff.iconSprite = Main.Assets.LoadAsset<Sprite>("Assets/Icons/Acrid/PathosisDebuff.png");

            ContentAddition.AddBuffDef(PathosisBuff);

            On.RoR2.CrocoDamageTypeController.GetDamageType += (orig, self) => {
                if (self.passiveSkillSlot && self.passiveSkillSlot.skillDef == SkillDef) {
                    return PathosisDamageType | orig(self);
                }
                else {
                    return orig(self);
                }
            };

            On.RoR2.HealthComponent.TakeDamage += (orig, self, info) => {
                orig(self, info);
                if (NetworkServer.active && info.attacker && info.attacker.GetComponent<TeamComponent>()) {
                    // apply pathosis
                    if ((info.damageType.damageTypeCombined & PathosisDamageType) != 0) {
                        self.body.AddTimedBuff(PathosisBuff, 7.5f);
                    }

                    // handle damage sharing
                    if (self.body.HasBuff(PathosisBuff) && !info.procChainMask.HasProc(PathosisMask)) {
                        SphereSearch search = new();
                        search.origin = info.position;
                        search.radius = 40f;
                        search.mask = LayerIndex.entityPrecise.mask;
                        search.queryTriggerInteraction = QueryTriggerInteraction.Ignore;
                        
                        TeamIndex index = info.attacker.GetComponent<TeamComponent>().teamIndex;
                        
                        search.RefreshCandidates();
                        search.FilterCandidatesByDistinctHurtBoxEntities();
                        HurtBox[] boxes = search.GetHurtBoxes();
                        foreach (HurtBox box in boxes) {
                            if (box.teamIndex != index && box.healthComponent && box.healthComponent != self && box.healthComponent.body.HasBuff(PathosisBuff)) {
                                LightningOrb orb = new();
                                orb.lightningType = LightningOrb.LightningType.TreePoisonDart;
                                orb.canBounceOnSameTarget = false;
                                orb.bouncesRemaining = 1;
                                orb.damageColorIndex = DamageColorIndex.Poison;
                                orb.damageValue = info.damage;
                                orb.damageCoefficientPerBounce = 1f;
                                orb.attacker = info.attacker;
                                orb.isCrit = info.crit;
                                orb.target = box;
                                orb.teamIndex = index;
                                orb.targetsToFindPerBounce = 1;
                                orb.speed = 10;
                                orb.origin = info.position;

                                ProcChainMask mask = new();
                                mask.AddProc(PathosisMask);

                                orb.procChainMask = mask;

                                OrbManager.instance.AddOrb(orb);
                            }
                        }
                    }
                }
            };

            On.RoR2.CharacterBody.RecalculateStats += (orig, self) => {
                orig(self);
                if (NetworkServer.active) {
                    if (self.HasBuff(PathosisBuff)) {
                        BurnEffectController controller = self.GetComponent<BurnEffectController>();
                        if (!controller) {
                            controller = self.gameObject.AddComponent<BurnEffectController>();
                            controller.effectType = BurnEffectController.blightEffect;
                            controller.target = self.modelLocator.modelTransform.gameObject;
                        }
                    }
                    else {
                        bool hasBlight = self.HasBuff(RoR2Content.Buffs.Blight);
                        if (!hasBlight) {
                            BurnEffectController controller = self.GetComponent<BurnEffectController>();
                            if (controller) {
                                GameObject.Destroy(controller);
                            }
                        }
                    }
                }
            };
        }
    }
}