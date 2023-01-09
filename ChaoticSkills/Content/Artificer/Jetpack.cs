using System;

namespace ChaoticSkills.Content.Artificer {
    public class Jetpack : SkillBase<Jetpack> {
        public override SerializableEntityStateType ActivationState => new SerializableEntityStateType(typeof(Idle));
        public override float Cooldown => 0f;
        public override bool DelayCooldown => true;
        public override string Description => "Hold <style=cIsUtility>jump</style> to <style=cIsUtility>fly into the air</style>. <style=cDeath>Flying drains fuel</style>. Fuel passively regenerates when not flying.";
        public override bool Agile => false;
        public override bool IsCombat => false;
        public override string LangToken => "Jetpack";
        public override int StockToConsume => 0;
        public override int MaxStock => 1;
        public override UnlockableDef Unlock => null;
        public override string Machine => "CoolerJet";
        public override Sprite SkillIcon => Main.Assets.LoadAsset<Sprite>("Assets/Icons/Artificer/Jetpack.png");
        public override SkillSlot Slot => SkillSlot.Primary;
        public override string Survivor => Utils.Paths.GameObject.MageBody;
        public override bool MustKeyPress => false;
        public override string Name => "Overcharged Jetpack";
        public override bool Passive => true;
        public static GameObject jetpackUIPrefab;
        public override void PostCreation()
        {
            base.PostCreation();
            GameObject mageBody = Survivor.Load<GameObject>();
            EntityStateMachine machine = mageBody.AddComponent<EntityStateMachine>();
            machine.customName = "CoolerJet";
            SerializableEntityStateType state = ContentAddition.AddEntityState<EntityStates.Artificer.Jetpack>(out bool _);
            machine.initialStateType = state;
            machine.mainStateType = state;
            machine.enabled = false;

            NetworkStateMachine machines = mageBody.GetComponent<NetworkStateMachine>();
            List<EntityStateMachine> machinesList = machines.stateMachines.ToList();
            machinesList.Add(machine);
            machines.stateMachines = machinesList.ToArray();

            On.RoR2.CharacterBody.Start += (orig, self) => {
                orig(self);
                foreach (GenericSkill skill in self.GetComponents<GenericSkill>()) {
                    if (skill.skillName.ToLower().Contains("passive") && skill.skillDef == SkillDef) {
                        EntityStateMachine.FindByCustomName(self.gameObject, "Jet").enabled = false;
                        EntityStateMachine.FindByCustomName(self.gameObject, "CoolerJet").enabled = true;
                        EntityStateMachine.FindByCustomName(self.gameObject, "CoolerJet").SetNextState(new EntityStates.Artificer.Jetpack());
                    }
                }
            };

            jetpackUIPrefab = PrefabAPI.InstantiateClone(Main.Assets.LoadAsset<GameObject>("Assets/Prefabs/Artificer/Jetpack/FuelIndicator.prefab"), "ArtiFuelHUD");
        }
    }
}