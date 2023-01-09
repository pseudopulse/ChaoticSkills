using System;
using EntityStates.Captain.Weapon;
using RoR2.UI;
namespace ChaoticSkills.EntityStates.Captain {
    public class CallBackup : CallAirstrikeBase {
        public override void OnEnter()
        {
            base.airstrikeRadius = 3;
            base.endpointVisualizerPrefab = Utils.Paths.GameObject.TreebotMortarAreaIndicator.Load<GameObject>();
            base.maxDistance = Mathf.Infinity;
            base.rayRadius = 0.2f;
            base.damageCoefficient = 0f;
            base.projectilePrefab = Content.Captain.Backup.projectilePrefab;
            base.minimumDuration = 0.2f;
            base.OnEnter();
        }

        public override void FireProjectile()
        {
            List<GameObject> masters = new() {
                Utils.Paths.GameObject.MercMonsterMaster.Load<GameObject>(),
                Utils.Paths.GameObject.ToolbotMonsterMaster.Load<GameObject>(),
                Utils.Paths.GameObject.TreebotMonsterMaster.Load<GameObject>(),
                Utils.Paths.GameObject.LoaderMonsterMaster.Load<GameObject>(),
                Utils.Paths.GameObject.CommandoMonsterMaster.Load<GameObject>()
            };

            MasterSummon summon = new();
            summon.teamIndexOverride = TeamIndex.Player;
            summon.ignoreTeamMemberLimit = true;
            summon.inventoryToCopy = base.characterBody.inventory;
            summon.summonerBodyObject = base.gameObject;
            summon.useAmbientLevel = false;
            summon.position = base.currentTrajectoryInfo.hitPoint;
            summon.rotation = Quaternion.identity;
            summon.masterPrefab = masters.GetRandom();

            if (NetworkServer.active) {
                CharacterMaster master = summon.Perform();
                if (master) {
                    master.inventory.GiveItem(RoR2Content.Items.HealthDecay, 30);
                }
            }
        }
    }

    public class AimBackup : BaseState {
        public static SkillDef primarySkillDef => Content.Captain.Backup.callDropPodDef;

        public static GameObject crosshairOverridePrefab => Utils.Paths.GameObject.CaptainAirstrikeCrosshair.Load<GameObject>();

        public static string enterSoundString => "Play_captain_shift_start";

        public static string exitSoundString => "Play_captain_shift_end";

        public static GameObject effectMuzzlePrefab => Utils.Paths.GameObject.CaptainRadioEffect.Load<GameObject>();

        public static string effectMuzzleString => "MuzzleHandRadio";

        public static float baseExitDuration => 0.3f;

        private GenericSkill primarySkillSlot;

        private GameObject effectMuzzleInstance;

        private Animator modelAnimator;

        private float timerSinceComplete;

        private bool beginExit;

        private CrosshairUtils.OverrideRequest crosshairOverrideRequest;

        private float exitDuration => baseExitDuration / attackSpeedStat;

        public override void OnEnter()
        {
            base.OnEnter();
            primarySkillSlot = (base.skillLocator ? base.skillLocator.primary : null);
            if ((bool)primarySkillSlot)
            {
                primarySkillSlot.SetSkillOverride(this, primarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
            }
            modelAnimator = GetModelAnimator();
            if ((bool)modelAnimator)
            {
                modelAnimator.SetBool("PrepAirstrike", value: true);
            }
            PlayCrossfade("Gesture, Override", "PrepAirstrike", 0.1f);
            PlayCrossfade("Gesture, Additive", "PrepAirstrike", 0.1f);
            Transform transform = FindModelChild(effectMuzzleString);
            if ((bool)transform)
            {
                effectMuzzleInstance = GameObject.Instantiate(effectMuzzlePrefab, transform);
            }
            if ((bool)crosshairOverridePrefab)
            {
                crosshairOverrideRequest = CrosshairUtils.RequestOverrideForBody(base.characterBody, crosshairOverridePrefab, CrosshairUtils.OverridePriority.Skill);
            }
            Util.PlaySound(enterSoundString, base.gameObject);
            Util.PlaySound("Play_captain_shift_active_loop", base.gameObject);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if ((bool)base.characterDirection)
            {
                base.characterDirection.moveVector = GetAimRay().direction;
            }
            if (!primarySkillSlot || primarySkillSlot.stock == 0)
            {
                beginExit = true;
            }
            if (beginExit)
            {
                timerSinceComplete += Time.fixedDeltaTime;
                if (timerSinceComplete > exitDuration)
                {
                    outer.SetNextStateToMain();
                }
            }
        }

        public override void OnExit()
        {
            if ((bool)primarySkillSlot)
            {
                primarySkillSlot.UnsetSkillOverride(this, primarySkillDef, GenericSkill.SkillOverridePriority.Contextual);
            }
            Util.PlaySound(exitSoundString, base.gameObject);
            Util.PlaySound("Stop_captain_shift_active_loop", base.gameObject);
            if ((bool)effectMuzzleInstance)
            {
                EntityState.Destroy(effectMuzzleInstance);
            }
            if ((bool)modelAnimator)
            {
                modelAnimator.SetBool("PrepAirstrike", value: false);
            }
            crosshairOverrideRequest?.Dispose();
            base.OnExit();
        }
    }
}