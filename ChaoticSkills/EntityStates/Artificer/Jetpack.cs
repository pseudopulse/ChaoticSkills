using System;
using RoR2.UI;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace ChaoticSkills.EntityStates.Artificer {
    public class Jetpack : BaseState {
        public float minFuel = 1f;
        public float maxFuel = 100f;
        public float fuel = 100f;
        public float fuelPerSecond = 50f;
        private Transform jetpackEffect;
        private GameObject hud;
        private float rechargeStopwatch = 0f;
        private float rechargeDelay = 1f;
        private float jumpTimerStopwatch = 0f;
        private float jumpTimerDelay = 0.2f;
        private bool isOnCooldown = false;

        public override void OnEnter()
        {
            base.OnEnter();
            jetpackEffect = base.FindModelChild("JetOn");
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.isAuthority && !hud) {
                hud = GameObject.Instantiate(Content.Artificer.Jetpack.jetpackUIPrefab);
                FuelBar bar = hud.transform.Find("Bar").gameObject.AddComponent<FuelBar>();
                bar.jet = this;
            }

            if (inputBank.jump.down) {
                jumpTimerStopwatch += Time.fixedDeltaTime;
            }
            else if (!isGrounded) {
                jumpTimerStopwatch = jumpTimerDelay * 2;
            }
            else {
                jumpTimerStopwatch = 0f;
            }

            if (base.isAuthority && inputBank.jump.down && fuel > minFuel && jumpTimerStopwatch >= jumpTimerDelay && !isOnCooldown) {
                float yTarget = base.characterMotor.velocity.y + 7.5f;
                if (yTarget < 0) {
                    yTarget = 0 + 7.5f;
                }
                float y = base.characterMotor.velocity.y;
                y = Mathf.Lerp(y, yTarget, 5 * Time.fixedDeltaTime);
                base.characterMotor.velocity = new Vector3(base.characterMotor.velocity.x, y, base.characterMotor.velocity.z);
                fuel -= fuelPerSecond * Time.fixedDeltaTime;
                if (jetpackEffect) {
                    jetpackEffect.gameObject.SetActive(true);
                }
            }
            else {
                if (jetpackEffect) {
                    jetpackEffect.gameObject.SetActive(false);
                }
                fuel += fuelPerSecond * Time.fixedDeltaTime;
            }

            if (fuel < 2) {
                isOnCooldown = true;
            }
            
            if (isOnCooldown) {
                rechargeStopwatch += Time.fixedDeltaTime;

                if (rechargeStopwatch >= rechargeDelay) {
                    rechargeStopwatch = 0f;
                    isOnCooldown = false;
                }
            }

            fuel = Mathf.Clamp(fuel, minFuel, maxFuel);
        }

        public override void OnExit()
        {
            base.OnExit();
            Destroy(hud);
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Death;
        }

        private class FuelBar : MonoBehaviour {
            private RectTransform fillRectTransform;
            private UnityEngine.UI.Image image;
            public Jetpack jet;
            public void Start() {
                fillRectTransform = gameObject.GetComponentInChildren<RectTransform>();
            }

            public void FixedUpdate() {
                if (fillRectTransform && jet != null) {
                    fillRectTransform.localScale = new(1 * (jet.fuel / jet.maxFuel), fillRectTransform.localScale.y, fillRectTransform.localScale.z);
                }
            }
        }
    }
}