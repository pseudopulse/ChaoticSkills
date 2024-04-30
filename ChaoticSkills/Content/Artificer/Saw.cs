using System;
using HarmonyLib;
using R2API.Utils;
using Rewired.ComponentControls.Effects;
using TMPro;

namespace ChaoticSkills.Content.Artificer {
    public class Saw : SkillBase<Saw> {
        public override SerializableEntityStateType ActivationState => ContentAddition.AddEntityState<EntityStates.Artificer.Saw>(out bool _);
        public override float Cooldown => 0f;
        public override bool DelayCooldown => true;
        public override string Description => "<style=cIsDamage>Ignite.</style> Swing a <style=cIsDamage>flaming saw</style> for <style=cIsDamage>400% damage per second</style>.";
        public override bool Agile => true;
        public override bool IsCombat => true;
        public override string LangToken => "Saw";
        public override int StockToConsume => 0;
        public override int MaxStock => 1;
        public override UnlockableDef Unlock => null;
        public override string Machine => "Weapon";
        public override Sprite SkillIcon => Main.Assets.LoadAsset<Sprite>("Flamerang.png");
        public override SkillSlot Slot => SkillSlot.Primary;
        public override string Survivor => Utils.Paths.GameObject.MageBody;
        public override bool MustKeyPress => false;
        public override bool SprintCancelable => false;
        public override string Name => "Flamerang";
        public static GameObject SawPrefab;
        public override void PostCreation()
        {
            base.PostCreation();
            SawPrefab = new("MageSaw");
            SawPrefab.SetActive(false);
            GameObject.DontDestroyOnLoad(SawPrefab);
            GameObject mdlSaw = PrefabAPI.InstantiateClone(Utils.Paths.GameObject.mdlSawmerang.Load<GameObject>(), "mdlMageSaw", false);
            mdlSaw.transform.localScale *= 0.5f;

            mdlSaw.GetComponents<MeshRenderer>().ForEachTry(x => {
                x.material = Utils.Paths.Material.matFirePillarParticle.Load<Material>();   
            });

            mdlSaw.transform.SetParent(SawPrefab.transform);

            RotateAroundAxis axis = mdlSaw.AddComponent<RotateAroundAxis>();
            axis.speed = RotateAroundAxis.Speed.Fast;
            axis.fastRotationSpeed = 720f;
            axis.rotateAroundAxis = RotateAroundAxis.RotationAxis.Y;

            GameObject hitbox = new("HitBox");
            hitbox.transform.localScale = new(3.4f, 2f, 3.4f);
            hitbox.transform.SetParent(mdlSaw.transform);

            HitBox hb = hitbox.AddComponent<HitBox>();

            HitBoxGroup group = SawPrefab.AddComponent<HitBoxGroup>();
            group.groupName = "MageSaw";
            group.hitBoxes = new HitBox[] { hb };
        }
    }
}