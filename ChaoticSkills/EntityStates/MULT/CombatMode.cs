using System;

namespace ChaoticSkills.EntityStates.MULT {
    public class CombatMode : BaseSkillState {
        public GenericSkill slot;
        public GenericSkill oldSlot;
        public ToolbotWeaponSkillDef sd1;
        public ToolbotWeaponSkillDef sd2;
        public override void OnEnter()
        {
            base.OnEnter();
            skillLocator.utility.SetSkillOverride(characterBody, ChaoticSkills.Content.MULT.Jets.Instance.SkillDef, GenericSkill.SkillOverridePriority.Contextual);
            skillLocator.special.SetSkillOverride(characterBody, ChaoticSkills.Content.MULT.CancelCM.Instance.SkillDef, GenericSkill.SkillOverridePriority.Contextual);
            characterBody.SetBuffCount(ChaoticSkills.Content.MULT.CombatMode.CMBuff.buffIndex, 1);

            slot = skillLocator.FindSkill("FireSpear");
            oldSlot = skillLocator.primary;
            skillLocator.primary = slot;
            characterBody.bodyFlags |= RoR2.CharacterBody.BodyFlags.SprintAnyDirection;

            sd1 = slot.skillDef as ToolbotWeaponSkillDef;
            sd2 = oldSlot.skillDef as ToolbotWeaponSkillDef;

            GetModelAnimator().SetInteger("weaponStance", sd1.animatorWeaponIndex);
            PlayCrossfade("Gesture, Additive", sd1.enterGestureAnimState, 0.2f);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (skillLocator.utility.stock >= skillLocator.utility.maxStock)
            {
                skillLocator.utility.ExecuteIfReady();
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            skillLocator.utility.UnsetSkillOverride(characterBody, ChaoticSkills.Content.MULT.Jets.Instance.SkillDef, GenericSkill.SkillOverridePriority.Contextual);
            skillLocator.special.UnsetSkillOverride(characterBody, ChaoticSkills.Content.MULT.CancelCM.Instance.SkillDef, GenericSkill.SkillOverridePriority.Contextual);
            characterBody.SetBuffCount(ChaoticSkills.Content.MULT.CombatMode.CMBuff.buffIndex, 0);
            skillLocator.primary = oldSlot;
            characterBody.bodyFlags &= ~RoR2.CharacterBody.BodyFlags.SprintAnyDirection;

            GetModelAnimator().SetInteger("weaponStance", sd2.animatorWeaponIndex);
            PlayCrossfade("Gesture, Additive", sd2.enterGestureAnimState, 0.2f);
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}