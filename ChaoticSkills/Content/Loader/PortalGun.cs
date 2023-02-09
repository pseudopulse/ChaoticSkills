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
        public override SkillSlot Slot => SkillSlot.Secondary;
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
            PortalProjectileController blueCont = PortalBluePrefab.AddComponent<PortalProjectileController>();
            blueCont.portalType = PortalType.Blue;
            PortalProjectileController orangeCont = PortalOrangePrefab.AddComponent<PortalProjectileController>();
            orangeCont.portalType = PortalType.Yellow;
            PortalController blueController = ActualPortalBluePrefab.AddComponent<PortalController>();
            blueController.portalType = PortalType.Blue;
            PortalController orangeController = ActualPortalOrangePrefab.AddComponent<PortalController>();
            orangeController.portalType = PortalType.Yellow;
        }

        public class PortalProjectileController : MonoBehaviour {
            public PortalType portalType;
            public ProjectileImpactEventCaller caller => GetComponent<ProjectileImpactEventCaller>();
            public void Start() {
                caller.impactEvent.AddListener(OnImpact);
            }

            public void OnImpact(ProjectileImpactInfo info) {
                GameObject prefab = null;
                switch (portalType) {
                    case PortalType.Yellow:
                        prefab = ActualPortalOrangePrefab;
                        break;
                    case PortalType.Blue:
                        prefab = ActualPortalBluePrefab;
                        break;
                }

                if (prefab != null) {
                    Debug.Log("spawning portal");
                    Quaternion rotation = Quaternion.Euler(base.transform.forward * -1);
                    if (Physics.Raycast(base.transform.position, -base.transform.forward, out RaycastHit hit)) {
                        rotation = Quaternion.Euler((base.transform.position - hit.point).normalized);
                    }
                    else {
                        rotation = Quaternion.Euler(Vector3.up);
                    }
                    GameObject portal = GameObject.Instantiate(prefab, base.transform.position, rotation);
                }

                GameObject.Destroy(gameObject);
            }
        }

        public class PortalController : MonoBehaviour {
            public PortalType portalType;
            public bool isOnCooldown = false;

            public void Start() {
                PortalController[] portals = GameObject.FindObjectsOfType<PortalController>().Where(x => x.portalType == portalType && x != this).ToArray();
                for (int i = 0; i < portals.Length; i++) {
                    Debug.Log("destroying an already placed portal");
                    GameObject.DestroyImmediate(portals[i].gameObject);
                }
            }

            public void OnTriggerEnter(Collider other) {
                Debug.Log("collided");
                Debug.Log(other.gameObject.name);
                CharacterBody body = null;
                GameObject col = null;
                bool wasProjectie = false;
                if (!other.GetComponent<CharacterBody>() || isOnCooldown) {
                    if (isOnCooldown) {
                        return;
                    }

                    if (other.GetComponent<HurtBox>() && other.GetComponent<HurtBox>().healthComponent) {
                        body = other.GetComponent<HurtBox>().healthComponent.body;
                    }
                    else {
                        col = other.gameObject;
                        wasProjectie = true;
                    }
                }
                else {
                    body = other.GetComponent<CharacterBody>();
                }
                Debug.Log("there was a characterbody");
                PortalController linked = null;
                foreach (PortalController controller in GameObject.FindObjectsOfType<PortalController>().Where(x => (int)x.portalType == (int)portalType * -1)) {
                    linked = controller;
                    break;
                }
                if (linked == null) return;
                linked.EnterCooldown();

                if (wasProjectie) {
                    TeleportHelper.TeleportGameObject(col, linked.transform.position);
                    if (col.GetComponent<Rigidbody>()) {
                        col.GetComponent<Rigidbody>().AddForce(linked.transform.forward * 5000f, ForceMode.Impulse);
                    }
                }
                else {
                    TeleportHelper.TeleportBody(body, linked.transform.position);
                    if (body.rigidbody) {
                        body.rigidbody.AddForce(linked.transform.forward * 5000f, ForceMode.Impulse);
                    }
                }
                EffectManager.SpawnEffect(Utils.Paths.GameObject.ParentTeleportEffect.Load<GameObject>(), new EffectData {
                    origin = linked.transform.position,
                    scale = 2
                }, true);
            }

            public void EnterCooldown() {
                isOnCooldown = true;
                Invoke(nameof(ExitCooldown), 2f);
            }

            private void ExitCooldown() {
                isOnCooldown = false;
            }
        }

        public enum PortalType : int {
            Yellow = -1,
            Blue = 1
        }
    }
}*/