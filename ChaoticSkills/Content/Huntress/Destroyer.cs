using System;

namespace ChaoticSkills.Content.Huntress {
    public class Destroyer : SkillBase<Destroyer> {
        public override SerializableEntityStateType ActivationState => ContentAddition.AddEntityState<EntityStates.Huntress.Destroyer>(out bool _);
        public override float Cooldown => 14f;
        public override bool DelayCooldown => true;
        public override string Description => "<style=cDeath>Overdriven</style>. Charge up a piercing blade, dealing <style=cIsDamage>900%-3900%</style>. Upon fully charging, release a <style=cIsDamage>stunning</style> shockwave.";
        public override bool Agile => false;
        public override bool IsCombat => true;
        public override string LangToken => "Destroyer";
        public override int StockToConsume => 1;
        public override int MaxStock => 1;
        public override bool SprintCancelable => false;
        public override UnlockableDef Unlock => null;
        public override string Machine => "Weapon";
        public override Sprite SkillIcon => Main.Assets.LoadAsset<Sprite>("Assets/Icons/Huntress/Destroyer.png");
        public override SkillSlot Slot => SkillSlot.Special;
        public override string Survivor => Utils.Paths.GameObject.HuntressBody;
        public override string Name => "Destroyer";
        public override List<string> Keywords => new() { "KEYWORD_OVERDRIVE" };
        public static GameObject DestroyerProjectile;

        public override void PostCreation()
        {
            DestroyerProjectile = Main.Assets.LoadAsset<GameObject>("Assets/Prefabs/Huntress/Destroyer/Destroyer.prefab");
            DestroyerProjectile.AddComponent<DestroyerCharge>();

            PrefabAPI.RegisterNetworkPrefab(DestroyerProjectile);
            ContentAddition.AddProjectile(DestroyerProjectile);
        }

        public class DestroyerCharge : MonoBehaviour {
            public ProjectileController controller;
            public GameObject owner;
            public ProjectileSimple simple;
            public EntityStateMachine machine;
            public EntityStates.Huntress.Destroyer state;
            public ProjectileDamage damage;
            public bool hasBeenInState = false;
            
            public void Start() {
                controller = GetComponent<ProjectileController>();
                if (controller.owner) {
                    owner = controller.owner;
                }
                simple = GetComponent<ProjectileSimple>();
                damage = GetComponent<ProjectileDamage>();
            }

            public void FixedUpdate() {
                if (!owner && controller) {
                    if (controller.owner) {
                        owner = controller.owner;
                    }
                }

                if (!machine && owner) {
                    machine = owner.GetComponents<EntityStateMachine>().FirstOrDefault(x => x.customName == "Weapon");
                }

                if (state == null && machine) {
                    if ((state = machine.state as EntityStates.Huntress.Destroyer) != null) {
                        state = machine.state as EntityStates.Huntress.Destroyer;
                        hasBeenInState = true;
                    }
                }

                if (owner) {
                    base.transform.position = owner.GetComponent<InputBankTest>().GetAimRay().GetPoint(1.5f) - new Vector3(0, 1f, 0f);
                    base.transform.forward = owner.GetComponent<InputBankTest>().GetAimRay().direction;
                }

                if (state != null) {
                    damage.damage = state.targetDamage;
                    base.transform.localScale = new Vector3(1f, 1f, 1f) * state.targetScale;
                }

                if (state != null && state.ended) {
                    simple.enabled = true;
                    base.gameObject.RemoveComponent<DestroyerCharge>();
                }
            }
        }
    }
}