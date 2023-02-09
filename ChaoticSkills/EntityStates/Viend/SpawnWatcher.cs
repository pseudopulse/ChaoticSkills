/*using System;

namespace ChaoticSkills.EntityStates.Viend {
    public class SpawnWatcher : BaseState {
        private float duration = 2f;

        public override void OnEnter()
        {
            base.OnEnter();
            duration = duration / base.attackSpeedStat;
            PlayAnimation("LeftArm, Override", "ChargeCrushCorruption", "CrushCorruption.playbackRate", duration);
            AkSoundEngine.PostEvent(Events.Play_voidman_R_activate, base.gameObject);
            EffectManager.SimpleMuzzleFlash(Utils.Paths.GameObject.VoidSurvivorChargeCrushCorruption.Load<GameObject>(), base.gameObject, "MuzzleHandBeam", false);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.fixedAge >= duration) {
                EffectManager.SpawnEffect(Utils.Paths.GameObject.ExplodeOnDeathVoidExplosionEffect.Load<GameObject>(), new EffectData {
                    origin = base.transform.position + new Vector3(0, 3, 0),
                    scale = 5f,
                }, false);
                GameObject watcher = GameObject.Instantiate(Content.Viend.Watcher.WatcherPrefab, base.transform.position + new Vector3(0, 3, 0), Quaternion.identity);
                watcher.GetComponent<Content.Viend.Watcher.WatcherBehavior>().SetOwner(base.characterBody, base.GetComponent<VoidSurvivorController>());
                outer.SetNextStateToMain();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }

    public class SpawnWatcherVoid : BaseState {
        private float duration = 2f;

        public override void OnEnter()
        {
            base.OnEnter();
            duration = duration / base.attackSpeedStat;
            PlayAnimation("LeftArm, Override", "ChargeCrushCorruption", "CrushCorruption.playbackRate", duration);
            AkSoundEngine.PostEvent(Events.Play_voidman_R_activate, base.gameObject);
            EffectManager.SimpleMuzzleFlash(Utils.Paths.GameObject.VoidSurvivorChargeCrushCorruption.Load<GameObject>(), base.gameObject, "MuzzleHandBeam", false);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.fixedAge >= duration) {
                EffectManager.SpawnEffect(Utils.Paths.GameObject.ExplodeOnDeathVoidExplosionEffect.Load<GameObject>(), new EffectData {
                    origin = base.transform.position + new Vector3(0, 3, 0),
                    scale = 5f,
                }, false);
                GameObject watcher = GameObject.Instantiate(Content.Viend.WatcherVoid.WatcherVoidPrefab, base.transform.position + new Vector3(0, 3, 0), Quaternion.identity);
                watcher.GetComponent<Content.Viend.WatcherVoid.WatcherVoidBehavior>().SetOwner(base.characterBody, base.GetComponent<VoidSurvivorController>());
                outer.SetNextStateToMain();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}*/