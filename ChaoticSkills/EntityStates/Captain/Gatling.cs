using System;
using EntityStates.CaptainSupplyDrop;
using EntityStates.Captain.Weapon;

namespace ChaoticSkills.EntityStates.Captain {
    public class Gatling : BaseState {
        private GameObject prefab => Utils.Paths.GameObject.VoidRaidCrabMissileProjectile.Load<GameObject>();
        private float damageCoeff => 0.7f;
        private float shotsPerSecond = 7;
        private float delay => (1f / shotsPerSecond) / base.attackSpeedStat;
        private float stopwatch = 0f;
        private float revUpTime = 1f;
        private int shotsFired = 0;
        private float revDown = 1f;
        private bool isRevvingDown = false;

        public override void OnEnter()
        {
            base.OnEnter();
            PlayCrossfade("Gesture, Override", "ChargeCaptainShotgun", "ChargeCaptainShotgun.playbackRate", revUpTime, 0.1f);
		    PlayCrossfade("Gesture, Additive", "ChargeCaptainShotgun", "ChargeCaptainShotgun.playbackRate", revUpTime, 0.1f);
            base.characterBody.AddBuff(RoR2Content.Buffs.Slow50);
        }

        public override void OnExit()
        {
            base.OnExit();
            base.characterBody.RemoveBuff(RoR2Content.Buffs.Slow50);
            PlayAnimation("Gesture, Additive", "FireCaptainShotgun");
		    PlayAnimation("Gesture, Override", "FireCaptainShotgun");
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.isAuthority) {
                base.characterDirection.forward = base.GetAimRay().direction;
                stopwatch += Time.fixedDeltaTime;
                if (stopwatch >= delay && base.fixedAge >= revUpTime && !isRevvingDown) {
                    stopwatch = 0f;
                    shotsFired++;

                    FireProjectileInfo info = new();
                    info.crit = base.RollCrit();
                    info.damage = base.damageStat * damageCoeff;
                    info.procChainMask = new();
                    info.damageColorIndex = DamageColorIndex.Void;
                    info.projectilePrefab = prefab;
                    info.owner = base.gameObject;
                    info.position = base.FindModelChild("MuzzleGun").position;
                    info.rotation = Util.QuaternionSafeLookRotation(Util.ApplySpread(base.GetAimRay().direction, -3f, 3f, 1, 1));

                    PlayCrossfade("Gesture, Override", "ChargeCaptainShotgun", "ChargeCaptainShotgun.playbackRate", revUpTime, 0.1f);
		            PlayCrossfade("Gesture, Additive", "ChargeCaptainShotgun", "ChargeCaptainShotgun.playbackRate", revUpTime, 0.1f);

                    ProjectileManager.instance.FireProjectile(info);

                    if (shotsFired % 7 == 0) {
                        if (base.gameObject.GetComponent<Content.Captain.OffensiveMicrobots.OffensiveMatrixController>()) {
                            Content.Captain.OffensiveMicrobots.OffensiveMatrixController controller = base.gameObject.GetComponent<Content.Captain.OffensiveMicrobots.OffensiveMatrixController>();
                            controller.Fire(null);
                        }
                    }

                    AkSoundEngine.PostEvent(Events.Play_voidRaid_m1_shoot, base.gameObject);
                }
            }

            if (!base.inputBank.skill1.down && !isRevvingDown) {
                isRevvingDown = true;
                revDown = base.fixedAge + 1f;
                PlayCrossfade("Gesture, Override", "ChargeCaptainShotgun", "ChargeCaptainShotgun.playbackRate", revUpTime, 0.1f);
		        PlayCrossfade("Gesture, Additive", "ChargeCaptainShotgun", "ChargeCaptainShotgun.playbackRate", revUpTime, 0.1f);
            }

            if (base.fixedAge >= revDown && isRevvingDown) {
                outer.SetNextStateToMain();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}