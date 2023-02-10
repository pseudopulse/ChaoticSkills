using System;

namespace ChaoticSkills.Content.Captain {
    public class OffensiveMicrobots : SkillBase<OffensiveMicrobots> {
        public override SerializableEntityStateType ActivationState => new SerializableEntityStateType(typeof(Idle));
        public override float Cooldown => 0f;
        public override bool DelayCooldown => false;
        public override string Description => "Gain <style=cIsUtility>combatant microbots</style> that fire bursts of slowing lasers for <style=cIsDamage>360% damage</style> alongside you.";
        public override bool Agile => true;
        public override bool SprintCancelable => true;
        public override bool AgileAddKeyword => false;
        public override bool IsCombat => true;
        public override string LangToken => "OffensiveMicrobots";
        public override int StockToConsume => 1;
        public override int MaxStock => 1;
        public override UnlockableDef Unlock => null;
        public override bool AutoApply => true;
        public override string Machine => "Weapon";
        public override bool Passive => true;
        public override Sprite SkillIcon => Main.Assets.LoadAsset<Sprite>("Assets/Icons/Captain/OffensiveMicrobots.png");
        public override SkillSlot Slot => SkillSlot.None;
        public override string Survivor => Utils.Paths.GameObject.CaptainBody;
        public override string Name => "Offensive Microbots";
        public static GameObject MicrobotPrefab;
        public override void PostCreation()
        {

            On.RoR2.CaptainDefenseMatrixController.TryGrantItem += (orig, self) => {
                bool hasPassive = false;
                if (self.characterBody) {
                    foreach (GenericSkill skill in self.characterBody.GetComponents<GenericSkill>()) {
                        if (skill.skillDef == SkillDef) {
                            hasPassive = true;
                        }
                    }
                }
                if (!hasPassive) {
                    orig(self);
                }
                else {
                    
                }
            };

            On.RoR2.CharacterBody.Start += (orig, self) => {
                orig(self);
                bool hasPassive = false;
                if (self) {
                    foreach (GenericSkill skill in self.GetComponents<GenericSkill>()) {
                        if (skill.skillDef == SkillDef) {
                            hasPassive = true;
                        }
                    }
                }
                if (!hasPassive) {
                
                }
                else {
                    if (!self.GetComponent<OffensiveMatrixController>()) {
                        self.gameObject.AddComponent<OffensiveMatrixController>();
                    }
                }
            };

            On.RoR2.CaptainDefenseMatrixController.OnServerMasterSummonGlobal += (orig, self, report) => {
                bool hasPassive = false;
                if (self.characterBody) {
                    foreach (GenericSkill skill in self.characterBody.GetComponents<GenericSkill>()) {
                        if (skill.skillDef == SkillDef) {
                            hasPassive = true;
                        }
                    }
                }
                if (!hasPassive) {
                    orig(self, report);
                }
            };

            MicrobotPrefab = Utils.Paths.GameObject.PickupCaptainDefenseMatrix.Load<GameObject>().InstantiateClone("OffensiveMicrobot", false);
        }

        enum AttackType {
            Fire,
            FirePassive
        }

