using System;

namespace ChaoticSkills.Content.Captain {
    public class VoidBeacon : SkillBase<VoidBeacon> {
        public override SerializableEntityStateType ActivationState => ContentAddition.AddEntityState<EntityStates.Captain.CallVoidBeacon>(out bool _);
        public override float Cooldown => 0f;
        public override bool DelayCooldown => false;
        public override string Description => "<style=cIsVoid>V??oid Fo??g</style> <style=cIsDamage>slowly damages</style> targets within the beacon radius.";
        public override bool Agile => false;
        public override bool IsCombat => true;
        public override string LangToken => "VoidBeacon";
        public override int StockToConsume => 1;
        public override int MaxStock => 1;
        public override UnlockableDef Unlock => null;
        public override bool AutoApply => false;
        public override string Machine => "Weapon";
        public override Sprite SkillIcon => Main.Assets.LoadAsset<Sprite>("Assets/Icons/Captain/Void.png");
        public override SkillSlot Slot => SkillSlot.Primary;
        public override string Survivor => Utils.Paths.GameObject.CaptainBody;
        public override string Name => "Beacon: Void";
        public static GameObject VoidProjectile;
        private static GameObject VoidRadiusIndicator;
        public override void PostCreation()
        {
            VoidProjectile = PrefabAPI.InstantiateClone(Utils.Paths.GameObject.CaptainSupplyDropHacking.Load<GameObject>(), "VoidBeacon");
            GameObject radius = Utils.Paths.GameObject.VoidCamp.Load<GameObject>().transform.Find("mdlVoidFogEmitter").gameObject;
            VoidRadiusIndicator = PrefabAPI.InstantiateClone(radius, "VoidRadius");

            VoidRadiusIndicator.transform.parent = VoidProjectile.transform;

            VoidRadiusIndicator.gameObject.SetActive(false);

            ModelLocator locator = VoidProjectile.GetComponent<ModelLocator>();
            locator.modelTransform.Find("Indicator").Find("IndicatorRing").gameObject.SetActive(false);
            locator.modelTransform.Find("CaptainSupplyDropMesh").gameObject.SetActive(false);
            
            SphereZone zone = VoidProjectile.AddComponent<SphereZone>();
            zone.radius = 10f;
            zone.rangeIndicator = VoidRadiusIndicator.transform.Find("RangeIndicator");
            zone.rangeIndicatorScaleVelocity = 3f;

            // VoidRadiusIndicator.transform.Find("mdlVoidFogEmitterSphere").gameObject.SetActive(false);

            EntityStateMachine machine = VoidProjectile.GetComponent<EntityStateMachine>();
            machine.mainStateType = ContentAddition.AddEntityState<EntityStates.Captain.VoidState>(out bool _);

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