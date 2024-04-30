using System;

namespace ChaoticSkills.Content.Commando {
    public class IceGrenade : SkillBase<IceGrenade> {
        public override SerializableEntityStateType ActivationState => ContentAddition.AddEntityState<EntityStates.Commando.IceGrenade>(out bool _);
        public override float Cooldown => 7;
        public override bool DelayCooldown => true;
        public override string Description => "<style=cIsUtility>Freezing</style>. Toss an <style=cIsDamage>ice grenade</style> that erupts into a <style=cIsUtility>freezing blast</style>.";
        public override bool Agile => false;
        public override bool IsCombat => true;
        public override string LangToken => "IceGrenade";
        public override int StockToConsume => 1;
        public override int MaxStock => 1;
        public override bool SprintCancelable => false;
        public override UnlockableDef Unlock => null;
        public override string Machine => "Weapon";
        public override Sprite SkillIcon => Main.Assets.LoadAsset<Sprite>("Assets/Icons/Commando/FrostGrenade.png");
        public override SkillSlot Slot => SkillSlot.Special;
        public override string Survivor => Utils.Paths.GameObject.CommandoBody;
        public override string Name => "Frost Grenade";
        public override List<string> Keywords => new() { Utils.Keywords.Freeze };
        public static GameObject IceGrenadeProjectile;
        public static GameObject ExplosionVFX;
        public static AnimatorOverrideController animatorOverrideController;
        public override void PostCreation()
        {
           IceGrenadeProjectile = PrefabAPI.InstantiateClone(Utils.Paths.GameObject.CommandoGrenadeProjectile.Load<GameObject>(), "FrostGrenade");
           ExplosionVFX = PrefabAPI.InstantiateClone(Utils.Paths.GameObject.IceRingExplosion.Load<GameObject>(), "IceExplosionVFX");
           
           ProjectileImpactExplosion explosion = IceGrenadeProjectile.GetComponent<ProjectileImpactExplosion>();
           explosion.blastDamageCoefficient = 0f;
           explosion.blastRadius = 14f;
           explosion.impactEffect = ExplosionVFX;
           explosion.impactOnWorld = true;
           explosion.timerAfterImpact = false;
           explosion.destroyOnEnemy = true;
           explosion.destroyOnWorld = true;
           explosion.blastProcCoefficient = 1f;

           IceGrenadeProjectile.layer = LayerIndex.projectile.intVal;

           ProjectileDamage damage = IceGrenadeProjectile.GetComponent<ProjectileDamage>();
           damage.damageType = DamageType.Freeze2s;

           ContentAddition.AddProjectile(IceGrenadeProjectile);
           ContentAddition.AddEffect(ExplosionVFX);

           GameObject surv = Survivor.Load<GameObject>();
           GameObject model = surv.GetComponent<ModelLocator>()._modelTransform.gameObject;
           Animator anim = model.GetComponent<Animator>();
           RuntimeAnimatorController cont = anim.runtimeAnimatorController;

           
        }
    }
}