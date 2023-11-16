using System;
using RoR2EntityStates = EntityStates;

namespace ChaoticSkills.Content.Merc {
    public class Strike : SkillBase<Strike>
    {
        public override SerializableEntityStateType ActivationState => new(typeof(EntityStates.Merc.PhantomStrike));

        public override float Cooldown => 0f;

        public override string Machine => "Weapon";

        public override int MaxStock => 1;
        public override bool DelayCooldown => true;
        public override int StockToConsume => 0;

        public override string LangToken => "MERC_PHANTOMSTRIKE";

        public override string Name => "Phantom Strike";

        public override string Description => "Unleash 3 phantom mercenaries that slash for 200% damage.";

        public override Sprite SkillIcon => Main.Assets.LoadAsset<Sprite>("PhantomStrike.png");
        public override SkillSlot Slot => SkillSlot.Primary;
        public override string Survivor => Utils.Paths.GameObject.MercBody;
        public override bool Agile => true;
        public static GameObject GhostMercPrefab;

        public override void PostCreation()
        {
            base.PostCreation();

            GhostMercPrefab = PrefabAPI.InstantiateClone(Utils.Paths.GameObject.MercBody.Load<GameObject>(), "GhostMerc");
            GhostMercPrefab.GetComponent<CharacterBody>().bodyFlags |= CharacterBody.BodyFlags.Masterless;
            GhostMercPrefab.GetComponent<KinematicCharacterController.KinematicCharacterMotor>().enabled = false;
            GhostMercPrefab.GetComponent<CharacterMotor>().enabled = false;
            GhostMercPrefab.GetComponent<EntityStateMachine>().initialStateType = new(typeof(RoR2EntityStates.GenericCharacterMain));
            GhostMercPrefab.layer = LayerIndex.noCollision.intVal;
            ContentAddition.AddBody(GhostMercPrefab);

            On.RoR2.CharacterModel.UpdateOverlays += (orig, self) => {
                orig(self);

                if (self.body && self.body.gameObject.name.Contains("GhostMerc")) {
                    AddOverlay(CharacterModel.ghostMaterial, true);
                }

                void AddOverlay(Material overlayMaterial, bool condition)
                {
                    if (self.activeOverlayCount < CharacterModel.maxOverlays && condition)
                    {
                        self.currentOverlays[self.activeOverlayCount++] = overlayMaterial;
                    }
                }
            };
        }
    }
}
