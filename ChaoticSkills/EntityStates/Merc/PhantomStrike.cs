using System.Diagnostics;
using System.Reflection;
using System;

namespace ChaoticSkills.EntityStates.Merc {
    public class PhantomStrike : BaseState {
        public static GameObject GhostMercPrefab => Content.Merc.Strike.GhostMercPrefab;
        private GameObject GhostMerc1Instance;
        private GameObject GhostMerc2Instance;
        private GameObject GhostMerc3Instance;
        private List<GameObject> ghostMercs = new();
        private float delay = 0.3f;
        private Vector3 gm1Target;
        private Vector3 gm2Target;
        private Vector3 gm3Target;
        private bool hasSlashed = false;
        private GameObject swingEffectInstance;

        public override void OnEnter()
        {
            base.OnEnter();

            GhostMerc1Instance = SpawnGhostMerc();
            GhostMerc2Instance = SpawnGhostMerc();
            GhostMerc3Instance = SpawnGhostMerc();

            gm1Target = GetTarget(0);
            gm2Target = GetTarget(1);
            gm3Target = GetTarget(2);

            Vector3 GetTarget(int i) {
                float bonusYaw = (float)Mathf.FloorToInt(i - ((3 - 1) / 2f) / (float)(3 - 1)) * 20f;
                Vector3 forward = Util.ApplySpread(inputBank.aimDirection, 0f, 0f, 1f, 1f, bonusYaw);
                return forward;
            }

            PlayCrossfade("Gesture, Additive", "GroundLight1", "GroundLight.playbackRate", 1f, 0.05f);
            PlayCrossfade("Gesture, Override", "GroundLight1", "GroundLight.playbackRate", 1f, 0.05f);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.fixedAge >= delay) {
                
                if (hasSlashed == false) {
                    GhostMercSlash();
                    hasSlashed = true;

                    /*swingEffectInstance = GameObject.Instantiate(Utils.Paths.GameObject.MercSwordSlash.Load<GameObject>(), base.transform);
                    ScaleParticleSystemDuration component = swingEffectInstance.GetComponent<ScaleParticleSystemDuration>();
                    if (component)
                    {
                        component.newDuration = component.initialDuration;
                    }*/
                }

                if (base.fixedAge >= delay + 1f) {
                    outer.SetNextStateToMain();
                }
            }

            if (base.inputBank.skill1.down || base.fixedAge < 0.3f) {
                UpdateGhostMercPositions();
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            Destroy(GhostMerc1Instance);
            Destroy(GhostMerc2Instance);
            Destroy(GhostMerc3Instance);
            // GameObject.Destroy(swingEffectInstance);
        }

        public void UpdateGhostMercPositions() {
            UpdateGhostMercPosition(GhostMerc1Instance, GhostMerc1Instance.transform.position + (gm1Target * (2f)), -base.transform.right);
            UpdateGhostMercPosition(GhostMerc2Instance, GhostMerc2Instance.transform.position + (gm2Target * (2f)), base.transform.right);
            UpdateGhostMercPosition(GhostMerc3Instance, GhostMerc3Instance.transform.position + (gm3Target * (2f)), base.transform.forward);
            // Main.ModLogger.LogError("Updated GhostMerc positions");
        }

        public void UpdateGhostMercPosition(GameObject merc, Vector3 targetPosition, Vector3 targetRotation) {
            merc.GetComponent<CharacterDirection>().forward = targetRotation;
            merc.transform.forward = targetRotation;
            Vector3 currentPos = merc.transform.position;
            merc.GetComponent<Rigidbody>().position = Vector3.Lerp(currentPos, targetPosition, 10f * Time.fixedDeltaTime);
        }

        public void GhostMercSlash() {
            foreach (GameObject merc in ghostMercs) {
                EntityStateMachine esm = EntityStateMachine.FindByCustomName(merc, "Weapon");
                esm.SetNextState(new GroundLight2(characterBody));
            }
        }

        public GameObject SpawnGhostMerc() {
            GameObject ghostMerc = GameObject.Instantiate(GhostMercPrefab, base.transform.position, Quaternion.identity);
            ghostMerc.GetComponent<CharacterBody>().AddBuff(RoR2Content.Buffs.Intangible);
            ghostMerc.GetComponent<TeamComponent>()._teamIndex = TeamIndex.Player;
            ghostMercs.Add(ghostMerc);
            NetworkServer.Spawn(ghostMerc);
            return ghostMerc;
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Death;
        }
    }

    public class GroundLight2 : BasicMeleeAttack
    {
        public CharacterBody owner;
        private string animationStateName;
        public override void OnEnter()
        {
            baseDuration = 0.6f;
            damageCoefficient = 2f;
            hitBoxGroupName = "Sword";
            hitEffectPrefab = Utils.Paths.GameObject.OmniImpactVFXSlashMerc.Load<GameObject>();
            procCoefficient = 1f;
            forceVector = Vector3.zero;
            pushAwayForce = 600;
            swingEffectPrefab = Utils.Paths.GameObject.MercSwordSlash.Load<GameObject>();
            mecanimHitboxActiveParameter = "Sword.active";

            base.OnEnter();
            base.characterDirection.forward = GetAimRay().direction;
            damageStat = owner.damage;
        }

        public GroundLight2(CharacterBody b) {
            owner = b;
        }

        public override void OnExit()
        {
            base.OnExit();
        }
        public override void PlayAnimation()
        {
            animationStateName = "";
            animationStateName = "GroundLight1";
            bool @bool = animator.GetBool("isMoving");
            bool bool2 = animator.GetBool("isGrounded");
            if (!@bool && bool2)
            {
                PlayCrossfade("FullBody, Override", animationStateName, "GroundLight.playbackRate", duration, 0.05f);
            }
            else
            {
                PlayCrossfade("Gesture, Additive", animationStateName, "GroundLight.playbackRate", duration, 0.05f);
                PlayCrossfade("Gesture, Override", animationStateName, "GroundLight.playbackRate", duration, 0.05f);
            }
        }

        public override void BeginMeleeAttackEffect()
        {
            swingEffectMuzzleString = animationStateName;
            base.BeginMeleeAttackEffect();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}