using System;

namespace ChaoticSkills.Content.Captain {
    public class Spark : SkillBase<Spark> {
        public override SerializableEntityStateType ActivationState => ContentAddition.AddEntityState<EntityStates.Captain.SparkBeam>(out bool _);
        public override float Cooldown => 0f;
        public override bool DelayCooldown => false;
        public override string Description => "Channel a <style=cIsUtility>electric beam</style> for <style=cIsDamage>340% damage per second</style>. Deals <style=cIsUtility>critical hits</style> after 3 seconds.";
        public override bool Agile => false;
        public override bool IsCombat => true;
        public override string LangToken => "Spark";
        public override int StockToConsume => 0;
        public override int MaxStock => 1;
        public override UnlockableDef Unlock => null;
        public override string Machine => "Weapon";
        public override Sprite SkillIcon => Main.Assets.LoadAsset<Sprite>("Assets/Icons/Captain/Spark.png");
        public override SkillSlot Slot => SkillSlot.Primary;
        public override string Survivor => Utils.Paths.GameObject.CaptainBody;
        public override bool MustKeyPress => true;
        public override string Name => "Spark Cannon";
        public static GameObject BeamPrefab;

        public override void PostCreation()
        {
            base.PostCreation();
            BeamPrefab = Main.Assets.LoadAsset<GameObject>("Assets/Prefabs/Captain/Spark/SparkPrefab.prefab");
        }
    }
}