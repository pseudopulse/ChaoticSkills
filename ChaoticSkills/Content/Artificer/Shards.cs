using System;

namespace ChaoticSkills.Content.Artificer {
    public class Shards : SkillBase<Shards> {
        public override SerializableEntityStateType ActivationState => ContentAddition.AddEntityState<EntityStates.Artificer.ChargeShards>(out bool _);
        public override float Cooldown => 3.5f;
        public override bool DelayCooldown => true;
        public override string Description => "Charge up a <style=cIsUtility>tracking bomb</style> for <style=cIsDamage>100%-700% damage</style>.";
        public override bool Agile => true;
        public override bool IsCombat => true;
        public override string LangToken => "Shards";
        public override int StockToConsume => 1;
        public override int MaxStock => 1;
        public override UnlockableDef Unlock => null;
        public override string Machine => "Weapon";
        public override Sprite SkillIcon => Main.Assets.LoadAsset<Sprite>("Assets/Icons/Artificer/Shard.png");
        public override SkillSlot Slot => SkillSlot.Primary;
        public override string Survivor => Utils.Paths.GameObject.MageBody;
        public override bool MustKeyPress => false;
        public override string Name => "Fracture";
        public static GameObject prefab;
        public override void PostCreation()
        {
            base.PostCreation();
            prefab = PrefabAPI.InstantiateClone(Utils.Paths.GameObject.LunarWispTrackingBomb.Load<GameObject>(), "MageShard");
            prefab.GetComponent<ProjectileSteerTowardTarget>().rotationSpeed = 90;
            prefab.GetComponent<ProjectileDirectionalTargetFinder>().lookCone = 40;
            prefab.GetComponent<ProjectileDirectionalTargetFinder>().lookRange = 30;
            prefab.GetComponent<ProjectileController>().startSound = "";

            ContentAddition.AddProjectile(prefab);
        }
    }
}