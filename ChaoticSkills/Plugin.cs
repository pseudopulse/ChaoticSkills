using BepInEx;
using R2API;
using R2API.Utils;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Reflection;
using BepInEx.Configuration;

namespace ChaoticSkills {
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    public class Main : BaseUnityPlugin {
        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "pseudopulse";
        public const string PluginName = "ChaoticSkills";
        public const string PluginVersion = "1.0.0";
        public static BepInEx.Logging.ManualLogSource ModLogger;
        public static AssetBundle Assets;
        public static ConfigFile config;

        public void Awake() {
            // logger
            ModLogger = Logger;

            // config
            config = Config;

            // assetbundle
            Assets = AssetBundle.LoadFromFile(Assembly.GetExecutingAssembly().Location.Replace("ChaoticSkills.dll", "chaoticbundle"));

            // skills
            IEnumerable<Type> skills = Assembly.GetExecutingAssembly().GetTypes().Where(x => !x.IsAbstract && x.IsSubclassOf(typeof(SkillBase)));
            foreach (Type skill in skills) {
                SkillBase skillBase = (SkillBase)Activator.CreateInstance(skill);
                skillBase.Init();
            }

            // hopoo why
            Misc.RealPassives.Hook();
        }
    }
}