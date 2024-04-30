using System;
using System.Collections;
using System.Diagnostics.Tracing;
using System.Threading.Tasks;
using EntityStates.Toolbot;
using RoR2.DirectionalSearch;

namespace ChaoticSkills.EntityStates.MULT {
    public class ChargePileDriver : ChargeSpear {
        public override void OnEnter()
        {
            baseChargeDuration = 2.5f;
            GetModelAnimator().SetInteger("weaponStance", Content.MULT.PileDriver.animWeaponIndexForSpear);
            base.OnEnter();
        }
        public override void FixedUpdate()
        {
            if (base.isAuthority && base.fixedAge >= minChargeDuration && !IsKeyDownAuthority()) {
                outer.SetNextState(new FirePileDriver() {
                    activatorSkillSlot = activatorSkillSlot,
                    charge = Util.Remap(base.fixedAge, minChargeDuration, chargeDuration, 0f, 1f)
                });

                return;
            }

            base.FixedUpdate();
        }
    }

    public class FirePileDriver : FireSpear {
        public override void ModifyBullet(BulletAttack bulletAttack)
        {
            bulletAttack.maxDistance = 9f;
            bulletAttack.radius = 3f;
            bulletAttack.smartCollision = true;
            bulletAttack.stopperMask = LayerIndex.world.mask;
            bulletAttack.muzzleName = this.muzzleName;
            bulletAttack.damage = base.damageCoefficient * (Util.Remap(charge, 0f, 1f, 6f, 20f));
            bulletAttack.force = 4000f;
            bulletAttack.damageType = DamageType.Stun1s;
        }

        public override void FireBullet(Ray aimRay)
        {
            base.FireBullet(aimRay);
            AkSoundEngine.PostEvent(Events.Play_MULT_m1_snipe_shoot, base.gameObject);
            AkSoundEngine.PostEvent(Events.Play_MULT_m1_snipe_shoot, base.gameObject);
            AkSoundEngine.PostEvent(Events.Play_MULT_m1_snipe_shoot, base.gameObject);
        }

        public override void OnExit()
        {
            base.OnExit();
            GetModelAnimator().SetInteger("weaponStance", (skillLocator.primary.skillDef as ToolbotWeaponSkillDef).animatorWeaponIndex);
        }
    }
}