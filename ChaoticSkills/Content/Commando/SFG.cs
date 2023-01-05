using System;

namespace ChaoticSkills.Content.Commando {
    public class SFG : SkillBase<SFG> {
        public override SerializableEntityStateType ActivationState => ContentAddition.AddEntityState<EntityStates.Commando.SFG>(out bool _);
        public override float Cooldown => 0f;
        public override bool DelayCooldown => false;
        public override string Description => "<style=cIsDamage>Stunning</style>. Fire a slow <style=cIsDamage>energy blast</style> for <style=cIsDamage>700% damage</style> in an <style=cIsUtility>explosion</style>. Has strong recoil.";
        public override bool Agile => false;
        public override bool IsCombat => true;
        public override string LangToken => "SFG";
        public override int StockToConsume => 0;
        public override int MaxStock => 1;
        public override UnlockableDef Unlock => null;
        public override string Machine => "Weapon";
        public override Sprite SkillIcon => Main.Assets.LoadAsset<Sprite>("Assets/Icons/Commando/SFG.png");
        public override SkillSlot Slot => SkillSlot.Primary;
        public override string Survivor => Utils.Paths.GameObject.CommandoBody;
        public override string Name => "SFG-900";
        public override List<string> Keywords => new() { Utils.Keywords.Stun };
        public static GameObject SFGProjectile;
        public static GameObject SFGGhost;
        public static GameObject SFGExplosion;

        public override void PostCreation()
        {
            SFGProjectile = PrefabAPI.InstantiateClone(Utils.Paths.GameObject.BeamSphere.Load<GameObject>(), "SFGOrb");
            SFGGhost = PrefabAPI.InstantiateClone(Utils.Paths.GameObject.BeamSphereGhost.Load<GameObject>(), "SFGGhost");
            SFGExplosion = PrefabAPI.InstantiateClone(Utils.Paths.GameObject.BeamSphereExplosion.Load<GameObject>(), "SFGExplosion");

            Utils.Paths.GameObject.BeamSphereOrbEffect.Load<GameObject>().RemoveComponents<AkEvent>();

            SFGProjectile.RemoveComponents<AkEvent>();
            SFGProjectile.RemoveComponents<AkGameObj>();

            SFGProjectile.transform.localScale *= 0.3f;
            SFGGhost.transform.localScale *= 0.3f;
            SFGExplosion.transform.localScale *= 0.3f;

            ProjectileImpactExplosion SFGImpact = SFGProjectile.GetComponent<ProjectileImpactExplosion>();
            SFGImpact.blastDamageCoefficient = 1f;
            SFGImpact.blastProcCoefficient = 0.5f;
            SFGImpact.blastRadius = 5f;
            SFGImpact.explosionEffect = SFGExplosion;

            SFGProjectile.RemoveComponent<ProjectileProximityBeamController>();
            SFGProjectile.RemoveComponent<WindZone>();

            ProjectileController SFGController = SFGProjectile.GetComponent<ProjectileController>();
            SFGController.ghostPrefab = SFGGhost;

            ProjectileSimple SFGSimple = SFGProjectile.GetComponent<ProjectileSimple>();
            SFGSimple.desiredForwardSpeed = 45;

            ProjectileDamage SFGDamage = SFGProjectile.GetComponent<ProjectileDamage>();
            SFGDamage.damageType = DamageType.Stun1s;

            ContentAddition.AddProjectile(SFGProjectile);
        }
    }
}