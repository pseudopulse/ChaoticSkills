using System;

namespace ChaoticSkills.Content.MULT {
    public class ElfMelter : SkillBase<ElfMelter> {
        public override SerializableEntityStateType ActivationState => ContentAddition.AddEntityState<EntityStates.MULT.SlagBlast>(out bool _);
        public override float Cooldown => 0f;
        public override bool DelayCooldown => false;
        public override string Description => "<style=cIsDamage>Ignite.</style> Spout 3 <style=cIsDamage>molten slag balls</style> that <style=cIsUtility>slow</style> and deal <style=cIsDamage>180% damage</style>. <style=cIsUtility>Sets terrain ablaze</style>.";
        public override bool Agile => false;
        public override bool IsCombat => true;
        public override string LangToken => "ElfMelter";
        public override int StockToConsume => 0;
        public override int MaxStock => 1;
        public override bool SprintCancelable => false;
        public override UnlockableDef Unlock => null;
        public override string Machine => "Weapon";
        public override Sprite SkillIcon => Main.Assets.LoadAsset<Sprite>("SlagCannon.png");
        public override SkillSlot Slot => SkillSlot.Primary;
        public override string Survivor => Utils.Paths.GameObject.ToolbotBody;
        public override bool AutoApply => false;
        public override List<string> Keywords => new() { Utils.Keywords.Ignite };
        public override string Name => "Heat Flush";
        public static GameObject ElfMelterSlagOrb;
        public static GameObject ElfMelterSlagPool;

        public override void PostCreation()
        {
            GameObject surv = Survivor.Load<GameObject>();
            GenericSkill s1 = surv.GetComponents<GenericSkill>().First(x => x.skillName != null && x.skillName == "FireNailgun");
            GenericSkill s2 = surv.GetComponents<GenericSkill>().First(x => x.skillName != null && x.skillName == "FireSpear");
            
            SkillFamily family = s1.skillFamily;

            Array.Resize(ref family.variants, family.variants.Length + 1);
                
            family.variants[family.variants.Length - 1] = new SkillFamily.Variant {
                skillDef = SkillDef,
                unlockableDef = Unlock,
                viewableNode = new ViewablesCatalog.Node(SkillDef.skillNameToken, false, null)
            };

            family = s2.skillFamily;

            Array.Resize(ref family.variants, family.variants.Length + 1);
                
            family.variants[family.variants.Length - 1] = new SkillFamily.Variant {
                skillDef = SkillDef,
                unlockableDef = Unlock,
                viewableNode = new ViewablesCatalog.Node(SkillDef.skillNameToken, false, null)
            };

            ElfMelterSlagPool = PrefabAPI.InstantiateClone(Utils.Paths.GameObject.MolotovProjectileDotZone.Load<GameObject>(), "ElfMelterSlagPool");
            ElfMelterSlagPool.transform.localScale *= 0.7f;


            ElfMelterSlagOrb = PrefabAPI.InstantiateClone(Utils.Paths.GameObject.Fireball.Load<GameObject>(), "ElfMelterProjectile");
            AntiGravityForce force = ElfMelterSlagOrb.AddComponent<AntiGravityForce>();
            force.antiGravityCoefficient = -0.7f;
            force.rb = ElfMelterSlagOrb.GetComponent<Rigidbody>();
            ElfMelterSlagOrb.AddComponent<ElfMelterImpact>();
            ElfMelterSlagOrb.GetComponent<ProjectileDamage>().damageType = DamageType.IgniteOnHit | DamageType.SlowOnHit;
            ElfMelterSlagOrb.GetComponent<ProjectileSimple>().desiredForwardSpeed = 80f;

            ElfMelterSlagPool.GetComponent<ProjectileDamage>().damageType |= DamageType.SlowOnHit;

            ContentAddition.AddProjectile(ElfMelterSlagOrb);
            ContentAddition.AddProjectile(ElfMelterSlagPool);

            Utils.Paths.SkillDef.ToolbotBodyStunDrone.Load<SkillDef>().interruptPriority = InterruptPriority.PrioritySkill;
        }

        public override SkillDef GetSkillDef()
        {
            ToolbotWeaponSkillDef sd = ScriptableObject.CreateInstance<ToolbotWeaponSkillDef>();
            ToolbotWeaponSkillDef scrap = Utils.Paths.ToolbotWeaponSkillDef.ToolbotBodyFireGrenadeLauncher.Load<ToolbotWeaponSkillDef>();
            sd.animatorWeaponIndex = scrap.animatorWeaponIndex;
            sd.crosshairPrefab = scrap.crosshairPrefab;
            sd.crosshairSpreadCurve = scrap.crosshairSpreadCurve;
            sd.enterGestureAnimState = scrap.enterGestureAnimState;
            sd.exitGestureAnimState = scrap.exitGestureAnimState;
            sd.entryAnimState = scrap.entryAnimState;
            sd.exitAnimState = scrap.exitAnimState;
            sd.entrySound = scrap.entrySound;
            sd.stanceName = scrap.stanceName;

            return sd;
        }

        private class ElfMelterImpact : MonoBehaviour, IProjectileImpactBehavior
        {
            public void OnProjectileImpact(ProjectileImpactInfo impactInfo)
            {
                Vector3 position = impactInfo.estimatedPointOfImpact;
                Vector3 normal = impactInfo.estimatedImpactNormal;
                if (impactInfo.collider && impactInfo.collider.GetComponent<HurtBox>()) {
                    if (Physics.Raycast(position, Vector3.down, out RaycastHit hitinfo, 5f, LayerIndex.world.mask, QueryTriggerInteraction.Ignore)) {
                        position = hitinfo.point;
                        normal = Vector3.up;
                    }
                    else {
                        return;
                    }
                }

                EffectManager.SimpleImpactEffect(Utils.Paths.GameObject.ExplosionSolarFlare.Load<GameObject>(), position, normal, false);

                ProjectileController controller = base.GetComponent<ProjectileController>();
                ProjectileDamage damage = base.GetComponent<ProjectileDamage>();

                FireProjectileInfo info = new();
                info.position = position;
                info.rotation = Util.QuaternionSafeLookRotation(normal * -1f);
                info.damage = damage.damage;
                info.crit = false;
                info.owner = controller.owner;
                info.projectilePrefab = ElfMelterSlagPool;

                AkSoundEngine.PostEvent(Events.Play_wWormExplosion, base.gameObject);

                if (NetworkServer.active) {
                    ProjectileManager.instance.FireProjectile(info);
                }
            }
        }
    }
}