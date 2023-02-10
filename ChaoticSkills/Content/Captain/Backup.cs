using System;

namespace ChaoticSkills.Content.Captain {
    public class Backup : SkillBase<Backup> {
        public override SerializableEntityStateType ActivationState => ContentAddition.AddEntityState<EntityStates.Captain.CallBackup>(out bool _);
        public override float Cooldown => 60f;
        public override bool DelayCooldown => true;
        public override string Description => "Call down <style=cIsUtility>reinforcements</style> from the UES Safe Travels. Lasts 30 seconds.";
        public override bool Agile => false;
        public override bool IsCombat => true;
        public override string LangToken => "Backup";
        public override int StockToConsume => 1;
        public override int MaxStock => 1;
        public override UnlockableDef Unlock => null;
        public override string Machine => "Weapon";
        public override Sprite SkillIcon => Main.Assets.LoadAsset<Sprite>("Assets/Icons/Captain/Crash.png");
        public override SkillSlot Slot => SkillSlot.Utility;
        public override string Survivor => Utils.Paths.GameObject.CaptainBody;
        public override bool MustKeyPress => true;
        public override string Name => "Crash Remote";
        public static GameObject projectilePrefab;
        public static GameObject ghostPrefab;
        public static SkillDef callDropPodDef;
        public override void PostCreation()
        {
            base.PostCreation();
            projectilePrefab = PrefabAPI.InstantiateClone(Utils.Paths.GameObject.RoboCratePod.Load<GameObject>(), "CaptainCrashPod");
            projectilePrefab.RemoveComponent<SurvivorPodController>();
            projectilePrefab.RemoveComponent<BuffPassengerWhileSeated>();
            projectilePrefab.RemoveComponent<VehicleSeat>();
            projectilePrefab.RemoveComponent<NetworkStateMachine>();
            projectilePrefab.RemoveComponent<EntityStateMachine>();
            SphereCollider col = projectilePrefab.AddComponent<SphereCollider>();
            col.radius = 0.5f;

            projectilePrefab.layer = LayerIndex.defaultLayer.intVal;

            ProjectileNetworkTransform networkTransform = projectilePrefab.AddComponent<ProjectileNetworkTransform>();

            ghostPrefab = PrefabAPI.InstantiateClone(new GameObject("Ghost"), "CaptainCrashPodGhost");
            ghostPrefab.AddComponent<ProjectileGhostController>();

            ProjectileController controller = projectilePrefab.AddComponent<ProjectileController>();
            controller.ghostPrefab = ghostPrefab;
            controller.procCoefficient = 0f;
            controller.cannotBeDeleted = true;

            ProjectileDamage damage = projectilePrefab.AddComponent<ProjectileDamage>();
            Rigidbody rb = projectilePrefab.AddComponent<Rigidbody>();

            // projectilePrefab.AddComponent<SpawnRandomSurvivorOnImpact>();
            
            ContentAddition.AddProjectile(projectilePrefab);

            callDropPodDef = ScriptableObject.Instantiate(Utils.Paths.SkillDef.CallAirstrike.Load<SkillDef>());
            GameObject.DontDestroyOnLoad(callDropPodDef);
            callDropPodDef.activationState = ContentAddition.AddEntityState<EntityStates.Captain.CallBackup>(out bool _);
            ContentAddition.AddSkillDef(callDropPodDef);
        }

    }
}