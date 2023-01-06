using System;
using EntityStates.CaptainSupplyDrop;
using EntityStates.Captain.Weapon;

namespace ChaoticSkills.EntityStates.Captain {
    public class ShotgunFire : BaseState {
        private GameObject prefab => Utils.Paths.GameObject.VoidJailerDart.Load<GameObject>();
        private int projCount = 12;
        private float damageCoeff = 1f;

        public override void OnEnter()
        {
            base.OnEnter();
            Ray aim = base.GetAimRay();
            for (int i = 0; i < projCount; i++) {
                FireProjectileInfo info = new FireProjectileInfo {
                    projectilePrefab = prefab,
                    crit = RollCrit(),
                    damage = damageStat * damageCoeff,
                    owner = base.gameObject,
                    position = FindModelChild("MuzzleGun").position,
                    rotation = Util.QuaternionSafeLookRotation(Util.ApplySpread(aim.direction, -1.6f, 1.6f, 1f, 1f))
                };

                ProjectileManager.instance.FireProjectile(info);
            }

            if (base.GetComponent<Content.Captain.OffensiveMicrobots.OffensiveMatrixController>()) {
                base.GetComponent<Content.Captain.OffensiveMicrobots.OffensiveMatrixController>().Fire(null);
            }

            AkSoundEngine.PostEvent(Events.Play_voidJailer_m1_shoot, base.gameObject);
            outer.SetNextStateToMain();
        }
    }
}