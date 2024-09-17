using System;
using EntityStates.Merc.Weapon;
using static RoR2.CharacterMotor;

namespace ChaoticSkills.EntityStates.Engineer {
    public class Thruster : BaseState {
        private float forwardForce = 3600f;
        private float upwardForce = 2400f;
        private float upwardDelay = 0.7f;
        private bool hasThrusted = false;

        public override void OnEnter()
        {
            base.OnEnter();
            base.characterMotor.velocity = Vector3.zero;
            base.characterMotor.ApplyForce(Vector3.up * upwardForce, true);
            AkSoundEngine.PostEvent(Events.Play_item_use_BFG_explode, base.gameObject);
        }

        public override void OnExit()
        {
            base.OnExit();
            On.RoR2.GlobalEventManager.OnCharacterHitGroundServer -= Impact;
        }

        private void Impact(On.RoR2.GlobalEventManager.orig_OnCharacterHitGroundServer orig, GlobalEventManager self, CharacterBody body, HitGroundInfo info) {
            if (body == base.characterBody) {
                orig(self, body, info);
                float damageCoeff = Mathf.Clamp(1f + (Mathf.Abs(info.velocity.y * 5) * 0.3f), 1f, 25f);
                float radiusCoeff = Mathf.Clamp(1f + (Mathf.Abs(info.velocity.y * 5) * 0.3f), 1f, 3f);
                BlastAttack attack = new();
                attack.damageType = DamageType.Stun1s;
                attack.baseDamage = base.damageStat * damageCoeff;
                attack.radius = 3.5f * radiusCoeff;
                attack.position = body.corePosition;
                attack.crit = base.RollCrit();
                attack.damageType = DamageType.Stun1s;
                attack.attackerFiltering = AttackerFiltering.NeverHitSelf;
                attack.losType = BlastAttack.LoSType.None;
                attack.falloffModel = BlastAttack.FalloffModel.Linear;
                attack.baseForce = 500f;
                attack.procCoefficient = 1f;
                attack.teamIndex = base.GetTeam();
                attack.inflictor = base.gameObject;
                attack.attacker = base.gameObject;

                if (base.isAuthority) {
                    // Debug.Log("firing attack");
                    attack.Fire();
                }

                EffectManager.SpawnEffect(Utils.Paths.GameObject.OmniImpactVFX.Load<GameObject>(), new EffectData {
                    scale = radiusCoeff,
                    origin = body.corePosition,
                    rotation = Quaternion.identity,
                }, true);

                AkSoundEngine.PostEvent(Events.Play_item_proc_fallboots_impact, base.gameObject);

                outer.SetNextStateToMain();
            }
            else {
                // Debug.Log("impact wasnt us");
                orig(self, body, info);
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.fixedAge >= upwardDelay && !hasThrusted) {
                // Debug.Log("thrusting");
                On.RoR2.GlobalEventManager.OnCharacterHitGroundServer += Impact;
                hasThrusted = true;
                base.characterMotor.velocity = Vector3.zero;
                base.characterMotor.ApplyForce((base.GetAimRay().direction) * forwardForce, true);
                AkSoundEngine.PostEvent(Events.Play_item_use_BFG_explode, base.gameObject);
            }
        }
    }
}