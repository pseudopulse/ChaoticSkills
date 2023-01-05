using System;
using EntityStates.CaptainSupplyDrop;
using EntityStates.Captain.Weapon;

namespace ChaoticSkills.EntityStates.Captain {
    public class VoidState : BaseState {
        private SphereZone zone => base.GetComponent<SphereZone>();
        private float stopwatch = 0f;
        private float interval = 0.3f;
        private float damageFraction = 1f;

        public override void OnEnter()
        {
            base.OnEnter();
            zone.rangeIndicator.parent.gameObject.SetActive(true);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (NetworkServer.active) {
                stopwatch += Time.fixedDeltaTime;

                if (stopwatch >= interval) {
                    stopwatch = 0f;

                    TeamComponent[] teamMembers = TeamComponent.GetTeamMembers(TeamIndex.Monster).ToArray();

                    foreach (TeamComponent component in teamMembers) {
                        if (component.body && component.body.healthComponent) {
                            CharacterBody body = component.body;
                            HealthComponent health = body.healthComponent;

                            if (zone.IsInBounds(body.corePosition)) {
                                float damage = health.health * (damageFraction * 0.01f);

                                DamageInfo info = new();
                                info.damage = damage;
                                info.crit = false;
                                info.damageColorIndex = DamageColorIndex.Void;
                                info.position = body.corePosition;
                                info.damageType = DamageType.BypassBlock | DamageType.BypassArmor;

                                health.TakeDamage(info);
                                body.AddTimedBuff(RoR2Content.Buffs.VoidFogMild, 0.3f, 1);
                            }
                        }
                    }
                }
            }
        }
    }   
}