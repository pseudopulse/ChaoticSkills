/*using System;
using RoR2.UI;

namespace ChaoticSkills.EntityStates.Loader {
    public class TargetPortalGun : BaseState {
        private SkillLocator locator;
        private SkillDef yellowPortalSkillDef => Content.Loader.FirePortalYellow.Instance.SkillDef;
        private SkillDef bluePortalSkillDef => Content.Loader.FirePortalBlue.Instance.SkillDef;
        private CrosshairUtils.OverrideRequest crosshair;

        public override void OnEnter()
        {
            base.OnEnter();
            locator = base.skillLocator;
            locator.primary.SetSkillOverride(base.gameObject, yellowPortalSkillDef, GenericSkill.SkillOverridePriority.Contextual);
            locator.utility.SetSkillOverride(base.gameObject, bluePortalSkillDef, GenericSkill.SkillOverridePriority.Contextual);
            crosshair = CrosshairUtils.RequestOverrideForBody(base.characterBody, Utils.Paths.GameObject.TreebotCrosshair.Load<GameObject>(), CrosshairUtils.OverridePriority.PrioritySkill);
        }

        public override void OnExit()
        {
            base.OnExit();
            locator.primary.UnsetSkillOverride(base.gameObject, yellowPortalSkillDef, GenericSkill.SkillOverridePriority.Contextual);
            locator.utility.UnsetSkillOverride(base.gameObject, bluePortalSkillDef, GenericSkill.SkillOverridePriority.Contextual);
            crosshair?.Dispose();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (!locator.utility.IsReady() || !locator.primary.IsReady()) {
                outer.SetNextStateToMain();
            }
        }
    }
}*/