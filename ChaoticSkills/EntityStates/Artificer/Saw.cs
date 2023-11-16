using System;

namespace ChaoticSkills.EntityStates.Artificer {
    public class Saw : BaseSkillState {
        public float DamageCoefficient = 4f;
        public int hitrate = 5;
        public float duration = 1.1f;
        public Transform saw;
        public OverlapAttack attack;
        private float stopwatch = 0f;

        public override void OnEnter()
        {
            base.OnEnter();
            Transform muzzle = FindModelChild("MuzzleBetween");
            GameObject sawObj = GameObject.Instantiate(Content.Artificer.Saw.SawPrefab, muzzle);
            saw = sawObj.transform;
            saw.gameObject.SetActive(true);
            Transform mdl = saw.Find("mdlMageSaw");
            // mdl.localRotation = Quaternion.Euler(270, 0, 0);
            mdl.localRotation = Quaternion.Euler(0f, 0f, 0f);
            mdl.Find("HitBox").localRotation = Quaternion.Euler(0f, 0f, 0f);
            
            saw.localPosition += 3.4f * base.transform.forward;

            hitrate = Mathf.CeilToInt(hitrate * base.attackSpeedStat);
            
            PlayAnimation("Gesture, Additive", "ChargeNovaBomb", "ChargeNovaBomb.playbackRate", 0.1f);
            PlayAnimation("Gesture, Additive", "FireNovaBomb", "FireNovaBomb.playbackRate", duration);
            
            attack = new();
            attack.damage = damageStat * (DamageCoefficient / 5f);
            attack.attacker = base.gameObject;
            attack.procCoefficient = 0.7f;
            attack.hitBoxGroup = saw.GetComponent<HitBoxGroup>();
            attack.hitEffectPrefab = Utils.Paths.GameObject.ToolbotBuzzsawImpactEffectLoop.Load<GameObject>();
            attack.isCrit = base.RollCrit();
            attack.teamIndex = base.GetTeam();
            attack.damageType = DamageType.IgniteOnHit;

            AkSoundEngine.PostEvent(Events.Play_lemurian_fireball_shoot, base.gameObject);
            AkSoundEngine.PostEvent(Events.Play_lemurian_fireball_flight_loop, base.gameObject);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            base.characterBody.SetAimTimer(0.2f);
            base.characterBody.isSprinting = false;

            stopwatch -= Time.fixedDeltaTime;

            if (base.isAuthority && stopwatch <= 0f) {
                stopwatch = (duration / hitrate);
                attack.ResetIgnoredHealthComponents();
                attack.Fire();
            }

            if (base.fixedAge >= duration) {
                outer.SetNextStateToMain();
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            EffectManager.SimpleEffect(Utils.Paths.GameObject.FireMeatBallExplosion.Load<GameObject>(), saw.transform.position, Quaternion.identity, false);
            Destroy(saw.gameObject);
            AkSoundEngine.PostEvent(Events.Stop_lemurian_fireball_flight_loop, base.gameObject);
            AkSoundEngine.PostEvent(Events.Play_fireballsOnHit_impact, base.gameObject);
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}