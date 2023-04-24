/*using System;

namespace ChaoticSkills.Content.Loader {
    public class PortalGun : SkillBase<PortalGun> {
        public override SerializableEntityStateType ActivationState => ContentAddition.AddEntityState<EntityStates.Loader.TargetPortalGun>(out bool _);
        public override float Cooldown => 0.5f;
        public override bool DelayCooldown => true;
        public override string Description => "Launch up to two interconnected portals that teleport to the other upon contact.";
        public override bool Agile => true;
        public override bool IsCombat => false;
        public override string LangToken => "PORTAL";
        public override int MaxStock => 1;
        public override UnlockableDef Unlock => null;
        public override int StockToConsume => 1;
        public override string Machine => "Hook";
        public override Sprite SkillIcon => null;
        public override SkillSlot Slot => SkillSlot.Special;
        public override string Survivor => Utils.Paths.GameObject.LoaderBody;
        public override string Name => "Portal Gun";
        public static GameObject PortalOrangePrefab;
        public static GameObject PortalBluePrefab;
        public static GameObject ActualPortalOrangePrefab;
        public static GameObject ActualPortalBluePrefab;

        public override void PostCreation()
        {
            PortalBluePrefab = Main.Assets.LoadAsset<GameObject>("Assets/Prefabs/Loader/PortalBlueProjectile.prefab");
            PortalOrangePrefab = Main.Assets.LoadAsset<GameObject>("Assets/Prefabs/Loader/PortalYellowProjectile.prefab");
            ActualPortalBluePrefab = Main.Assets.LoadAsset<GameObject>("Assets/Prefabs/Loader/PortalPrefabs/PortalBlue.prefab");
            ActualPortalOrangePrefab = Main.Assets.LoadAsset<GameObject>("Assets/Prefabs/Loader/PortalPrefabs/PortalYellow.prefab");
            PortalBluePrefab.AddComponent<PortalProjectile>().portalPrefab = ActualPortalBluePrefab;
            PortalOrangePrefab.AddComponent<PortalProjectile>().portalPrefab = ActualPortalOrangePrefab;
            ActualPortalBluePrefab.transform.Find("Frame").gameObject.AddComponent<PortalController>().portalType = PortalType.Blue;
            ActualPortalOrangePrefab.transform.Find("Frame").gameObject.AddComponent<PortalController>().portalType = PortalType.Orange;

            PrefabAPI.RegisterNetworkPrefab(ActualPortalBluePrefab);
            PrefabAPI.RegisterNetworkPrefab(ActualPortalOrangePrefab);

            ContentAddition.AddProjectile(PortalBluePrefab);
            ContentAddition.AddProjectile(PortalOrangePrefab);
        }
    }

    public enum PortalType {
        Blue,
        Orange
    }
}*/