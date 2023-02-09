using System;

namespace ChaoticSkills.Content.Engineer {
    public class SBFT : SkillBase<SBFT> {
        public override SerializableEntityStateType ActivationState => ContentAddition.AddEntityState<EntityStates.Engineer.PlaceBFT>(out bool _);
        public override float Cooldown => 30f;
        public override bool DelayCooldown => false;
        public override string Description => $"Place a turret that <style=cIsUtility>inherits all your items</style>. Fires a <style=cDeath>giant fucking laser</style> for <style=cIsDamage>{DamageCoeff * 100}%</style> damage on a long cooldown. Can place only 1. ";
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
        public override bool SprintCancelable => false;
        public static float DamageCoeff => Main.config.Bind<float>("BFT", "Damage Coefficient", 13567.5f * 0.01f, "The damage coefficient of the BFT-9000").Value;

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

            PrefabAPI.RegisterNetworkPrefab(BFTBody);
            ContentAddition.AddBody(BFTBody);
            ContentAddition.AddMaster(BFTMaster);

            Misc.AllyCaps.RegisterAllyCap(BFTBody);

            On.RoR2.ModelLocator.UpdateModelTransform += (orig, self, time) => {
                if (self.modelTransform && self.modelTransform.gameObject.name.Contains("BFT")) {
                    if (self.modelParentTransform) {
                        Vector3 position = self.modelParentTransform.position;
                        position -= new Vector3(0, 4f, 0);
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
    }
}