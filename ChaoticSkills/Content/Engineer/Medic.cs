using System;

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

            On.RoR2.CharacterBody.Start += HopooWhy;
            On.RoR2.UI.AllyCardController.UpdateInfo += Uber;
        }
        // turret cap of 2 is hardcoded so lol lmao
        private void HopooWhy(On.RoR2.CharacterBody.orig_Start orig, CharacterBody self) {
            orig(self);
            if (NetworkServer.active && self.bodyIndex == BodyCatalog.FindBodyIndex(MedicTurretBody)) {
                List<TeamComponent> others = TeamComponent.GetTeamMembers(self.teamComponent.teamIndex).ToList();
                foreach (TeamComponent component in others) {
                    if (component.body && component.body != self && component.body.bodyIndex == BodyCatalog.FindBodyIndex(MedicTurretBody)) {
                        component.body.master.TrueKill();
                    }
                }
            }
        }

        private void Uber(On.RoR2.UI.AllyCardController.orig_UpdateInfo orig, RoR2.UI.AllyCardController self) {
            orig(self);
            if (self.sourceMaster.GetBody()) {
                CharacterBody body = self.sourceMaster.GetBody();
                if (body.bodyIndex != BodyCatalog.FindBodyIndex(MedicTurretBody)) {
                    return;
                }
                EntityStateMachine machine = body.GetComponents<EntityStateMachine>().First(x => x.customName == "Weapon");
                EntityStates.Engineer.MedicTurretState state;
                if ((state = machine.state as EntityStates.Engineer.MedicTurretState) != null) {
                    string text = Util.GetBestBodyName(body.gameObject);
                    text = text + $" ({Mathf.RoundToInt(state.uber)}%)";
                    self.nameLabel.text = text;
                }

            }
        }
    }
}