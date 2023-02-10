using System;
using EntityStates.Engi.EngiWeapon;
using ChaoticSkills.Content.Engineer;
using R2API.Networking.Interfaces;
using R2API.Networking;

namespace ChaoticSkills.EntityStates.Engineer {
    public class MedicTurretState : BaseState {
        public float uber = 0f;
        private float durationOfUber = 0.5f;
        private GameObject healBeamInstance;
        private GameObject healBeamPrefab = Utils.Paths.GameObject.HealDroneHealBeam.Load<GameObject>();
        private BaseAI ai;
        private bool isUbercharged = false;
        private float maxUber = 100f;
        private float uberRate => maxUber / secondsForFullUber;
        private float uberDrainRate => maxUber / usedUberDuration;
        private float secondsForFullUber = 45f;
        private float usedUberDuration = 8f;
        private bool canBuildUber => target != null;
        private HealthComponent prevTarget;
        public HealthComponent target;
        private HealthComponent healBeamTarget;
        private Medic.UberComponent uberCom;

        public override void OnEnter()
        {
            base.OnEnter();
            ai = base.characterBody.masterObject.GetComponent<BaseAI>();
            uberCom = base.GetComponent<Medic.UberComponent>();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (ai && base.isAuthority) {
                UpdateHealBeam();
            }
        }

        private void UpdateHealBeam() {
            prevTarget = target;
            if ((prevTarget != target || healBeamTarget != target) && healBeamInstance) {
                // target swapped
                NetworkServer.UnSpawn(healBeamInstance);
                Destroy(healBeamInstance);
                new Medic.UberUpdate(isUbercharged, canBuildUber, uber, this.gameObject).Send(NetworkDestination.Clients);
            }

            UpdateTarget();
            if (target) {
                bool isWithinRange = Vector3.Distance(base.characterBody.corePosition, target.transform.position) <= 25;
                if (isWithinRange) {
                    if (!isUbercharged) {
                        uber += uberRate * Time.fixedDeltaTime;

                        if (uber >= maxUber) {
                            isUbercharged = true;
                            new Medic.UberUpdate(isUbercharged, canBuildUber, uber, this.gameObject).Send(NetworkDestination.Clients);
                        }
                    }
                    else {
                        uber -= uberDrainRate * Time.fixedDeltaTime;
                        target.body.AddTimedBuff(RoR2Content.Buffs.Immune, durationOfUber, 1);
                        target.body.AddTimedBuff(RoR2Content.Buffs.FullCrit, durationOfUber, 1);

                        if (uber <= 0) {
                            uber = 0f;
                            isUbercharged = false;
                            new Medic.UberUpdate(isUbercharged, canBuildUber, uber, this.gameObject).Send(NetworkDestination.Clients);
                        }
                    }

                    if (!healBeamInstance) {
                        healBeamInstance = GameObject.Instantiate(healBeamPrefab, base.FindModelChild("Muzzle"));
                        HealBeamController controller = healBeamInstance.GetComponent<HealBeamController>();
                        controller.maxLineWidth = 1.5f;
                        controller.healRate = 2.5f;
                        controller.ownership.ownerObject = base.gameObject;
                        controller.target = target.body.mainHurtBox;
                        healBeamTarget = target;
                        
                        NetworkServer.Spawn(healBeamInstance);
                        new Medic.UberUpdate(isUbercharged, canBuildUber, uber, this.gameObject).Send(NetworkDestination.Clients);
                    }
                }
                else {
                    if (healBeamInstance) {
                        NetworkServer.UnSpawn(healBeamInstance);
                        Destroy(healBeamInstance);
                        new Medic.UberUpdate(isUbercharged, canBuildUber, uber, this.gameObject).Send(NetworkDestination.Clients);
                    }
                }

                uberCom.uber = uber;
            }
            else {
                if (healBeamInstance) {
                    NetworkServer.UnSpawn(healBeamInstance);
                    Destroy(healBeamInstance);
                    new Medic.UberUpdate(isUbercharged, canBuildUber, uber, this.gameObject).Send(NetworkDestination.Clients);
                }
            }

            if (uberCom) {
                uberCom.uber = uber;
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

        public override void OnExit()
        {
            base.OnExit();
            outer.SetNextStateToMain();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Death;
        }
    }
}