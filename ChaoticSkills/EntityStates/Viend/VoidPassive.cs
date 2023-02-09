using System;
using RoR2.Navigation;

namespace ChaoticSkills.EntityStates.Viend {
    public class VoidPassive : BaseState {
        public CharacterBody body => base.characterBody;
        public HealthComponent hc => base.healthComponent;
        private float stopwatchAlly = 0f;
        private float delayAlly = 10f;
        private WeightedSelection<GameObject> selection = new();
        private Xoroshiro128Plus rng => Run.instance.spawnRng;

        public override void OnEnter()
        {
            base.OnEnter();
            selection.AddChoice(Utils.Paths.GameObject.LemurianMaster.Load<GameObject>(), 0.2f);
            selection.AddChoice(Utils.Paths.GameObject.GolemMaster.Load<GameObject>(), 0.2f);
            selection.AddChoice(Utils.Paths.GameObject.WispMaster.Load<GameObject>(), 0.2f);
            selection.AddChoice(Utils.Paths.GameObject.GreaterWispMaster.Load<GameObject>(), 0.2f);
            selection.AddChoice(Utils.Paths.GameObject.VoidSurvivorMonsterMaster.Load<GameObject>(), 0.2f);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (NetworkServer.active) {
                stopwatchAlly += Time.fixedDeltaTime;
                if (stopwatchAlly >= delayAlly) {
                    stopwatchAlly = 0;
                    if (GetCurrentVoidAllies() <= 5) {
                        SummonVoidAlly();
                    }
                }
            }
        }

        public Vector3 GetPositionNearSelf() {
            if (SceneInfo.instance && SceneInfo.instance.groundNodes) {
                try {
                    NodeGraph graph = SceneInfo.instance.groundNodes;
                    NodeGraph.Node node = graph.nodes.Where(x => Vector3.Distance(x.position, body.corePosition) < 30).ToList().GetRandom(rng);
                    return node.position;
                }
                catch {
                    return body.corePosition + (Vector3.up * 3);
                }
            }
            else {
                return body.corePosition + (Vector3.up * 3);
            }
        }

        public CharacterMaster SummonVoidAlly() {
            MasterSummon summon = new();
            summon.position = GetPositionNearSelf();
            summon.rotation = Quaternion.identity;
            summon.ignoreTeamMemberLimit = false;
            summon.masterPrefab = selection.Evaluate(rng.nextNormalizedFloat);
            summon.useAmbientLevel = false;
            summon.summonerBodyObject = base.gameObject;
            summon.teamIndexOverride = base.GetTeam();
            summon.preSpawnSetupCallback += (CharacterMaster c) => {
                c.inventory.SetEquipmentIndex(DLC1Content.Equipment.EliteVoidEquipment.equipmentIndex);
            };

            EffectManager.SpawnEffect(Utils.Paths.GameObject.ElementalRingVoidImplodeEffect.Load<GameObject>(), new EffectData {
                origin = summon.position,
                scale = 3
            }, true);

            return summon.Perform();
        }

        public int GetCurrentVoidAllies() {
            int total = 0;
            foreach (TeamComponent com in TeamComponent.GetTeamMembers(base.GetTeam())) {
                if (com.body && com.body.equipmentSlot.equipmentIndex == DLC1Content.Equipment.EliteVoidEquipment.equipmentIndex) {
                    total++;
                }
            }
            return total;
        }
    }
}