using System;

namespace ChaoticSkills.EntityStates.Engineer {
    public class Flamethrower : BaseState {
        private GameObject prefab => Utils.Paths.GameObject.DroneFlamethrowerEffect.Load<GameObject>();
        private GameObject instanceOne;
        private GameObject instanceTwo;
        private float damageCoeffPerSecond = 200f;
        private float procCoeff = 1f;
        private float stopwatch = 0f;
        private float ticks = 6;
        private float delay => 1f / ticks;
        private float stopwatchReset = 0f;
        private float delayReset = 2f;
        private float damagePerHit => damageCoeffPerSecond / ticks;

        public override void OnEnter()
        {
            base.OnEnter();
            if (base.isAuthority) {
                damageCoeffPerSecond *= base.attackSpeedStat;
                ResetInstances();
            }   

            AkSoundEngine.PostEvent(Events.Play_mage_R_start, base.gameObject);
        }

        public override void OnExit()
        {
            base.OnExit();
            if (instanceOne) Destroy(instanceOne);
            if (instanceTwo) Destroy(instanceTwo);
            AkSoundEngine.PostEvent(Events.Play_mage_R_end, base.gameObject);
        }

        private void ResetInstances() {
            if (instanceOne) Destroy(instanceOne);
            if (instanceTwo) Destroy(instanceTwo);

            Transform muzzleLeft = base.FindModelChild("MuzzleLeft");
                Transform muzzleRight = base.FindModelChild("MuzzleRight");

                instanceOne = GameObject.Instantiate(prefab, muzzleLeft);
                instanceOne.RemoveComponent<DestroyOnTimer>();
                instanceTwo = GameObject.Instantiate(prefab, muzzleRight);
                instanceTwo.RemoveComponent<DestroyOnTimer>();
                ParticleSystem.MainModule system1 = instanceOne.GetComponent<ScaleParticleSystemDuration>().particleSystems[0].main;
                ParticleSystem.MainModule system2 = instanceTwo.GetComponent<ScaleParticleSystemDuration>().particleSystems[0].main;
                system1.duration = 900f;
                system2.duration = 900f;

                instanceOne.RemoveComponent<ScaleParticleSystemDuration>();
                instanceTwo.RemoveComponent<ScaleParticleSystemDuration>();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.isAuthority) {
                stopwatch += Time.fixedDeltaTime;
                stopwatchReset += Time.fixedDeltaTime;

                if (stopwatchReset >= delayReset) {
                    stopwatchReset = 0f;
                    ResetInstances();
                }
                
                if (stopwatch >= delay) {
                    stopwatch = 0f;

                    if (instanceOne) {
                        FireBullet(instanceOne.transform, "MuzzleLeft");
                        FireBullet(instanceTwo.transform, "MuzzleRight");
                    }
                }

                if (instanceOne) {
                    instanceOne.transform.forward = base.GetAimRay().direction;
                }
                if (instanceTwo) {
                    instanceTwo.transform.forward = base.GetAimRay().direction;
                }

                if (!base.inputBank.skill1.down) {
                    outer.SetNextStateToMain();
                }
            }
        }

        public void FireBullet(Transform transform, string muzzleName) {
            BulletAttack attack = new();
            attack.damage = base.damageStat * damagePerHit;
            attack.procCoefficient = procCoeff;
            attack.maxDistance = 18f;
            attack.damageType = DamageType.IgniteOnHit;
            attack.isCrit = base.RollCrit();
            attack.muzzleName = muzzleName;
            attack.owner = base.gameObject;
            attack.aimVector = base.GetAimRay().direction;
            attack.origin = transform.position;
            attack.stopperMask = LayerIndex.world.mask;
            attack.radius = 1.5f;

            attack.Fire();
        }
    }
}