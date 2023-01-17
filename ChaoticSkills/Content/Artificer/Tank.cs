using System;

namespace ChaoticSkills.Content.Artificer {
    public class Tank : SkillBase<Tank> {
        public override SerializableEntityStateType ActivationState => ContentAddition.AddEntityState<EntityStates.Artificer.CallTank>(out bool _);
        public override float Cooldown => 50f;
        public override bool DelayCooldown => true;
        public override string Description => "Deploy a rideable M1A2 SEPV3 Main Battle Tank.";
        public override bool Agile => false;
        public override bool IsCombat => true;
        public override string LangToken => "Tank";
        public override int StockToConsume => 1;
        public override int MaxStock => 1;
        public override UnlockableDef Unlock => null;
        public override string Machine => "Weapon";
        public override Sprite SkillIcon => Main.Assets.LoadAsset<Sprite>("Assets/Icons/Artificer/Tank.png");
        public override SkillSlot Slot => SkillSlot.Utility;
        public override string Survivor => Utils.Paths.GameObject.MageBody;
        public override bool MustKeyPress => false;
        public override string Name => "M1A2 SEPV3 Main Battle Tank";
        public static GameObject TankPrefab;
        public static GameObject TankProjectile;
        public static GameObject TankProjectileGhost;
        public override void PostCreation()
        {
            base.PostCreation();
            TankPrefab = Main.Assets.LoadAsset<GameObject>("Assets/Prefabs/Artificer/Tank/Tank.prefab");
            TankPrefab.AddComponent<TankVehicle>();

            TankProjectile = PrefabAPI.InstantiateClone(Utils.Paths.GameObject.ToolbotGrenadeLauncherProjectile.Load<GameObject>(), "TankProjectile");
            ProjectileImpactExplosion exp = TankProjectile.GetComponent<ProjectileImpactExplosion>();
            exp.blastRadius = 35f;
            exp.falloffModel = BlastAttack.FalloffModel.Linear;
            TankProjectile.transform.localScale *= 5f;

            TankProjectileGhost = PrefabAPI.InstantiateClone(Utils.Paths.GameObject.ToolbotGrenadeGhost.Load<GameObject>(), "TankProjectileGhost");
            TankProjectileGhost.transform.localScale *= 5f;

            TankProjectile.GetComponent<ProjectileController>().ghostPrefab = TankProjectileGhost;

            ContentAddition.AddProjectile(TankProjectile);

            ContentAddition.AddNetworkedObject(TankPrefab);

            "CS_EXIT_TANK".Add("Exit Tank.");

            GlobalEventManager.onCharacterDeathGlobal += (report) => {
                if (report.victim && report.victim.GetComponent<TankVehicle>()) {
                    report.victim.GetComponent<VehicleSeat>().EjectPassenger(report.victim.GetComponent<VehicleSeat>().currentPassengerBody.gameObject);
                    report.victim.GetComponent<TankVehicle>().Explode();
                }
            };
        }

        public class TankVehicle : MonoBehaviour {
            private VehicleSeat seat;
            private Rigidbody rb;
            private Transform top;
            private Transform barrel;
            private Transform aimTarget;
            private Transform originalCamPivot;
            private Transform camPivot;
            private Transform muzzle;
            private bool isReady = true;
            private OverlapAttack attack;
            private float stopwatch;
            private float delay = 0.2f;

            public void Awake() {
                seat = GetComponent<VehicleSeat>();
                seat.onPassengerEnter += OnPassengerEnter;
                seat.onPassengerExit += OnPassengerExit;
                rb = GetComponent<Rigidbody>();
                top = GetComponent<ChildLocator>().FindChild("Top");
                barrel = GetComponent<ChildLocator>().FindChild("Barrel");
                aimTarget = GetComponent<ChildLocator>().FindChild("TankAim");
                camPivot = GetComponent<ChildLocator>().FindChild("Pivot");
                muzzle = GetComponent<ChildLocator>().FindChild("Muzzle");
            }
            
            public void OnPassengerEnter(GameObject passenger) {
                if (passenger.GetComponent<CameraTargetParams>()) {
                    CameraTargetParams ctp = passenger.GetComponent<CameraTargetParams>();
                    if (ctp.cameraPivotTransform) {
                        originalCamPivot = ctp.cameraPivotTransform;
                        ctp.cameraPivotTransform = camPivot;
                    }

                    if (passenger.GetComponent<CharacterBody>()) {
                        attack = new();
                        attack.damage = passenger.GetComponent<CharacterBody>().damage * 0.4f;
                        attack.attacker = passenger;
                        attack.hitBoxGroup = GetComponent<HitBoxGroup>();
                        attack.procCoefficient = 0.3f;
                        attack.teamIndex = passenger.GetComponent<CharacterBody>().teamComponent.teamIndex;
                        attack.attackerFiltering = AttackerFiltering.NeverHitSelf;
                        attack.damageType = DamageType.Generic;
                    }
                }
            }

            public void OnPassengerExit(GameObject passenger) {
                if (passenger.GetComponent<CameraTargetParams>()) {
                    CameraTargetParams ctp = passenger.GetComponent<CameraTargetParams>();
                    if (originalCamPivot) {
                        ctp.cameraPivotTransform = originalCamPivot;
                    }
                }
                Explode();
            }

            public void FixedUpdate() {
                if (seat && seat.currentPassengerBody && seat.currentPassengerInputBank) {
                    Vector3 target = Vector3.MoveTowards(base.transform.position, base.transform.position + (seat.currentPassengerInputBank.moveVector * 14f * Time.fixedDeltaTime), Time.fixedDeltaTime * 50);
                    rb.MovePosition(target);
                    /*top.forward = seat.currentPassengerInputBank.aimDirection;
                    Vector3 guh = top.forward;
                    guh.z = 0;
                    top.forward = guh;

                    Vector3 guh2 = base.transform.rotation.eulerAngles;
                    guh2.z = 0;
                    base.transform.rotation = Quaternion.Euler(guh2);*/

                    seat.currentPassengerBody.AddTimedBuff(RoR2Content.Buffs.HiddenInvincibility, 0.5f);

                    aimTarget.position = seat.currentPassengerInputBank.GetAimRay().GetPoint(300);

                    if (seat.currentPassengerInputBank.skill1.down && isReady) {
                        isReady = false;
                        Invoke(nameof(ResetCooldown), 3f);
                        Fire();
                    }

                    Vector3 guh2 = new Vector3(-90, 0, 0);
                    top.parent.rotation = Quaternion.Euler(guh2);

                    stopwatch += Time.fixedDeltaTime;
                    if (stopwatch >= delay && attack != null) {
                        stopwatch = 0f;
                        attack.ResetIgnoredHealthComponents();
                        attack.Fire();
                    }
                }
            }

            public void Explode() {
                EffectManager.SpawnEffect(Utils.Paths.GameObject.OmniExplosionVFX.Load<GameObject>(), new EffectData {
                    origin = base.transform.position,
                    scale = 23f
                }, true);

                Destroy(base.gameObject);
            }

            public void Fire() {
                FireProjectileInfo info = new();
                info.projectilePrefab = TankProjectile;
                info.damage = seat.currentPassengerBody.damage * 25f;
                info.force = 9000f;
                info.crit = false;
                info.owner = seat.currentPassengerBody.gameObject;
                info.position = muzzle.position;
                info.rotation = Util.QuaternionSafeLookRotation(seat.currentPassengerInputBank.aimDirection);

                ProjectileManager.instance.FireProjectile(info);
            }

            public void ResetCooldown() {
                isReady = true;
            }
        }
    }
}