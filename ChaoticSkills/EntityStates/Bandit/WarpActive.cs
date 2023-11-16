using System;

namespace ChaoticSkills.EntityStates.Bandit {
    public class WarpActive : BaseState {
        public override void OnEnter()
        {
            base.OnEnter();
            AkSoundEngine.PostEvent(Events.Play_bandit2_shift_enter, base.gameObject);
            EffectManager.SpawnEffect(Utils.Paths.GameObject.Bandit2SmokeBomb.Load<GameObject>(), new EffectData {
                origin = base.transform.position
            }, true);
            base.characterBody.AddBuff(Content.Bandit.Warp.cloakBuff);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (!characterBody.HasBuff(Content.Bandit.Warp.cloakBuff)) {
                outer.SetNextStateToMain();
            }
        }
    }
}