using System;
using EntityStates.Mage.Weapon;
using RoR2.UI;

namespace ChaoticSkills.EntityStates.Artificer {
    public class ChargeShards : BaseState {
        private float minimumCharge = 0.4f;
        private float baseShardCount = 3f;
        private float baseSpread = 3f;
        private float maxCharge = 1.5f;
        private float duration;
        private CrosshairUtils.OverrideRequest crosshair;
        // private GameObject chargeInstance;
        private uint id;

        public override void OnEnter()
        {
            base.OnEnter();
            duration = maxCharge / base.attackSpeedStat;
            Transform parent = base.FindModelChild("MuzzleBetween") ?? base.characterBody.coreTransform;
            // chargeInstance = GameObject.Instantiate(chargePrefab, parent);
            PlayAnimation("Gesture, Additive", "ChargeNovaBomb", "ChargeNovaBomb.playbackRate", duration);

            crosshair = CrosshairUtils.RequestOverrideForBody(base.characterBody, Utils.Paths.GameObject.MageCrosshair.Load<GameObject>(), CrosshairUtils.OverridePriority.Skill);
            base.StartAimMode(duration + 2);
            id = AkSoundEngine.PostEvent(Events.Play_mage_m2_iceSpear_charge, base.gameObject);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            float charge = Mathf.Clamp01(base.fixedAge / duration);
            base.characterBody.SetSpreadBloom(Util.Remap(charge, 0f, 1f, 0f, 1f));
            if (base.isAuthority && !IsKeyDown() && base.fixedAge >= minimumCharge) {
                ThrowShards state = new(charge);
                outer.SetNextState(state);
            }
            else if (base.isAuthority && base.fixedAge >= duration) {
                ThrowShards state = new(charge);
                outer.SetNextState(state);
            }
        }

        private bool IsKeyDown() {
            if (base.isAuthority && base.inputBank.skill1.down) {
                return true;
            }
            else {
                return false;
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            // Destroy(chargeInstance);
            AkSoundEngine.StopPlayingID(id);
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }

    public class ThrowShards : BaseState {
        private float charge;
        private int maxShards = 1;
        private int minShards = 1;
        private int totalShards = 1;
        private float damageCoeff = 2f;
        private float minSpread = 0f;
        private float maxSpread = 2f;
        private GameObject prefab => Content.Artificer.Shards.prefab;
        public ThrowShards(float _charge) {
            charge = _charge;
            damageCoeff = Mathf.CeilToInt(Util.Remap(charge, 0, 1, 1f, 11f));
        }

        public override void OnEnter()
        {
            base.OnEnter();
            PlayAnimation("Gesture, Additive", "FireNovaBomb", "FireNovaBomb.playbackRate", 1f);
            int spread = -(totalShards / 2);
            for (int i = 0; i < totalShards; i++) {
                FireProjectileInfo info = new();
                info.projectilePrefab = prefab;
                info.position = base.characterBody.corePosition;
                info.rotation = Util.QuaternionSafeLookRotation(base.GetAimRay().direction);
                info.damage = (base.damageStat * damageCoeff);
                info.owner = base.gameObject;
                info.crit = base.RollCrit();
                info.speedOverride = 200f;
                info.useSpeedOverride = true;

                spread++;

                if (base.isAuthority) {
                    ProjectileManager.instance.FireProjectile(info);
                }
            }
            AkSoundEngine.PostEvent(Events.Play_moonBrother_orb_slam_impact, base.gameObject);
            outer.SetNextStateToMain();
        }
    }
}