/*using System;

namespace ChaoticSkills.Content.Captain.Drones {
    public class OffenseDrone : SkillBase<OffenseDrone> {
        public override SerializableEntityStateType ActivationState => new SerializableEntityStateType(typeof(Idle));
        public override float Cooldown => 0f;
        public override bool DelayCooldown => false;
        public override string Description => "The <style=cIsUtility>micro-drone</style> will <style=cIsDamage>spin around a target</style>, firing a <style=cIsDamage>slowing laser</style> at them for <style=cIsDamage>90% damage per second</style>.";
        public override bool Agile => true;
        public override bool AgileAddKeyword => false;
        public override bool IsCombat => true;
        public override string LangToken => "OffenseDrone";
        public override int StockToConsume => 0;
        public override int MaxStock => 0;
        public override UnlockableDef Unlock => null;
        public override string Machine => "Weapon";
        public override Sprite SkillIcon => null;
        public override SkillSlot Slot => SkillSlot.Secondary;
        public override string Survivor => Utils.Paths.GameObject.CaptainBody;
        public override bool MustKeyPress => true;
        public override bool AutoApply => false;
        public override string Name => "Offense Drone";
        public static GameObject OffenseDronePrefab;
        private static Material droneMat;
        public override void PostCreation()
        {
            base.PostCreation();
            // actual drone
            OffenseDronePrefab = PrefabAPI.InstantiateClone(Utils.Paths.GameObject.PickupCaptainDefenseMatrix.Load<GameObject>(), "OffenseDrone");
            OffenseDronePrefab.AddComponent<OffenseDroneController>();
            OffenseDronePrefab.transform.localPosition = Vector3.zero;
            OffenseDronePrefab.transform.localScale *= 0.7f;
            MeshRenderer renderer = OffenseDronePrefab.GetComponentInChildren<MeshRenderer>();
            Material mat = GameObject.Instantiate(Utils.Paths.Material.matCaptainRobotBits.Load<Material>());
            mat.color = Color.black;
            GameObject.DontDestroyOnLoad(mat);
            renderer.material = mat;
            // handling the selectable matching
            if (MicroDrones.Instance != null) {
                HandleDrone(null, null);
            } else {
                MicroDrones.PostCreationEvent += HandleDrone;
            }
        }

        public class OffenseDroneController : CaptainDrone {
            private GameObject laserInstance;
            private Transform laserEnd;
            private LineRenderer lr;
            private float stopwatch = 0f;
            private float damageCoeffPerTick=> 0.9f / 3;
            private float delay = 1f / 3;
            public override void Start()
            {
                base.Start();
                laserInstance = GameObject.Instantiate(Utils.Paths.GameObject.LaserGolem.Load<GameObject>(), base.transform);
                lr = laserInstance.GetComponent<LineRenderer>();
            }
            public override void ReachedTarget() {
                ChangeOrbitTarget(target);
            }

            public override void FixedUpdate()
            {
                base.FixedUpdate();
                if (target && owner && laserInstance) {
                    laserInstance.SetActive(true);
                    lr.SetPosition(0, base.transform.position);
                    lr.SetPosition(1, target.position);
                    lr.startWidth = 1f;
                    lr.endWidth = 1f;
                    stopwatch += Time.fixedDeltaTime;
                    if (stopwatch >= delay) {
                        CharacterBody body = owner.GetComponent<CharacterBody>();
                        stopwatch = 0f;
                        BulletAttack attack = new();
                        attack.aimVector = (target.position - base.transform.position).normalized;
                        attack.damage = body.damage * damageCoeffPerTick;
                        attack.damageColorIndex = DamageColorIndex.Item;
                        attack.owner = owner;
                        attack.weapon = owner;
                        attack.origin = base.transform.position;
                        attack.falloffModel = BulletAttack.FalloffModel.None;
                        attack.damageType = DamageType.SlowOnHit;
                        attack.procCoefficient = 0.5f;

                        attack.Fire();
                    }
                }
                else {
                    if (laserInstance) laserInstance.SetActive(false);
                }
            }
        }

        private void HandleDrone(object sender, EventArgs e) {
            GameObject surv = Survivor.Load<GameObject>();
            GenericSkill[] skills = surv.GetComponents<GenericSkill>();
            GenericSkill match = null;
            string toFind = Misc.Selectables.Prefix + MicroDrones.MiscSelectableNameStatic;
            try {
                match = skills.First(x => x.skillName != null && x.skillName == toFind);
            }
            catch {
                Main.ModLogger.LogWarning("Couldn't find the matching GenericSkill for MicroDrones");
            }

            if (match) {
                Debug.Log("found match");
                SkillFamily family = match.skillFamily;

                if (family.variants == null) {
                    Debug.Log("variants null making new");
                    family.variants = new SkillFamily.Variant[1];
                    Debug.Log(family.variants == null);
                    family.variants[0] = new SkillFamily.Variant {
                        skillDef = SkillDef,
                        viewableNode = new ViewablesCatalog.Node(SkillDef.skillNameToken, false, null)
                    };
                }
                else {
                    Debug.Log("variants real resizing and adding");
                    Array.Resize(ref family.variants, family.variants.Length + 1);
                
                    family.variants[family.variants.Length - 1] = new SkillFamily.Variant {
                        skillDef = SkillDef,
                        viewableNode = new ViewablesCatalog.Node(SkillDef.skillNameToken, false, null)
                    };
                }
            }
        }
    }
}*/