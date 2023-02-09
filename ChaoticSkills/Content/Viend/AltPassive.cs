/*using System;
using RoR2.Orbs;

namespace ChaoticSkills.Content.Viend {
    public class AltPassive : SkillBase<AltPassive> {
        public override SerializableEntityStateType ActivationState => new SerializableEntityStateType(typeof(Idle));
        public override float Cooldown => 0f;
        public override bool DelayCooldown => false;
        public override string Description => "<style=cIsVoid>The Void</style> assists you...";
        public override bool Agile => false;
        public override bool IsCombat => true;
        public override string LangToken => "CallOfTheVoid";
        public override int StockToConsume => 0;
        public override int MaxStock => 1;
        public override bool SprintCancelable => false;
        public override UnlockableDef Unlock => null;
        public override string Machine => "Weapon";
        public override Sprite SkillIcon => null;
        public override SkillSlot Slot => SkillSlot.Special;
        public override string Survivor => Utils.Paths.GameObject.VoidSurvivorBody;
        public override string Name => "C??all of th?e Voi??d";
        public override bool Passive => true;

        public override void PostCreation()
        {
            base.PostCreation();
            GameObject viend = Survivor.Load<GameObject>();
            viend.AddStateMachine<EntityStates.Viend.VoidPassive>("VoidPassive", false);

            On.RoR2.VoidSurvivorController.OnEnable += (orig, self) => {
                CharacterBody body = self.GetComponent<CharacterBody>();
            };

            On.RoR2.CharacterBody.Start += (orig, self) => {
                orig(self);
                EntityStateMachine machine = EntityStateMachine.FindByCustomName(self.gameObject, "VoidPassive");
                if (self.isPlayerControlled && machine) {
                    if (self.HasSkillEquipped(SkillDef)) {
                        machine.enabled = true;
                        machine.SetNextStateToMain();
                        GameObject.Destroy(self.GetComponent<VoidSurvivorController>());
                        return;
                    }
                    else {
                        machine.enabled = false;
                        machine.SetNextState(new Idle());
                        orig(self);
                        return;
                    }
                }
                if (self.teamComponent.teamIndex != TeamIndex.Void) {
                    return;
                }
                foreach (PlayerCharacterMasterController masterController in PlayerCharacterMasterController.instances) {
                    if (masterController.body && masterController.body.HasSkillEquipped(SkillDef)) {
                        self.master.teamIndex = TeamIndex.Player;
                        self.teamComponent.teamIndex = TeamIndex.Player;
                        BaseAI ai = self.master.GetComponent<BaseAI>();
                        if (ai) {
                            ai.enemyAttention = 0;
                            ai.ForceAcquireNearestEnemyIfNoCurrentEnemy();
                        }
                    }
                }
            };
        }
    }
}*/