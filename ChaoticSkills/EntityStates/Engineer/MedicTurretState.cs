using System;
using EntityStates.Engi.EngiWeapon;

namespace ChaoticSkills.EntityStates.Engineer {
    public class MedicTurretState : BaseState {
        public float uber = 0f;
        private float durationOfUber = 0.5f;
        private float maxUber = 100f;
        private GameObject healBeamInstance;
        private GameObject healBeamPrefab = Utils.Paths.GameObject.HealDroneHealBeam.Load<GameObject>();
        private BaseAI ai;
        private bool isUbercharged = false;
        private HealthComponent prevTarget;
        private HealthComponent target;

        public override void OnEnter()
        {
            base.OnEnter();
            ai = base.characterBody.masterObject.GetComponent<BaseAI>();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.isAuthority && ai) {
                UpdateHealBeam();
            }
        }

        private void UpdateHealBeam() {
            prevTarget = target;
            UpdateTarget();
            if (target != prevTarget) {
                if (healBeamInstance != null) {
                    Destroy(healBeamInstance);
                }
            }
            if (target && Vector3.Distance(base.transform.position, target.body.transform.position) < 25) {
                if (healBeamInstance != null) {
                    if (isUbercharged) {
                        ai.leader.characterBody.AddTimedBuff(RoR2Content.Buffs.FullCrit, durationOfUber, 1);
                        ai.leader.characterBody.AddTimedBuff(RoR2Content.Buffs.Immune, durationOfUber, 1);
                    }
                    else {
                        uber += 3f * Time.fixedDeltaTime;
                    }

                    if (uber >= maxUber) {
                        if (ai.leader.characterBody) {
                            isUbercharged = true;
                            AkSoundEngine.PostEvent(Events.Play_moonBrother_orb_slam_impact, base.gameObject);
                        }
                    }

                    HealBeamController controller = healBeamInstance.GetComponent<HealBeamController>();
                    controller.startPointTransform.parent = base.FindModelChild("Muzzle");
                    controller.startPointTransform.position = base.FindModelChild("Muzzle").position;
                }
                else {
                    healBeamInstance = GameObject.Instantiate(healBeamPrefab, base.FindModelChild("Muzzle"));
                    HealBeamController controller = healBeamInstance.GetComponent<HealBeamController>();
                    controller.healRate = base.damageStat * 0.7f;
                    controller.maxLineWidth = 1.5f;
                    controller.target = target.body.mainHurtBox;
                    controller.ownership.ownerObject = base.gameObject;
                }
            }
            else {
                if (healBeamInstance != null) {
                    Destroy(healBeamInstance);
                }
            }

            if (isUbercharged) {
                uber -= 12.5f * Time.fixedDeltaTime;
                base.characterBody.AddTimedBuff(RoR2Content.Buffs.Immune, durationOfUber, 1);
            }
            if (uber <= 1) {
                uber = 1.1f;
                isUbercharged = false;
            }
        }

        private void UpdateTarget() {
            if (ai.leader.characterBody && ai.leader.characterBody.master) {
                CharacterMaster master = ai.leader.characterBody.master;
                PingerController.PingInfo pingInfo = master.playerCharacterMasterController.pingerController.currentPing;
                if (pingInfo.active && pingInfo.targetGameObject) {
                    if (pingInfo.targetGameObject.GetComponent<TeamComponent>()) {
                        TeamComponent teamComponent = pingInfo.targetGameObject.GetComponent<TeamComponent>();
                        if (teamComponent.teamIndex == base.GetTeam() && teamComponent.body != base.characterBody) {
                            target = teamComponent.body.healthComponent;
                            ai.leader.gameObject = target.gameObject;
                        }
                    }
                }
                else {
                     target = ai.leader.healthComponent;
                }
            }
            else {
                target = ai.leader.healthComponent;
            }
        }
    }
}