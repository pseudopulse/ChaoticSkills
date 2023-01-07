using System;

namespace ChaoticSkills.Content.Engineer {
    public class SBFT : SkillBase<SBFT> {
        public override SerializableEntityStateType ActivationState => ContentAddition.AddEntityState<EntityStates.Engineer.PlaceBFT>(out bool _);
        public override float Cooldown => 30f;
        public override bool DelayCooldown => false;
        public override string Description => "Place a turret that <style=cIsUtility>inherits all your items</style>. Fires a <style=cDeath>giant fucking laser</style> for <style=cIsDamage>13,567.5%</style> damage on a long cooldown. Can place only 1. ";
        public override bool Agile => false;
        public override bool IsCombat => true;
        public override string LangToken => "BFT";
        public override int StockToConsume => 0;
        public override int MaxStock => 1;
        public override UnlockableDef Unlock => null;
        public override string Machine => "Weapon";
        public override Sprite SkillIcon => Main.Assets.LoadAsset<Sprite>("Assets/Icons/Engineer/BFT.png");
        public override SkillSlot Slot => SkillSlot.Special;
        public override string Survivor => Utils.Paths.GameObject.EngiBody;
        public override string Name => "BFT-9000";
        public static GameObject BFTBody;
        public static GameObject BFTMaster;

        public override void PostCreation()
        {
            base.PostCreation();

            // body
            BFTBody = Main.Assets.LoadAsset<GameObject>("Assets/Prefabs/Engineer/BFT/BFTBody.prefab");

            SkillLocator locator = BFTBody.GetComponent<SkillLocator>();
            SkillFamily family = locator.primary.skillFamily;

            family.variants[0] = new SkillFamily.Variant {
                skillDef = FireBFT.Instance.SkillDef
            };

            BFTBody.GetComponent<CharacterDeathBehavior>().deathState = new SerializableEntityStateType(typeof(EntityStates.Engineer.BFTDeath));

            // master
            BFTMaster = PrefabAPI.InstantiateClone(Utils.Paths.GameObject.EngiTurretMaster.Load<GameObject>(), "BFTMaster");

            foreach (AISkillDriver driver in BFTMaster.GetComponents<AISkillDriver>()) {
                switch (driver.customName) {
                    case "FireAtEnemy":
                        driver.maxDistance = float.PositiveInfinity;
                        break;
                    default:
                        driver.maxDistance = 0;
                        driver.enabled = false;
                        break;
                }
            }

            "CS_BFT_NAME".Add("BFT-9000");

            BFTMaster.GetComponent<CharacterMaster>().bodyPrefab = BFTBody;


            ContentAddition.AddBody(BFTBody);
            ContentAddition.AddMaster(BFTMaster);

            On.RoR2.CharacterBody.Start += HopooWhy;

            On.RoR2.ModelLocator.UpdateModelTransform += (orig, self, time) => {
                if (self.modelTransform && self.modelTransform.gameObject.name.Contains("BFT-MODEL")) {
                    if (self.modelParentTransform) {
                        Vector3 position = self.modelParentTransform.position;
                        position -= new Vector3(0, 2f, 0);
                        Quaternion rotation = self.modelParentTransform.rotation;
                        self.UpdateTargetNormal();
                        self.SmoothNormals(time);
                        rotation = Quaternion.FromToRotation(Vector3.up, self.currentNormal) * rotation;
                        self.modelTransform.SetPositionAndRotation(position, rotation);
                    }
                }
                else {
                    orig(self, time);
                }
            };
        }
        // turret cap of 2 is hardcoded so lol lmao
        private void HopooWhy(On.RoR2.CharacterBody.orig_Start orig, CharacterBody self) {
            orig(self);
            int totalCount = 0;
            if (self.bodyIndex != BodyCatalog.FindBodyIndex(BFTBody)) {
                return;
            }
            int maxCap = 1 + self.master.minionOwnership.ownerMaster.inventory.GetItemCount(DLC1Content.Items.EquipmentMagazineVoid);
            if (maxCap > 2) maxCap = 2;
            MinionOwnership[] ownerships = GameObject.FindObjectsOfType<MinionOwnership>().Where(x => x.ownerMaster == self.master.minionOwnership.ownerMaster).ToArray();
            foreach (MinionOwnership ownership in ownerships) {
                if (totalCount >= maxCap) {
                    ownership.GetComponent<CharacterMaster>().TrueKill();
                }
                totalCount += 1;
            }
        }

        
    }
}