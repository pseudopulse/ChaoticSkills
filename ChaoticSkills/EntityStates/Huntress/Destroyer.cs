using System;

namespace ChaoticSkills.EntityStates.Huntress {
    public class Destroyer : BaseState {
        private float maxCharge = 2.6f;
        private float minCharge = 0.4f;
        private float minDamage = 9f;
        private float maxDamage = 39f;
        private float minScale = 1f;
        private float maxScale = 4f;
        public float targetDamage;
        public float targetScale;
        public bool ended = false;

        public override void OnEnter()
        {
            base.OnEnter();
            maxCharge = maxCharge / base.attackSpeedStat;
            if (base.isAuthority) {
                FireProjectileInfo info = new();
                info.crit = base.RollCrit();
                info.rotation = Util.QuaternionSafeLookRotation(base.GetAimRay().direction);
                info.owner = base.gameObject;
                info.position = base.characterBody.corePosition;
                info.projectilePrefab = Content.Huntress.Destroyer.DestroyerProjectile;

                ProjectileManager.instance.FireProjectile(info);
            }

            AkSoundEngine.PostEvent(Events.Play_item_use_BFG_charge, base.gameObject);
            base.characterBody.AddBuff(RoR2Content.Buffs.Slow50);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            targetDamage = Util.Remap(base.fixedAge, minCharge, maxCharge, minDamage, maxDamage) * base.damageStat;
            targetScale = Util.Remap(base.fixedAge, minCharge, maxCharge, minScale, maxScale);

            if (base.fixedAge >= minCharge && !IsKeyDown()) {
                outer.SetNextStateToMain();
            }
            else if (base.fixedAge >= maxCharge) {
                BullseyeSearch search = new();
                search.maxAngleFilter = 90f;
                search.maxDistanceFilter = 45f;
                search.teamMaskFilter = TeamMask.all;
                search.teamMaskFilter.RemoveTeam(base.GetTeam());
                search.searchOrigin = base.characterBody.corePosition;
                search.searchDirection = base.GetAimRay().direction;
                search.sortMode = BullseyeSearch.SortMode.None;
                search.filterByLoS = false;
                search.RefreshCandidates();
                foreach (HurtBox box in search.GetResults()) {
                    if (box.healthComponent) {
                        CharacterBody cb = box.healthComponent.body;
                        if (cb.GetComponent<SetStateOnHurt>()) {
                            cb.GetComponent<SetStateOnHurt>().SetStun(3f);
                        }
                    }
                }
                EffectManager.SpawnEffect(Utils.Paths.GameObject.SonicBoomEffect.Load<GameObject>(), new EffectData {
                    origin = base.characterBody.corePosition,
                    rotation = Quaternion.LookRotation(base.GetAimRay().direction)
                }, true);
                AkSoundEngine.PostEvent(Events.Play_item_use_BFG_explode, base.gameObject);
                outer.SetNextStateToMain();
            }
        }

        private bool IsKeyDown() {
            return base.inputBank.skill4.down;
        }

        public override void OnExit()
        {
            base.OnExit();
            AkSoundEngine.PostEvent(Events.Stop_item_use_BFG_loop, base.gameObject);
            base.characterBody.RemoveBuff(RoR2Content.Buffs.Slow50);
            ended = true;
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}