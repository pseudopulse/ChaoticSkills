using System;
using EntityStates.CaptainSupplyDrop;
using EntityStates.Captain.Weapon;

namespace ChaoticSkills.EntityStates.Captain {
    public class DesignState : BaseState {
        private float range = 10;
        private GameObject prefab = Utils.Paths.GameObject.MoonBatteryDesignPulse.Load<GameObject>();
        private int force = 3000;
        private float duration = 3f;
        private float delay = 1f;
        private float stopwatch = 0f;

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (true) { // useless check but im too lazy to unindent this
                stopwatch += Time.fixedDeltaTime;

                if (stopwatch >= delay) {
                    stopwatch = 0f;

                    GameObject pulse = GameObject.Instantiate(prefab, base.transform);
                    
                    BlastAttack attack = new();
                    attack.damageType = DamageType.CrippleOnHit | DamageType.Silent;
                    attack.baseDamage = 0;
                    attack.radius = range;
                    attack.position = base.transform.position;
                    attack.crit = false;
                    attack.baseForce = force;
                    attack.procCoefficient = 0f;

                    if (NetworkServer.active) {
                        attack.Fire();
                    }

                    AkSoundEngine.PostEvent(Events.Play_moonBrother_orb_slam_impact, base.gameObject);
                }
            }
        }
    }
}