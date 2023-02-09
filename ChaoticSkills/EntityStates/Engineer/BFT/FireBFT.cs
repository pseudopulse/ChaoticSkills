using System;
using EntityStates.Merc.Weapon;

namespace ChaoticSkills.EntityStates.Engineer {
    public class FireBFT : BaseState {
        private float damageCoeff = Content.Engineer.SBFT.DamageCoeff;
        private float procCoefficient = 3f;
        private float shotDelay = 4;
        private Ray ray;
        private GameObject instance;

        public override void OnEnter()
        {
            base.OnEnter();
            instance = GameObject.Instantiate(Main.Assets.LoadAsset<GameObject>("Assets/Prefabs/Engineer/BFT/BFTModel/railgun.prefab"), FindModelChild("VFXPoint"));
            GetModelAnimator().enabled = false;
        }

        public override void OnExit()
        {
            base.OnExit();
            Destroy(instance);
            GetModelAnimator().enabled = true;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.fixedAge <= shotDelay) {
                Transform rotateobj = base.FindModelChild("RotateObject");
                if (base.characterBody.master && base.characterBody.master.GetComponent<BaseAI>()) {
                    BaseAI ai = base.characterBody.master.GetComponent<BaseAI>();
                    if (ai.currentEnemy.gameObject && ai.currentEnemy.GetBullseyePosition(out Vector3 pos)) {
                        rotateobj.position = pos;
                    }
                    else {
                        rotateobj.position = base.GetAimRay().GetPoint(100);
                    }
                }
                else {
                    rotateobj.position = base.GetAimRay().GetPoint(100);
                }
            }
            else {
                AkSoundEngine.PostEvent(Events.Play_railgunner_R_fire, base.gameObject);
                if (Physics.Raycast(base.GetAimRay().origin, base.GetAimRay().direction, out RaycastHit hit, Mathf.Infinity, LayerIndex.world.mask | LayerIndex.entityPrecise.mask)) {
                    BlastAttack battack = new();
                    battack.baseDamage = base.damageStat * damageCoeff;
                    battack.radius = 10f;
                    battack.position = hit.point;
                    battack.crit = base.RollCrit();
                    battack.baseForce = 3000;
                    battack.procCoefficient = procCoefficient;
                    battack.teamIndex = base.GetTeam();
                    battack.falloffModel = BlastAttack.FalloffModel.None;
                    battack.attacker = base.gameObject;

                    battack.Fire();
                    EffectManager.SpawnEffect(Utils.Paths.GameObject.BeamSphereExplosion.Load<GameObject>(), new EffectData {
                        origin = hit.point,
                        scale = 3
                    }, true);

                    GetModelAnimator().enabled = true;

                    base.PlayAnimation("Base Layer", "Armature_Firing");
                }

                outer.SetNextStateToMain();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}