        public class OffensiveMatrixController : MonoBehaviour {
            private CharacterBody self => GetComponent<CharacterBody>();
            private InputBankTest input => GetComponent<InputBankTest>();
            private List<Microbot> microbots;
            private float speed = 9f;
            private float initialTime = Run.instance.GetRunStopwatch();
            private Vector3[] planes = { Vector3.up, Vector3.forward, Vector3.down };
            private float stopwatch = 0f;
            struct Microbot {
                public GameObject microbot;
                public float mainSpeed;
                public Vector3 initialRadial;
                public Vector3 plane;
                public float distance;
                public float offset;
                public float targetSpeed;
                public float targetOffset;
                public float targetDistance;
                public AttackType type;
            }
            private void Start() {
                if (true) { // useless check but im too lazy to un-indent this
                    microbots = new();

                    for (int i = 0; i < 3; i++) {
                        Microbot bot = new() {
                            mainSpeed = (360/speed) * UnityEngine.Random.Range(0.8f, 1.2f),
                            microbot = GameObject.Instantiate(MicrobotPrefab),
                            distance = UnityEngine.Random.Range(0.9f, 2f),
                            offset = UnityEngine.Random.Range(-0.5f, 1f),
                            plane = Vector3.up,
                        };
                        bot.microbot.transform.localScale *= 0.3f;
                        bot.microbot.RemoveComponent<ModelPanelParameters>();
                        switch (i) {
                            case 0:
                                bot.type = AttackType.Fire;
                                break;
                            case 1:
                                bot.type = AttackType.FirePassive;
                                break;
                            case 2:
                                bot.type = AttackType.Fire;
                                break;
                        }
                        bot.initialRadial = Quaternion.AngleAxis(UnityEngine.Random.Range(0, 360), bot.plane) * self.transform.forward;
                        microbots.Add(bot);
                    }

                    self.onSkillActivatedAuthority += Fire;
                }
            }

            private void FixedUpdate() {
                if (true) { // ditto
                    stopwatch += Time.fixedDeltaTime;
                    if (stopwatch >= 1.3f) {
                        stopwatch = 0f;
                        FirePassive();
                    }
                    for (int i = 0; i < 3; i++) {
                        Microbot bot = microbots[i];
                        float angle = (Run.instance.GetRunStopwatch() - initialTime) * bot.mainSpeed;
                        Vector3 pos = self.corePosition + new Vector3(0, bot.offset, 0) + Quaternion.AngleAxis(angle, bot.plane) * bot.initialRadial * bot.distance;
                        bot.microbot.transform.position = Vector3.Lerp(bot.microbot.transform.position, pos, 15 * Time.fixedDeltaTime);
                        bot.microbot.transform.Rotate(new(45 * Time.fixedDeltaTime, 46 * Time.fixedDeltaTime, 46 * Time.fixedDeltaTime));

                        bot.targetDistance = Mathf.Clamp(bot.distance + (Time.fixedDeltaTime * UnityEngine.Random.Range(-5f, 5f)), 0.3f, 4f);
                        bot.targetOffset = Mathf.Clamp(bot.offset + (Time.fixedDeltaTime * UnityEngine.Random.Range(-5f, 5f)), -0.5f, 3f);
                        bot.targetSpeed = Mathf.Clamp(bot.mainSpeed + (Time.fixedDeltaTime * UnityEngine.Random.Range(-5f, 5f)), 9f, 24f);

                        bot.mainSpeed = Mathf.Lerp(bot.mainSpeed, bot.targetSpeed, 20 * Time.fixedDeltaTime);
                        bot.distance = Mathf.Lerp(bot.distance, bot.targetDistance, 20 * Time.fixedDeltaTime);
                        bot.offset = Mathf.Lerp(bot.offset, bot.targetOffset, 20 * Time.fixedDeltaTime);
                    }
                }
            }

            public void Fire(GenericSkill skill) {
                foreach (Microbot bot in microbots.Where(x => x.type == AttackType.Fire)) {
                    BulletAttack attack = new();
                    attack.origin = bot.microbot.transform.position;
                    attack.owner = self.gameObject;
                    attack.weapon = bot.microbot;
                    attack.aimVector = input.GetAimRay().direction;
                    attack.damage = self.damage * 3.6f;
                    attack.isCrit = false;
                    attack.minSpread = 0f;
                    attack.maxSpread = 1.5f;
                    attack.procCoefficient = 0.8f;
                    attack.damageType = DamageType.SlowOnHit;
                    attack.damageColorIndex = DamageColorIndex.Item;
                    attack.tracerEffectPrefab = Utils.Paths.GameObject.TracerCaptainDefenseMatrix.Load<GameObject>();
                    attack.filterCallback = delegate (BulletAttack attack, ref BulletAttack.BulletHit hit) {
                        if (hit.hitHurtBox && hit.hitHurtBox.teamIndex != TeamIndex.Player) {
                            return true;
                        }
                        else {
                            return false;
                        }
                    };
                    if (Physics.Raycast(bot.microbot.transform.position, input.GetAimRay().direction, out RaycastHit hit, Mathf.Infinity, LayerIndex.world.mask)) {
                        BlastAttack battack = new();
                        battack.damageType = DamageType.Silent;
                        battack.baseDamage = 0;
                        battack.radius = 4.5f;
                        battack.position = hit.point;
                        battack.crit = false;
                        battack.baseForce = 3000;
                        battack.procCoefficient = 0f;
                        battack.teamIndex = TeamIndex.Monster;

                        if (self.hasAuthority) {
                            battack.Fire();
                        }
                        EffectManager.SpawnEffect(Utils.Paths.GameObject.ExplosionGolem.Load<GameObject>(), new EffectData {
                            origin = hit.point,
                            scale = 3
                        }, true);
                    }
 
                    if (self.hasAuthority) {
                        attack.Fire();
                    }
                }
                AkSoundEngine.PostEvent(Events.Play_captain_drone_zap, self.gameObject);
            }

