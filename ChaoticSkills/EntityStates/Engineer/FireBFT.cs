using System;
using EntityStates.Merc.Weapon;

namespace ChaoticSkills.EntityStates.Engineer {
    public class FireBFT : BaseState {
        private float damageCoeff = 13567.5f * 0.01f;
        private float procCoefficient = 3f;
        private float shotDelay = 5;
        private LineRenderer lr;
        private GameObject lasr;
        private Ray ray;

        public override void OnEnter()
        {
            base.OnEnter();
            lasr = GameObject.Instantiate(Utils.Paths.GameObject.LaserGolem.Load<GameObject>(), base.FindModelChild("Muzzle"));
            lr = lasr.GetComponent<LineRenderer>();
            lr.startColor = Color.green;
            lr.endColor = Color.green;
            lr.startWidth = 3f;
            lr.endWidth = 3f;
            lr.SetPosition(0, base.FindModelChild("Muzzle").position);
            ray = new Ray {
                origin = base.FindModelChild("Muzzle").position,
                direction = base.FindModelChild("Barrel").forward
            };
        }

        public override void OnExit()
        {
            base.OnExit();
            Destroy(lasr);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.fixedAge <= shotDelay) {
                lr.SetPosition(0, base.FindModelChild("Muzzle").position);
                lr.SetPosition(1, ray.GetPoint(1000));
                lr.startWidth -= 0.5f * Time.fixedDeltaTime;
                lr.endWidth -= 0.5f * Time.fixedDeltaTime;
            }
            else {
                AkSoundEngine.PostEvent(Events.Play_railgunner_R_fire, base.gameObject);
                if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, Mathf.Infinity, LayerIndex.world.mask | LayerIndex.entityPrecise.mask)) {
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
                }

                outer.SetNextStateToMain();
            }
            Vector3 targetRotation = base.GetAimRay().direction;
            targetRotation.x = Mathf.Clamp(targetRotation.x, -70, 70);
            ray.direction = base.FindModelChild("Barrel").forward;
            base.FindModelChild("Barrel").forward = Vector3.Lerp(ray.direction, targetRotation, 40 * Time.fixedDeltaTime);
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}