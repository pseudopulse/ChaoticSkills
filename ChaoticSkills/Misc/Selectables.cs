/*using System;
using RoR2.UI;

namespace ChaoticSkills.Misc {
    public static class Selectables {
        public const string Prefix = "MiscSelectable/";
        public static void Hooks() {
            // lmao the code is in RealPassives.cs on line 70 because i dont know how to write IL hooks
            UserProfile.onLoadoutChangedGlobal += ForceRebuid;
        }

        private static void ForceRebuid(UserProfile profile) {
            List<LoadoutPanelController> controllers = GameObject.FindObjectsOfType<LoadoutPanelController>().ToList();
            foreach (LoadoutPanelController controller in controllers) {
                controller.Rebuild();
            }

            List<CharacterSelectController> cssList = GameObject.FindObjectsOfType<CharacterSelectController>().ToList();
            foreach (CharacterSelectController css in cssList) {
                css.RebuildLocal();
            }
        }
    }
}*/