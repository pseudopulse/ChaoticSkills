/*using System;
using RoR2.Orbs;

namespace ChaoticSkills.Content.Captain.Drones {
    public class SupportDrone : SkillBase<SupportDrone> {
        public override SerializableEntityStateType ActivationState => new SerializableEntityStateType(typeof(Idle));
        public override float Cooldown => 0f;
        public override bool DelayCooldown => false;
        public override string Description => "The <style=cIsUtility>micro-drone</style> will <style=cIsDamage>fly to an ally</style>, healing them for <style=cIsDamage>35%</style> of their maximum health before going on cooldown.";
        public override bool Agile => true;
        public override bool AgileAddKeyword => false;
        public override bool IsCombat => true;
        public override string LangToken => "SupportDrone";
        public override int StockToConsume => 0;
        public override int MaxStock => 0;
        public override UnlockableDef Unlock => null;
        public override string Machine => "Weapon";
        public override Sprite SkillIcon => null;
        public override SkillSlot Slot => SkillSlot.Secondary;
        public override string Survivor => Utils.Paths.GameObject.CaptainBody;
        public override bool MustKeyPress => true;
        public override bool AutoApply => false;
        public override string Name => "Support Drone";
        public static GameObject SupportDronePrefab;
        private static Material droneMat;
        public override void PostCreation()
        {
            base.PostCreation();
            // actual drone
            SupportDronePrefab = PrefabAPI.InstantiateClone(Utils.Paths.GameObject.PickupCaptainDefenseMatrix.Load<GameObject>(), "SupportDrone");
            SupportDronePrefab.AddComponent<SupportDroneController>();
            SupportDronePrefab.transform.localPosition = Vector3.zero;
            SupportDronePrefab.transform.localScale *= 0.7f;
            MeshRenderer renderer = SupportDronePrefab.GetComponentInChildren<MeshRenderer>();
            Material mat = GameObject.Instantiate(Utils.Paths.Material.matCaptainRobotBits.Load<Material>());
            mat.color = Color.green;
            GameObject.DontDestroyOnLoad(mat);
            renderer.material = mat;
            // handling the selectable matching
            if (MicroDrones.Instance != null) {
                HandleDrone(null, null);
            } else {
                MicroDrones.PostCreationEvent += HandleDrone;
            }
        }

        public class SupportDroneController : CaptainDrone {
            public override void Start()
            {
                base.Start();
                
            }
            public override void ReachedTarget() {
                CharacterBody cb = target.GetComponent<CharacterBody>();
                if (cb) {
                    HealOrb orb = new();
                    orb.healValue = cb.healthComponent.fullCombinedHealth * 0.35f;
                    orb.origin = base.transform.position;
                    orb.target = cb.mainHurtBox;
                    OrbManager.instance.AddOrb(orb);
                }
                chasing = false;
                owner.GetComponent<SkillLocator>().secondary.DeductStock(1);
                Recall();
            }

            public override void FixedUpdate()
            {
                base.FixedUpdate();
                
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