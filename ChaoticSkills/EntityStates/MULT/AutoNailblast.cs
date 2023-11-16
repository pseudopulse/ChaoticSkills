using System;
using EntityStates.Toolbot;
using RoR2.UI;

namespace ChaoticSkills.EntityStates.MULT {
    public class AutoNailblast : BaseToolbotPrimarySkillState { 
        private float damageCoeff = 0.9f;
        private int bulletCount = 6;
        private float maxSpread = 3f;
        private float shotsPerSecond = 3.5f;
        private float procCoefficient = 0.7f;
        private float delay => 1f / shotsPerSecond;
        private float stopwatch = 0f;
        private CrosshairUtils.OverrideRequest crosshair;
        public override string baseMuzzleName => "MuzzleGrenadeLauncher";
        public override void OnEnter()
        {
            base.OnEnter();
            shotsPerSecond *= base.attackSpeedStat;
            base.characterBody.AddBuff(RoR2Content.Buffs.Slow50);
            crosshair = CrosshairUtils.RequestOverrideForBody(base.characterBody, Utils.Paths.GameObject.ToolbotDualWieldCrosshair.Load<GameObject>(), CrosshairUtils.OverridePriority.PrioritySkill);
            Debug.Log("delay: " + delay);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            stopwatch += Time.fixedDeltaTime;
            if (stopwatch >= delay) {
                stopwatch = 0f;
                AkSoundEngine.PostEvent(Events.Play_captain_m1_shootWide, base.gameObject);

                for (int i = 0; i < bulletCount; i++) {
                    BulletAttack attack = new();
                    attack.owner = base.gameObject;
                    attack.weapon = base.gameObject;
                    attack.muzzleName = muzzleName;
                    attack.damage = base.damageStat * damageCoeff;
                    attack.procCoefficient = procCoefficient;
                    attack.falloffModel = BulletAttack.FalloffModel.Buckshot;
                    attack.maxDistance = 75;
                    attack.tracerEffectPrefab = Utils.Paths.GameObject.TracerToolbotNails.Load<GameObject>();
                    attack.hitEffectPrefab = Utils.Paths.GameObject.ImpactNailgun.Load<GameObject>();
                    attack.minSpread = 0f;
                    attack.maxSpread = maxSpread;
                    attack.aimVector = base.GetAimRay().direction;
                    attack.isCrit = base.RollCrit();
                    attack.origin = base.characterBody.corePosition;
                    attack.radius = 0.01f;

                    if (base.isAuthority) {
                        attack.Fire();
                    }
                }
            }

            if (!IsKeyDownAuthority()) {
                outer.SetNextStateToMain();
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            base.characterBody.RemoveBuff(RoR2Content.Buffs.Slow50);
            crosshair.Dispose();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}