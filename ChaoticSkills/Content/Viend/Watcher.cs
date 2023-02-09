/*using System;
using RoR2.Orbs;

namespace ChaoticSkills.Content.Viend {
    public class Watcher : SkillBase<Watcher> {
        public override SerializableEntityStateType ActivationState => ContentAddition.AddEntityState<EntityStates.Viend.SpawnWatcher>(out bool _);
        public override float Cooldown => 22f;
        public override bool DelayCooldown => false;
        public override string Description => "Deploy a <style=cIsVoid>vo??id watch??er</style> that <style=cIsUtility>siphons</style> <style=cIsVoid>corr??uption</style> from nearby <style=cIsDamage>enemies</style>.";
        public override bool Agile => false;
        public override bool IsCombat => true;
        public override string LangToken => "Watcher";
        public override int StockToConsume => 1;
        public override int MaxStock => 1;
        public override bool SprintCancelable => false;
        public override UnlockableDef Unlock => null;
        public override string Machine => "Weapon";
        public override Sprite SkillIcon => null;
        public override SkillSlot Slot => SkillSlot.Special;
        public override string Survivor => Utils.Paths.GameObject.VoidSurvivorBody;
        public override string Name => "Wa?tche??r";
        public static GameObject WatcherPrefab;
        public static GameObject OrbEffectPrefab;
        public override List<string> Keywords => new() { "CS_CORRUPTION_SECONDARY" };

        public override void PostCreation()
        {
            base.PostCreation();
            OrbEffectPrefab = PrefabAPI.InstantiateClone(Utils.Paths.GameObject.ArrowOrbEffect.Load<GameObject>(), "WatcherOrb");
            OrbEffectPrefab.transform.Find("TrailParent").Find("Trail").GetComponent<TrailRenderer>().startColor = Color.magenta;
            OrbEffectPrefab.transform.Find("TrailParent").Find("Trail").GetComponent<TrailRenderer>().endColor = Color.magenta;
            OrbEffectPrefab.transform.Find("Quad").GetComponent<MeshRenderer>().material = Utils.Paths.Material.matVoidBubble.Load<Material>();
            OrbEffectPrefab.transform.Find("Quad").Find("Quad").GetComponent<MeshRenderer>().material = Utils.Paths.Material.matVoidBubble.Load<Material>();

            WatcherPrefab = Main.Assets.LoadAsset<GameObject>("Assets/Prefabs/Viend/Watcher/Watcher.prefab");
            WatcherPrefab.AddComponent<WatcherBehavior>();

            ContentAddition.AddEffect(OrbEffectPrefab);

            if (WatcherVoid.Instance != null) {
                Misc.VoidSurvivorAlts.RegisterVoidSurvivorAlt(SkillDef, WatcherVoid.Instance.SkillDef);
            } else {
                WatcherVoid.PostCreationEvent += (s, e) => {
                    Misc.VoidSurvivorAlts.RegisterVoidSurvivorAlt(SkillDef, WatcherVoid.Instance.SkillDef);
                };
            }

            "CS_CORRUPTION_SECONDARY".Add("<style=cKeywordName>Corruption Upgrade</style>The Watcher siphons your <style=cIsVoid>corruption</style> and uses it to <style=cIsDamage>strike enemies for 400% damage</style>.");
        }

        public class WatcherBehavior : MonoBehaviour {
            private VoidSurvivorController ownerVSController;
            private CharacterBody owner;
            private SphereZone zone;
            private bool hasBeenSet = false;
            private float interval = 2f;
            private float lifetime = 10f;
            private float stopwatchInterval = 0f;
            private float stopwatchLifetime = 0f;
            private GameObject deathVfx;

            private void Start() {
                zone = GetComponent<SphereZone>();
                deathVfx = Utils.Paths.GameObject.ExplodeOnDeathVoidExplosionEffect.Load<GameObject>();
            }

            public void SetOwner(CharacterBody body, VoidSurvivorController vsc) {
                ownerVSController = vsc;
                owner = body;
                hasBeenSet = true;
            }

            private void Explode() {
                EffectManager.SpawnEffect(deathVfx, new EffectData {
                    origin = base.transform.position,
                    scale = 3.5f
                }, true);
                Destroy(base.gameObject);
            }

            private void FixedUpdate() {
                stopwatchInterval += Time.fixedDeltaTime;
                stopwatchLifetime += Time.fixedDeltaTime;

                if (stopwatchLifetime >= lifetime) {
                    Explode();
                    return;
                }

                if (!hasBeenSet) return;

                if (stopwatchInterval >= interval) {
                    stopwatchInterval = 0f;
                    HandleSiphon();
                }
            }

            private void HandleSiphon() {
                TeamComponent[] coms = GameObject.FindObjectsOfType<TeamComponent>().Where(x => zone.IsInBounds(x.transform.position)).ToArray();

                foreach (TeamComponent com in coms) {
                    if (com.body && com.body.mainHurtBox && com.body != owner) {
                        VoidSiphonOrb orb = new(ownerVSController, 45);
                        orb.origin = base.transform.position;
                        orb.target = com.body.mainHurtBox;

                        OrbManager.instance.AddOrb(orb);
                    }
                }
            }
        }

        public class VoidSiphonOrb : Orb {
            private VoidSurvivorController vsc;
            private float speed;
            public override void Begin()
            {
                base.duration = base.distanceToTarget / speed;
                EffectData data = new EffectData {
                    scale = 1f,
                    origin = base.origin,
                    genericFloat = base.duration
                };
                data.SetHurtBoxReference(base.target);
                EffectManager.SpawnEffect(OrbEffectPrefab, data, true);
            }

            public VoidSiphonOrb(VoidSurvivorController _vsc, float _speed) {
                vsc = _vsc;
                speed = _speed;
            }

            public override void OnArrival()
            {
                base.OnArrival();
                VoidReturnOrb orb = new(vsc, speed);
                orb.origin = target.transform.position;
                orb.target = vsc.characterBody.mainHurtBox;

                OrbManager.instance.AddOrb(orb);
            }
        }

        public class VoidReturnOrb : Orb {
            private VoidSurvivorController vsc;
            private float speed;
            public override void Begin()
            {
                base.duration = base.distanceToTarget / speed;
                EffectData data = new EffectData {
                    scale = 1f,
                    origin = base.origin,
                    genericFloat = base.duration
                };
                data.SetHurtBoxReference(base.target);
                EffectManager.SpawnEffect(OrbEffectPrefab, data, true);
            }

            public VoidReturnOrb(VoidSurvivorController _vsc, float _speed) {
                vsc = _vsc;
                speed = _speed;
            }

            public override void OnArrival()
            {
                vsc.AddCorruption(3.5f);
            }
        }
    }
}*/