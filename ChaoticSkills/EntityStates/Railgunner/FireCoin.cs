using System;
using ESR = EntityStates.Railgunner;

namespace ChaoticSkills.EntityStates.Railgunner {
    public class FireCoin : BaseSkillState {
        public override void OnEnter()
        {
            base.OnEnter();

            FireProjectileInfo info = new();
            info.projectilePrefab = Content.Railgunner.Coin.CoinPrefab;
            info.owner = base.gameObject;
            info.position = base.transform.position;
            info.rotation = Util.QuaternionSafeLookRotation(Util.ApplySpread(base.inputBank.aimDirection, 1f, 1f, 0f, 0f, 0f, 3f));

            ProjectileManager.instance.FireProjectile(info);

            AkSoundEngine.PostEvent(Events.Play_wCoins, base.gameObject);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            
            if (base.fixedAge >= 0.2f) {
                outer.SetNextStateToMain();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}