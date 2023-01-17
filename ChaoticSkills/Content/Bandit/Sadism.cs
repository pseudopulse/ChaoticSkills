using System;
using RoR2.Orbs;

namespace ChaoticSkills.Content.Bandit {
    public class Sadism : SkillBase<Sadism> {
        public override SerializableEntityStateType ActivationState => new SerializableEntityStateType(typeof(Idle));
        public override float Cooldown => 0f;
        public override bool DelayCooldown => true;
        public override string Description => "Your attacks <style=cIsDamage>strike an additional time</style> for each <style=cDeath>debuff</style> on the target, dealing <style=cIsDamage>50% TOTAL damage</style>.";
         public override bool Agile => true;
        public override bool AgileAddKeyword => false;
        public override bool IsCombat => false;
        public override string LangToken => "Sadism";
        public override int StockToConsume => 0;
        public override int MaxStock => 1;
        public override UnlockableDef Unlock => null;
        public override string Machine => "Weapon";
        public override Sprite SkillIcon => Main.Assets.LoadAsset<Sprite>("Assets/Icons/Bandit/Sadism.png");
        public override SkillSlot Slot => SkillSlot.Primary;
        public override string Survivor => Utils.Paths.GameObject.Bandit2Body;
        public override bool MustKeyPress => false;
        public override string Name => "Sadism";
        public override bool Passive => true;
        public static CharacterBody.BodyFlags HasSadismPassive = (CharacterBody.BodyFlags)89;
        public static ProcType SadismRepeatStrike = (ProcType)85;

        public override void PostCreation()
        {
            base.PostCreation();
            
            On.RoR2.CharacterBody.Start += (orig, self) => {
                orig(self);
                foreach (GenericSkill skill in self.GetComponents<GenericSkill>()) {
                    if (skill.skillName.ToLower().Contains("passive") && skill.skillDef == SkillDef) {
                        self.bodyFlags -= CharacterBody.BodyFlags.HasBackstabPassive;
                        self.bodyFlags |= HasSadismPassive;
                    }
                }
            };

            On.RoR2.GlobalEventManager.ServerDamageDealt += (orig, report) => {
                orig(report);
                if (report.attackerBody && report.attackerBody.bodyFlags.HasFlag(HasSadismPassive) && report.victimBody) {
                    if (report.damageInfo.procChainMask.HasProc(SadismRepeatStrike)) {
                        return;
                    }
                    int total = 0;
                    foreach (BuffIndex index in BuffCatalog.debuffBuffIndices) {
                        if (report.victimBody.HasBuff(index)) {
                            total++;
                        }
                    }
                    DotController controller = DotController.FindDotController(report.victimBody.gameObject);
                    if (controller) {
                        for (DotController.DotIndex index = DotController.DotIndex.Bleed; index < DotController.DotIndex.Count; index++) {
                            if (controller.HasDotActive(index)) {
                                total++;
                            }
                        }
                    }
                    Debug.Log("total debuffs: " + total);
                    for (int i = 0; i < total; i++) {
                        ProcChainMask mask = new();
                        mask.AddProc(SadismRepeatStrike);
                        DelayedHitOrb orb = new();
                        orb.damageColorIndex = DamageColorIndex.WeakPoint;
                        orb.damageType = DamageType.Generic;
                        orb.attacker = report.attacker;
                        orb.damageValue = report.damageDealt * 0.5f;
                        orb.target = report.victimBody.mainHurtBox;
                        orb.delay = Run.instance.runRNG.RangeFloat(0.3f, 0.9f);
                        orb.isCrit = report.damageInfo.crit;
                        orb.procCoefficient = report.damageInfo.procCoefficient;
                        orb.procChainMask = mask;
                        orb.delayedEffectPrefab = Utils.Paths.GameObject.ImpactLightning.Load<GameObject>();

                        OrbManager.instance.AddOrb(orb);
                    }
                }
            };
        }
    }
}