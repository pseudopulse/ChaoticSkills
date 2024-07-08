using System;
using EntityStates.Captain.Weapon;
using RoR2.UI;
using RoR2.Navigation;

namespace ChaoticSkills.EntityStates.Captain {
    public class CallBackup : BaseState {
        public override void OnEnter()
        {
            base.OnEnter();
            if (NetworkServer.active) {
                List<GameObject> masters = new() {
                    Utils.Paths.GameObject.MercMonsterMaster.Load<GameObject>(),
                    Utils.Paths.GameObject.ToolbotMonsterMaster.Load<GameObject>(),
                    Utils.Paths.GameObject.TreebotMonsterMaster.Load<GameObject>(),
                    Utils.Paths.GameObject.LoaderMonsterMaster.Load<GameObject>(),
                    Utils.Paths.GameObject.CommandoMonsterMaster.Load<GameObject>(),
                    Utils.Paths.GameObject.Bandit2MonsterMaster.Load<GameObject>()
                };

                MasterSummon summon = new();
                summon.teamIndexOverride = TeamIndex.Player;
                summon.ignoreTeamMemberLimit = true;
                summon.inventoryToCopy = base.characterBody.inventory;
                summon.summonerBodyObject = base.gameObject;
                summon.useAmbientLevel = false;
                summon.position = GetPositionNearSelf();
                summon.rotation = Quaternion.identity;
                summon.masterPrefab = masters.GetRandom();

                summon.Perform();
            }
            outer.SetNextStateToMain();
        }

        public Vector3 GetPositionNearSelf() {
            if (SceneInfo.instance && SceneInfo.instance.groundNodes) {
                try {
                    NodeGraph graph = SceneInfo.instance.groundNodes;
                    NodeGraph.Node node = graph.nodes.Where(x => Vector3.Distance(x.position, characterBody.corePosition) < 30).ToList().GetRandom();
                    return node.position;
                }
                catch {
                    return characterBody.corePosition + (Vector3.up * 3);
                }
            }
            else {
                return characterBody.corePosition + (Vector3.up * 3);
            }
        }
    }
}
