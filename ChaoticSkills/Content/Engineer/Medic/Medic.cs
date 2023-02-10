using System;
using R2API.Networking;
using R2API.Networking.Interfaces;
using ChaoticSkills.EntityStates.Engineer;

namespace ChaoticSkills.Content.Engineer {
    public class Medic : SkillBase<Medic> {
        public override SerializableEntityStateType ActivationState => ContentAddition.AddEntityState<EntityStates.Engineer.PlaceMedic>(out bool _);
        public override float Cooldown => 30f;
        public override bool DelayCooldown => false;
        public override string Description => "Place a <style=cIsUtility>mobile</style> turret that <style=cIsUtility>inherits all your items</style>. <style=cIsHealing>Heals</style> it's owner and grants <style=cIsUtility>invulnerability</style> and <style=cIsDamage>guaranteed crits</style> for 8 seconds after healing enough. Can place only 1. ";
        public override bool Agile => false;
        public override bool IsCombat => true;
        public override string LangToken => "MedicTurret";
        public override int StockToConsume => 0;
        public override int MaxStock => 1;
        public override UnlockableDef Unlock => null;
        public override string Machine => "Weapon";
        public override Sprite SkillIcon => Main.Assets.LoadAsset<Sprite>("Assets/Icons/Engineer/Medic.png");
        public override SkillSlot Slot => SkillSlot.Special;
        public override string Survivor => Utils.Paths.GameObject.EngiBody;
        public override bool SprintCancelable => false;
        public override string Name => "PM-30 Support Turret";
        public static GameObject MedicTurretBody;
        public static GameObject MedicTurretMaster;

        public override void PostCreation()
        {
            base.PostCreation();

            // body
            MedicTurretBody = PrefabAPI.InstantiateClone(Utils.Paths.GameObject.EngiWalkerTurretBody.Load<GameObject>(), "MedicTurretBody");

            CharacterBody cb = MedicTurretBody.GetComponent<CharacterBody>();
            cb.baseMaxHealth *= 2f;
            cb.baseRegen = 7.5f;
            cb.baseArmor = 35f;
            cb.baseMoveSpeed *= 1.9f;

            MedicTurretBody.AddComponent<UberComponent>();


            EntityStateMachine machine = MedicTurretBody.GetComponents<EntityStateMachine>().First(x => x.customName == "Weapon");
            SerializableEntityStateType state = ContentAddition.AddEntityState<EntityStates.Engineer.MedicTurretState>(out bool _);
            machine.mainStateType = state;
            machine.initialStateType = state;


            // master
            MedicTurretMaster = PrefabAPI.InstantiateClone(Utils.Paths.GameObject.EngiWalkerTurretMaster.Load<GameObject>(), "MedicTurretMaser");

            foreach (AISkillDriver driver in MedicTurretMaster.GetComponents<AISkillDriver>()) {
                switch (driver.customName) {
                    case "StrafeAndFireAtEnemy":
                        driver.minDistance = 0f;
                        driver.maxDistance = 5f;
                        driver.skillSlot = SkillSlot.None;
                        driver.movementType = AISkillDriver.MovementType.StrafeMovetarget;
                        driver.moveTargetType = AISkillDriver.TargetType.CurrentLeader;
                        break;
                    case "ChaseAndFireAtEnemy":
                        driver.moveTargetType = AISkillDriver.TargetType.CurrentLeader;
                        driver.movementType = AISkillDriver.MovementType.ChaseMoveTarget;
                        driver.maxDistance = float.PositiveInfinity;
                        driver.minDistance = 5f;
                        driver.aimType = AISkillDriver.AimType.AtCurrentLeader;
                        driver.skillSlot = SkillSlot.None;
                        break;
                    default:
                        break;
                }
            }

            MedicTurretMaster.GetComponent<CharacterMaster>().bodyPrefab = MedicTurretBody;

            ContentAddition.AddBody(MedicTurretBody);
            ContentAddition.AddMaster(MedicTurretMaster);

            Misc.AllyCaps.RegisterAllyCap(MedicTurretBody);

            NetworkingAPI.RegisterMessageType<UberUpdate>();
            
            On.RoR2.UI.AllyCardController.UpdateInfo += Uber;
        }

        private void Uber(On.RoR2.UI.AllyCardController.orig_UpdateInfo orig, RoR2.UI.AllyCardController self) {
            orig(self);
            if (self.sourceMaster.GetBody()) {
                CharacterBody body = self.sourceMaster.GetBody();
                if (body.bodyIndex != BodyCatalog.FindBodyIndex(MedicTurretBody)) {
                    return;
                }
                UberComponent com = body.GetComponent<UberComponent>();
                if (com) {
                    string text = Util.GetBestBodyName(body.gameObject);
                    text = text + $" ({Mathf.RoundToInt((float)com.uber)}%)";
                    self.nameLabel.text = text;
                }
            }
        }

        public class UberComponent : MonoBehaviour {
            public float uber = 0;
            public bool isUbercharged = false;
            public bool canBuildUber = false;
            private float maxUber = 100f;
            private float uberRate => maxUber / secondsForFullUber;
            private float uberDrainRate => maxUber / usedUberDuration;
            private float secondsForFullUber = 45f;
            private float usedUberDuration = 8f;

            public void FixedUpdate() {
                if (canBuildUber) {
                    if (!isUbercharged) {
                        uber += uberRate * Time.fixedDeltaTime;
                    }
                }

                if (isUbercharged) {
                    uber -= uberDrainRate * Time.fixedDeltaTime;
                }
            }
        }

        public class UberUpdate : INetMessage {
            private bool isUbercharged;
            private bool canBuildUber;
            private float uber;
            private GameObject target;

            public UberUpdate() {}

            public UberUpdate(bool x, bool y, float z, GameObject t) {
                isUbercharged = x;
                canBuildUber = y;
                uber = z;
                target = t;
            }

            public void Serialize(NetworkWriter writer) {
                writer.Write(isUbercharged);
                writer.Write(canBuildUber);
                writer.Write((double)uber);
                writer.Write(target);
            }

            public void Deserialize(NetworkReader reader) {
                isUbercharged = reader.ReadBoolean();
                canBuildUber = reader.ReadBoolean();
                uber = (float)reader.ReadDouble();
                target = reader.ReadGameObject();
            }

            public void OnReceived() {
                if (target) {
                    UberComponent com = target.GetComponent<UberComponent>();
                    if (com) {
                        com.uber = uber;
                        com.canBuildUber = canBuildUber;
                        com.isUbercharged = isUbercharged;
                    }
                }
            }
        }
    }
}