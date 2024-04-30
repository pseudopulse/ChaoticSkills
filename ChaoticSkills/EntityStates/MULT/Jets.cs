using System;
using EntityStates;

namespace ChaoticSkills.EntityStates.MULT {
    public class Jets : BaseSkillState {
        public GenericSkill slot;
        public float flightDuration = 3f;
        public GameObject effectInstance;
        public GameObject prefab => Utils.Paths.GameObject.DroneFlamethrowerEffect.Load<GameObject>();
        public float upForce = 7.5f;
        public float interval;
        public override void OnEnter()
        {
            base.OnEnter();
            slot = characterBody.skillLocator.utility;

            effectInstance = GameObject.Instantiate(prefab);
            effectInstance.SetActive(false);
            effectInstance.transform.localScale *= 0.2f;

            interval = flightDuration / slot.maxStock;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (!effectInstance) {
                SpawnJets();
            }

            if (slot.stock <= 0) {
                outer.SetNextStateToMain();
            }

            if (inputBank.jump.down && !base.characterMotor.isGrounded) {
                // AkSoundEngine.PostEvent(Events.Play_lemurian_fireball_shoot, base.gameObject); / too loud, idk how to scale volume
                float yTarget = base.characterMotor.velocity.y + 7.5f;
                if (yTarget < 0) {
                    yTarget = 0 + 7.5f;
                }
                float y = base.characterMotor.velocity.y;
                y = Mathf.Lerp(y, yTarget, 5 * Time.fixedDeltaTime);
                base.characterMotor.velocity = new Vector3(base.characterMotor.velocity.x, y, base.characterMotor.velocity.z);

                if (base.fixedAge >= interval) {
                    slot.DeductStock(1);
                    slot.rechargeStopwatch = 0f;
                    base.fixedAge = 0f;
                }

                effectInstance.SetActive(true);
            }
            else {
                effectInstance.SetActive(false);
            }

            effectInstance.transform.forward = Vector3.down;
            effectInstance.transform.position = characterBody.corePosition;
        }

        public void SpawnJets() {
            effectInstance = GameObject.Instantiate(prefab);
            effectInstance.SetActive(false);
            effectInstance.transform.localScale *= 0.2f;
        }

        public override void OnExit()
        {
            base.OnExit();
            EffectManager.SpawnEffect(Utils.Paths.GameObject.ExplosionSolarFlare.Load<GameObject>(), new EffectData {
                origin = characterBody.corePosition,
                scale = 1f
            }, true);

            Destroy(effectInstance);
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}