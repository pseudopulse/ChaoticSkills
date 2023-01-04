using System;

namespace ChaoticSkills.Content.Engineer {
    public class Sniper : SkillBase<Sniper> {
        public override SerializableEntityStateType ActivationState => ContentAddition.AddEntityState<EntityStates.Engineer.PlaceSniper>(out bool _);
        public override float Cooldown => 30f;
        public override bool DelayCooldown => false;
        public override string Description => "Place a turret that <style=cIsUtility>inherits all your items</style>. Fires a burst of 3 piercing bolts for <style=cIsDamage>900% damage</style> per shot. Can place only 1. ";
        public override bool Agile => false;
        public override bool IsCombat => true;
        public override string LangToken => "Sniper";
        public override int MaxStock => 1;
        public override UnlockableDef Unlock => null;
        public override string Machine => "Weapon";
        public override Sprite SkillIcon => Main.Assets.LoadAsset<Sprite>("Assets/Icons/Engineer/Sniper.png");
        public override SkillSlot Slot => SkillSlot.Special;
        public override string Survivor => Utils.Paths.GameObject.EngiBody;
        public override string Name => "TR-80 Sniper Turret";
        public static GameObject SniperTurretBody;
        public static GameObject SniperTurretMaster;

        public override void PostCreation()
        {
            base.PostCreation();

            // body
            SniperTurretBody = PrefabAPI.InstantiateClone(Utils.Paths.GameObject.EngiTurretBody.Load<GameObject>(), "SniperTurretBody");

            SkillLocator locator = SniperTurretBody.GetComponent<SkillLocator>();
            SkillFamily family = ScriptableObject.CreateInstance<SkillFamily>();
            (family as ScriptableObject).name = "TurretPrimary";
            family.variants = new SkillFamily.Variant[1];
            
            family.variants[0] = new SkillFamily.Variant {
                skillDef = FireBolt.Instance.SkillDef
            };

            locator.primary._skillFamily = family;

            ModelLocator model = SniperTurretBody.GetComponent<ModelLocator>();
            GameObject muzzleGlow = new("MuzzleGlow");
            Light light = muzzleGlow.AddComponent<Light>();
            light.range = 5f;
            light.color = Color.green;
            light.intensity = 5f;
            ChildLocator childLocator = model.modelTransform.gameObject.GetComponent<ChildLocator>();
            muzzleGlow.transform.parent = childLocator.FindChild("Muzzle");
            muzzleGlow.transform.position = childLocator.FindChild("Muzzle").position;

            // master
            SniperTurretMaster = PrefabAPI.InstantiateClone(Utils.Paths.GameObject.EngiTurretMaster.Load<GameObject>(), "SniperTurretMaser");

            foreach (AISkillDriver driver in SniperTurretMaster.GetComponents<AISkillDriver>()) {
                switch (driver.customName) {
                    case "FireAtEnemy":
                        driver.maxDistance = 180f;
                        driver.activationRequiresTargetLoS = true;
                        break;
                    default:
                        break;
                }
            }

            SniperTurretMaster.GetComponent<CharacterMaster>().bodyPrefab = SniperTurretBody;

            ContentAddition.AddBody(SniperTurretBody);
            ContentAddition.AddMaster(SniperTurretMaster);

            On.RoR2.CharacterBody.Start += HopooWhy;
        }
        // turret cap of 2 is hardcoded so lol lmao
        private void HopooWhy(On.RoR2.CharacterBody.orig_Start orig, CharacterBody self) {
            orig(self);
            if (NetworkServer.active && self.bodyIndex == BodyCatalog.FindBodyIndex(SniperTurretBody)) {
                List<TeamComponent> others = TeamComponent.GetTeamMembers(self.teamComponent.teamIndex).ToList();
                foreach (TeamComponent component in others) {
                    if (component.body && component.body != self && component.body.bodyIndex == BodyCatalog.FindBodyIndex(SniperTurretBody)) {
                        component.body.master.TrueKill();
                    }
                }
            }
        }
    }
}