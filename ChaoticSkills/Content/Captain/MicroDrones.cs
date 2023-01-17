/*using System;
using ChaoticSkills.Misc;

namespace ChaoticSkills.Content.Captain {
    public class MicroDrones : SkillBase<MicroDrones> {
        public override SerializableEntityStateType ActivationState => ContentAddition.AddEntityState<EntityStates.Captain.MicroDrone>(out bool _);
        public override float Cooldown => 10f;
        public override bool DelayCooldown => false;
        public override string Description => "<style=cIsUtility>Command</style> a selectable <style=cIsUtility>micro-drone</style> to assist you.";
        public override bool Agile => true;
        public override bool IsCombat => true;
        public override string LangToken => "Micro";
        public override int StockToConsume => 0;
        public override int MaxStock => 1;
        public override UnlockableDef Unlock => null;
        public override string Machine => "Weapon";
        public override Sprite SkillIcon => null;
        public override SkillSlot Slot => SkillSlot.Secondary;
        public override string Survivor => Utils.Paths.GameObject.CaptainBody;
        public override bool MustKeyPress => true;
        public override string Name => "Micro-Drones";
        public override bool MiscSelectable => true;
        public override string MiscSelectableName => "MicroDrone";
        public static string MiscSelectableNameStatic => "MicroDrone";

        public override void PostCreation()
        {
            base.PostCreation();
            On.RoR2.CharacterBody.Start += (orig, self) => {
                orig(self);
                if (self.skillLocator.secondary && self.skillLocator.secondary.skillDef == SkillDef) {
                    self.AddComponent<CaptainDroneTargeter>();
                }
            };

            On.RoR2.Skills.SkillDef.IsReady += (orig, self, slot) => {
                if (slot.skillName != null && slot.skillName == Selectables.Prefix + MiscSelectableName) {
                    if (slot.GetComponent<CaptainDroneTargeter>()) {
                        return self.HasRequiredStockAndDelay(slot) && slot.GetComponent<CaptainDroneTargeter>().target != null;
                    } else {
                        return orig(self, slot);
                    }
                } else {
                    return orig(self, slot);
                }
            };
        }
    }

    public class CaptainDroneTargeter : MonoBehaviour {
        public CharacterBody cb => GetComponent<CharacterBody>();
        public SkillLocator sl => cb.skillLocator;
        private BullseyeSearch search;
        private CaptainDrone drone;
        private float stopwatch = 0f;
        private float delay = 0.5f;
        private GameObject dronePrefab;
        public enum TargetType {
            Interactable,
            Enemy,
            Ally
        }

        public TargetType type;
        public Transform target;
        private bool ready => sl.secondary.IsReady();
        // indicator
        private Indicator indicator;
        private void Start() {
            indicator = new(base.gameObject, Utils.Paths.GameObject.LightningIndicator.Load<GameObject>());

            GenericSkill skill = base.gameObject.GetComponents<GenericSkill>().First(x => x.skillName != null && x.skillName == Selectables.Prefix + MicroDrones.MiscSelectableNameStatic);
            Debug.Log(skill.skillDef.skillNameToken);
            if (skill.skillDef == Drones.OffenseDrone.Instance.SkillDef) {
                dronePrefab = Drones.OffenseDrone.OffenseDronePrefab;
                type = TargetType.Enemy;
            }
            else if (skill.skillDef == Drones.SupportDrone.Instance.SkillDef) {
                dronePrefab = Drones.SupportDrone.SupportDronePrefab;
                type = TargetType.Ally;
            }

            GameObject obj = GameObject.Instantiate(dronePrefab);
            drone = obj.GetComponent<CaptainDrone>();
            drone.owner = base.gameObject;
        }
        private void OnDestroy() {
            indicator.active = false;
        }

        private void FixedUpdate() {
            if (drone) {
                if (sl.secondary.skillDef.HasRequiredStockAndDelay(sl.secondary)) {
                    stopwatch += Time.fixedDeltaTime;
                    if (stopwatch >= delay) {
                        search = new();
                        search.searchOrigin = cb.corePosition;
                        search.maxDistanceFilter = 90;
                        search.maxAngleFilter = 15;
                        search.searchDirection = cb.inputBank.aimDirection;
                        search.sortMode = BullseyeSearch.SortMode.Distance;
                        switch (type) {
                            case TargetType.Enemy:
                                search.teamMaskFilter = TeamMask.GetUnprotectedTeams(cb.teamComponent.teamIndex);
                                break;
                            case TargetType.Ally:
                                search.teamMaskFilter = TeamMask.none;
                                search.teamMaskFilter.AddTeam(cb.teamComponent.teamIndex);
                                break;
                            default:
                                break;
                        }
                        search.RefreshCandidates();
                        search.FilterOutGameObject(base.gameObject);
                        Transform tmp = null;
                        foreach (HurtBox box in search.GetResults()) {
                            tmp = box.transform;
                            indicator.targetTransform = target;
                            indicator.active = true;
                            break;
                        }
                        target = tmp;
                    }
                }
            }

            if (indicator != null && !target) {
                indicator.active = false;
            }

            if (!target) {
                
            }
        }

        public void Fire() {
            if (drone && target) {
                drone.Fire(target);
            }
            else {
                Recall();
            }
        }

        public void Recall() {
            drone.Recall();
        }
    }

    public abstract class CaptainDrone : MonoBehaviour {
        public Transform target;
        public GameObject owner;
        public Transform ownerTransform => owner.transform;
        private CaptainDroneTargeter targeter => owner.GetComponent<CaptainDroneTargeter>();
        public bool chasing = false;
        public float minDistance = 5f;
        // orbit stuff
        public Transform orbitTarget;
        private float speed = 360f / 9f;
        private float initialTime;
        private Vector3 initialRadial;
        // methods
        public abstract void ReachedTarget();
        public virtual void Fire(Transform _target) {
            chasing = true;
            target = _target;
        }
        public virtual void Start() {
            initialTime = Run.instance.GetRunStopwatch();
        }

        public void ChangeOrbitTarget(Transform _target) {
            initialRadial = Quaternion.AngleAxis(UnityEngine.Random.Range(0, 360), Vector3.up) * _target.forward;
            orbitTarget = _target;
        }

        public virtual void FixedUpdate() {
            if (chasing && target && target != orbitTarget) {
                Vector3 pos = Vector3.Lerp(base.transform.position, target.position, 15 * Time.fixedDeltaTime);
                base.transform.position = pos;

                if (Vector3.Distance(base.transform.position, target.position) < minDistance) {
                    ReachedTarget();
                }
            }
            else if (orbitTarget) {
                float angle = (Run.instance.GetRunStopwatch() - initialTime) * speed;
                Vector3 pos = orbitTarget.position + new Vector3(0, 2, 0) + Quaternion.AngleAxis(angle, Vector3.up) * initialRadial * 4;
                base.transform.position = Vector3.Lerp(base.transform.position, pos, 12 * Time.fixedDeltaTime);
                base.transform.Rotate(new(45 * Time.fixedDeltaTime, 46 * Time.fixedDeltaTime, 46 * Time.fixedDeltaTime));
            }

            if (orbitTarget == null && owner) {
                ChangeOrbitTarget(owner.transform);
            }

            if ((!target || !target.gameObject.activeSelf) && !orbitTarget) {
                Recall();
            }
        }

        public void Recall() {
            target = null;
            orbitTarget = null;
            chasing = false;
            ChangeOrbitTarget(owner.transform);
        }
    }
}*/