            private void FirePassive() {
                foreach (Microbot bot in microbots.Where(x => x.type == AttackType.FirePassive)) {
                    List<Collider> colliders = Physics.OverlapSphere(bot.microbot.transform.position, 25, LayerIndex.entityPrecise.mask, QueryTriggerInteraction.Ignore).ToList().Where(
                        x => x.GetComponent<HurtBox>() && x.GetComponent<HurtBox>().teamIndex != TeamIndex.Player
                    ).ToList();
                    if (colliders.Count >= 1) {
                        Collider col = colliders.First();
                        if (col.GetComponent<HurtBox>()) {
                            HurtBox box = col.GetComponent<HurtBox>();
                            Vector3 aimDirection = (box.transform.position - bot.microbot.transform.position).normalized;

                            BulletAttack attack = new();
                            attack.origin = bot.microbot.transform.position;
                            attack.owner = self.gameObject;
                            attack.weapon = bot.microbot;
                            attack.aimVector = aimDirection;
                            attack.damage = self.damage * 2f;
                            attack.isCrit = false;
                            attack.minSpread = 0f;
                            attack.maxSpread = 1.5f;
                            attack.procCoefficient = 0.8f;
                            attack.damageType = DamageType.SlowOnHit;
                            attack.damageColorIndex = DamageColorIndex.Item;
                            attack.tracerEffectPrefab = Utils.Paths.GameObject.TracerCaptainDefenseMatrix.Load<GameObject>();
                            attack.filterCallback = delegate (BulletAttack attack, ref BulletAttack.BulletHit hit) {
                                if (hit.hitHurtBox && hit.hitHurtBox.teamIndex != TeamIndex.Player) {
                                    return true;
                                }
                                else {
                                    return false;
                                }
                            };
                            if (Physics.Raycast(bot.microbot.transform.position, aimDirection, out RaycastHit hit, Mathf.Infinity, LayerIndex.world.mask)) {
                                BlastAttack battack = new();
                                battack.damageType = DamageType.Silent;
                                battack.baseDamage = 0;
                                battack.radius = 4.5f;
                                battack.position = hit.point;
                                battack.crit = false;
                                battack.baseForce = 3000;
                                battack.procCoefficient = 0f;
                                battack.teamIndex = TeamIndex.Monster;

                                if (self.hasAuthority) {
                                    battack.Fire();
                                }
                                EffectManager.SpawnEffect(Utils.Paths.GameObject.ExplosionGolem.Load<GameObject>(), new EffectData {
                                    origin = hit.point,
                                    scale = 3
                                }, true);
                            }
        
                            if (self.hasAuthority) {
                                attack.Fire();
                            }
                        }
                    }
                }
            }

            private void OnDestroy() {
                if (true) {
                    foreach (Microbot bot in microbots) {
                        GameObject.DestroyImmediate(bot.microbot);
                    }

                    microbots.RemoveAll(x => x.microbot == null);
                    self.onSkillActivatedServer -= Fire;
                }
            }
        }
    }
}