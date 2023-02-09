using System;

namespace ChaoticSkills.Content.Engineer {
    public class Constructor : SkillBase<Constructor> {
        public override SerializableEntityStateType ActivationState => ContentAddition.AddEntityState<EntityStates.Engineer.PlaceConstructor>(out bool _);
        public override float Cooldown => 30f;
        public override bool DelayCooldown => false;
        public override string Description => "Place a <style=cIsUtility>Constructor Pylon</style> that <style=cIsDamage>constructs clones</style> of it's targets. Can place only 1.";
        public override bool Agile => false;
        public override bool IsCombat => true;
        public override string LangToken => "Constructor";
        public override int StockToConsume => 0;
        public override int MaxStock => 1;
        public override UnlockableDef Unlock => null;
        public override string Machine => "Weapon";
        public override Sprite SkillIcon => Main.Assets.LoadAsset<Sprite>("Assets/Icons/Engineer/Constructor.png");
        public override SkillSlot Slot => SkillSlot.Special;
        public override string Survivor => Utils.Paths.GameObject.EngiBody;
        public override string Name => "Constructor Pylon";
        public static GameObject ConstructorTurretBody;
        public static GameObject ConstructorTurretMaster;
        public static BuffDef BeingConstructed;
        public static Material ConstructedMaterial;
        public override bool SprintCancelable => false;

        public override void PostCreation()
        {
            base.PostCreation();
            ConstructorTurretBody = Main.Assets.LoadAsset<GameObject>("Assets/Prefabs/Engineer/Constructor/ConstructorBody.prefab");
            ConstructorTurretMaster = Main.Assets.LoadAsset<GameObject>("Assets/Prefabs/Engineer/Constructor/ConstructorMaster.prefab");

            BeingConstructed = ScriptableObject.CreateInstance<BuffDef>();
            BeingConstructed.isHidden = true;
            BeingConstructed.canStack = false;
            BeingConstructed.isDebuff = false;

            ConstructorTurretBody.GetComponent<CharacterBody>().portraitIcon = Utils.Paths.Texture2D.texEngiTurretIcon.Load<Texture>();

            ConstructedMaterial = Utils.Paths.Material.matBlueprintsOk.Load<Material>();
            // ConstructedMaterial = Main.Assets.LoadAsset<Material>("Assets/Prefabs/Engineer/Constructor/vfx/matConstruction.mat");

            ContentAddition.AddBuffDef(BeingConstructed);

            SkillLocator locator = ConstructorTurretBody.GetComponent<SkillLocator>();
            SkillFamily.Variant variant = new SkillFamily.Variant {
                skillDef = ConstructClone.Instance.SkillDef
            };

            locator.primary.skillFamily.variants[0] = variant;

            ContentAddition.AddBody(ConstructorTurretBody);
            ContentAddition.AddMaster(ConstructorTurretMaster);


            On.RoR2.CharacterModel.UpdateOverlays += (orig, self) => {
                orig(self);
                if (self.body && self.body.HasBuff(BeingConstructed)) {
                    self.currentOverlays[self.activeOverlayCount++] = ConstructedMaterial;
                }
            };

            Misc.AllyCaps.RegisterAllyCap(ConstructorTurretBody);

            "CS_CONSTRUCTOR_NAME".Add("Constructor Pylon");
        }
    }
}