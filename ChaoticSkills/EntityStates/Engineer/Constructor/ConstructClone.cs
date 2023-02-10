using System;

namespace ChaoticSkills.EntityStates.Engineer {
    public class ConstructClone : BaseState {
        private Transform ConstructPoint;
        private float constructTimer = 10f;
        private BodyIndex targetIndex;
        private GameObject constructionInstance;
        private CharacterMaster constructionMaster;
        private CharacterBody constructionBody;
        private List<string> fallbackSpawns = new() {
            Utils.Paths.GameObject.LemurianMaster, Utils.Paths.GameObject.BeetleMaster
        };
        public override void OnEnter()
        {
            base.OnEnter();
            ConstructPoint = GetModelChildLocator().FindChild("SpawnTarget");
            if (!base.characterBody.master.GetComponent<BaseAI>()) {
                //Debug.Log("not an AI, returning");
                outer.SetNextStateToMain();
                return;
            }

            if (SceneManager.GetActiveScene().name == "moon2" || SceneManager.GetActiveScene().name == "moon") {
                fallbackSpawns.Clear();
                fallbackSpawns.Add(Utils.Paths.GameObject.LunarWispMaster);
                fallbackSpawns.Add(Utils.Paths.GameObject.LunarGolemMaster);
                fallbackSpawns.Add(Utils.Paths.GameObject.LunarExploderMaster);
            }

            BaseAI ai = base.characterBody.master.GetComponent<BaseAI>();
            if (ai.currentEnemy.characterBody && !ai.currentEnemy.characterBody.isBoss) {
                targetIndex = ai.currentEnemy.characterBody.bodyIndex;
            }
            else if (ai.leader.characterBody) {
                targetIndex = ai.leader.characterBody.bodyIndex;
            }
            else {
                // Debug.Log("dont have a current target, returning");
                outer.SetNextStateToMain();
                return;
            }

            GameObject masterPrefab = MasterCatalog.FindMasterPrefab(BodyCatalog.GetBodyName(targetIndex).Replace("Body", "Master"));
            // Debug.Log(BodyCatalog.GetBodyName(targetIndex));
            if (!masterPrefab) {
                // masterPrefab = MasterCatalog.FindMasterPrefab(BodyCatalog.GetBodyName(targetIndex).Replace("Body", "MonsterMaster"));
            }
            if (!masterPrefab) {
                masterPrefab = fallbackSpawns.GetRandom().Load<GameObject>();
            }
            if (NetworkServer.active && masterPrefab && IsSummonValid()) {
                GetModelChildLocator().FindChild("SpawnVFX").gameObject.SetActive(true);
                MasterSummon summon = new();
                summon.masterPrefab = masterPrefab;
                summon.inventoryToCopy = base.characterBody.inventory;
                summon.summonerBodyObject = base.gameObject;
                summon.position = ConstructPoint.transform.position;
                summon.rotation = Quaternion.identity;
                summon.useAmbientLevel = true;

                constructionInstance = summon.Perform().gameObject;
                if (constructionInstance) {
                    constructionMaster = constructionInstance.GetComponent<CharacterMaster>();
                    constructionBody = constructionMaster.GetBody();
                    constructionBody.AddBuff(Content.Engineer.Constructor.BeingConstructed);
                    constructionBody.visionDistance = 0f;

                    foreach (EntityStateMachine esm in constructionBody.GetComponents<EntityStateMachine>()) {
                        if (esm.customName == "Body") {
                            esm.SetState(EntityStateCatalog.InstantiateState(esm.mainStateType));
                        }
                    }
                }
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.fixedAge < constructTimer) {
                if (constructionBody) {
                    if (constructionBody.characterMotor) {
                        constructionBody.characterMotor.enabled = false;
                    }
                    if (constructionBody.rigidbody) {
                        constructionBody.rigidbody.MovePosition(ConstructPoint.position + new Vector3(0, Mathf.Abs(constructionBody.rigidbody.position.y - constructionBody.footPosition.y), 0));

                        Quaternion rotation = constructionBody.rigidbody.rotation;
                        rotation.z += 35 * Time.fixedDeltaTime;
                        rotation = rotation.normalized;
                        constructionBody.rigidbody.MoveRotation(rotation);
                    }
                    if (constructionMaster) {
                        constructionMaster.aiComponents[0].skillDriverUpdateTimer = 1f;
                    }
                }
            }
            else {
                if (constructionBody) {
                    constructionBody.visionDistance = constructionBody.baseVisionDistance;
                    if (constructionBody.characterMotor) {
                        constructionBody.characterMotor.enabled = true;
                    }
                }
                //Debug.Log("construction timer finished, returning");
                outer.SetNextStateToMain();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Death;
        }

        public override void OnExit()
        {
            base.OnExit();
            GetModelChildLocator().FindChild("SpawnVFX").gameObject.SetActive(false);
            if (constructionBody) {
                constructionBody.visionDistance = constructionBody.baseVisionDistance;
                if (constructionBody.characterMotor) {
                    constructionBody.characterMotor.enabled = true;
                }
            }
        }

        public bool IsSummonValid() {
            int t = 0;
            foreach (TeamComponent com in TeamComponent.GetTeamMembers(GetTeam())) {
                if (com.body.HasBuff(Content.Engineer.Constructor.BeingConstructed)) {
                    t++;
                }
            }
            return t < 6;
        }
    }
}