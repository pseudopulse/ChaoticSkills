using System;

namespace ChaoticSkills.Content.Captain {
    public class DesignBeacon : SkillBase<DesignBeacon> {
        public override SerializableEntityStateType ActivationState => ContentAddition.AddEntityState<EntityStates.Captain.CallDesignBeacon>(out bool _);
        public override float Cooldown => 0f;
        public override bool DelayCooldown => false;
        public override string Description => "Knocks back and <style=cDeath>cripples</style> ALL nearby characters.";
        public override bool Agile => false;
        public override bool IsCombat => true;
        public override string LangToken => "DesignBeacon";
        public override int StockToConsume => 1;
        public override int MaxStock => 1;
        public override UnlockableDef Unlock => null;
        public override bool AutoApply => false;
        public override string Machine => "Weapon";
        public override Sprite SkillIcon => Main.Assets.LoadAsset<Sprite>("Assets/Icons/Captain/Design.png");
        public override SkillSlot Slot => SkillSlot.Primary;
        public override string Survivor => Utils.Paths.GameObject.CaptainBody;
        public override string Name => "Beacon: Design";
        public static GameObject DesignProjectile;
        public override void PostCreation()
        {
            DesignProjectile = PrefabAPI.InstantiateClone(Utils.Paths.GameObject.CaptainSupplyDropHacking.Load<GameObject>(), "DesignBeacon");

            ModelLocator locator = DesignProjectile.GetComponent<ModelLocator>();
            locator.modelTransform.Find("Indicator").Find("IndicatorRing").GetComponent<MeshRenderer>().material = Utils.Paths.Material.matLunarWardCripple.Load<Material>();
            locator.modelTransform.Find("CaptainSupplyDropMesh").gameObject.GetComponent<SkinnedMeshRenderer>().material = Utils.Paths.Material.matMoonBoulder.Load<Material>();

            EntityStateMachine machine = DesignProjectile.GetComponent<EntityStateMachine>();
            machine.mainStateType = ContentAddition.AddEntityState<EntityStates.Captain.DesignState>(out bool _);

            GameObject survivor = Survivor.Load<GameObject>();

            foreach (GenericSkill skill in survivor.GetComponents<GenericSkill>().Where(x => x.skillName.Contains("SupplyDrop"))) {
                SkillFamily family = skill.skillFamily;
                Array.Resize(ref family.variants, family.variants.Length + 1);
                
                family.variants[family.variants.Length - 1] = new SkillFamily.Variant {
                    skillDef = SkillDef,
                    unlockableDef = Unlock,
                    viewableNode = new ViewablesCatalog.Node(SkillDef.skillNameToken, false, null)
                };
            }
        }
    }
}