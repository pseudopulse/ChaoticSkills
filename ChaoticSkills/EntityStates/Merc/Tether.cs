using System;
using EntityStates.Merc.Weapon;

namespace ChaoticSkills.EntityStates.Merc {
    public class Tether : BaseState {
        private HurtBox[] tetherTargets;
        private GameObject tetherPrefab => Utils.Paths.GameObject.LaserEngiTurret.Load<GameObject>();
        private float range = 40;
        private Light lightInstance;
        private float damageCoeff = 0.15f;
        private float delay = 0.1f;
        private float stopwatch = 0f;
        List<KeyValuePair<HurtBox, GameObject>> tethers = new();

        public override void OnEnter()
        {
            base.OnEnter();
            CreateLightInstance();
            delay = delay / base.attackSpeedStat;
            AkSoundEngine.PostEvent(Events.Play_engi_R_walkingTurret_laser_start, base.gameObject);
            base.characterBody.AddBuff(RoR2Content.Buffs.Slow50);
        }

        public override void OnExit()
        {
            base.OnExit();
            DestroyLightInstance();
            tetherTargets = Array.Empty<HurtBox>();
            CleanUpInstances();
            AkSoundEngine.PostEvent(Events.Play_engi_R_walkingTurret_laser_end, base.gameObject);
            base.characterBody.RemoveBuff(RoR2Content.Buffs.Slow50);
        }

        public override void FixedUpdate() {
            base.FixedUpdate();
            if (true) {
                stopwatch += Time.fixedDeltaTime;

                if (stopwatch >= delay) {
                    stopwatch = 0f;

                    BullseyeSearch search = new();
                    search.maxDistanceFilter = range;
                    search.teamMaskFilter = TeamMask.GetEnemyTeams(base.GetTeam());
                    search.maxAngleFilter = 75;
                    search.searchDirection = base.GetAimRay().direction;
                    search.searchOrigin = base.GetAimRay().origin;
                    search.sortMode = BullseyeSearch.SortMode.Distance;
                    search.filterByLoS = true;
                    search.filterByDistinctEntity = true;
                    search.RefreshCandidates();
                    tetherTargets = search.GetResults().ToArray();
                

                    foreach (HurtBox box in tetherTargets) {
                        if (!HasInstance(box)) {
                            CreateTetherInstance(box);
                        }

                        if (NetworkServer.active) {
                            DamageInfo info = new();
                            info.attacker = base.gameObject;
                            info.force = (base.transform.position - box.transform.position).normalized * 500;
                            info.crit = base.RollCrit();
                            info.procCoefficient = 0.2f;
                            info.procChainMask = new();
                            info.damageType = DamageType.SlowOnHit;
                            info.position = box.transform.position;
                            info.damage = base.damageStat * damageCoeff;

                            box.healthComponent.TakeDamageForce(info, true);
                            info.force = Vector3.zero;
                            box.healthComponent.TakeDamage(info);
                        }
                    }

                    CleanUpInstances();
                    UpdateInstances();
                }

                if (!inputBank.skill2.down) {
                    outer.SetNextStateToMain();
                }
            }
        }

        // light
        private void CreateLightInstance() {
            lightInstance = base.gameObject.AddComponent<Light>();
            lightInstance.color = Color.blue;
            lightInstance.range = 10;
            lightInstance.intensity = 5;
        }

        private void DestroyLightInstance() {
            GameObject.DestroyImmediate(lightInstance);
        }
        // tether 
        private void CreateTetherInstance(HurtBox hurtBox) {
            GameObject instance = GameObject.Instantiate(tetherPrefab, base.transform);
            tethers.Add(new(hurtBox, instance));
        }

        private bool HasInstance(HurtBox box) {
            foreach (KeyValuePair<HurtBox, GameObject> pair in tethers) {
                if (pair.Key == box) {
                    return true;
                }
            }
            return false;
        }

        private void CleanUpInstances() {
            List<KeyValuePair<HurtBox, GameObject>> pairs = new();

            foreach (KeyValuePair<HurtBox, GameObject> pair in tethers) {
                if (!tetherTargets.Contains(pair.Key)) {
                    pairs.Add(pair);
                }
                else if (Vector3.Distance(pair.Key.transform.position, base.transform.position) < 5) {
                    pairs.Add(pair);
                }
            }

            for (int i = 0; i < pairs.Count; i++) {
                Destroy(pairs[i].Value);
                tethers.Remove(pairs[i]);
            }
        }

        private void UpdateInstances() {
            foreach (KeyValuePair<HurtBox, GameObject> pair in tethers) {
                Transform laserEndPoint = pair.Value.GetComponent<ChildLocator>().FindChild("LaserEnd");
                laserEndPoint.position = pair.Key.transform.position;
                laserEndPoint.parent = pair.Key.transform;
            }
        }
    }
}