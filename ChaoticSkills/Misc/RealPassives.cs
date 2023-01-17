using System;
using RoR2.UI;
using MonoMod.RuntimeDetour;
using System.Reflection;

namespace ChaoticSkills.Misc {
    public static class RealPassives {
        public delegate void orig_Build(RoR2.UI.CharacterSelectController self, Loadout loadout, in CharacterSelectController.BodyInfo info, List<CharacterSelectController.StripDisplayData> dest);
        public delegate void orig_Rebuild(LoadoutPanelController self);
        private static BindingFlags BuildFlags = BindingFlags.NonPublic | BindingFlags.Instance;
        private static BindingFlags BuildHookFlags = BindingFlags.NonPublic | BindingFlags.Static;
        private static BindingFlags RebuildFlags = BindingFlags.NonPublic | BindingFlags.Instance;
        private static BindingFlags RebuildHookFlags = BindingFlags.NonPublic | BindingFlags.Static;
        public static void Hook() {
            Hook buildHook = new Hook(
                typeof(CharacterSelectController).GetMethod(nameof(CharacterSelectController.BuildSkillStripDisplayData), BuildFlags),
                typeof(RealPassives).GetMethod(nameof(HopooGames), BuildHookFlags)
            );

            Hook RebuildHook = new Hook(
                typeof(LoadoutPanelController).GetMethod(nameof(LoadoutPanelController.Rebuild), RebuildFlags),
                typeof(RealPassives).GetMethod(nameof(HopooGamesTheSequel), RebuildHookFlags)
            );
        }
        
        private static void HopooGames(orig_Build orig, CharacterSelectController self, Loadout loadout, in CharacterSelectController.BodyInfo bodyInfo, List<CharacterSelectController.StripDisplayData> data) {
            if (!bodyInfo.bodyPrefab || !bodyInfo.bodyPrefabBodyComponent) {
                orig(self, loadout, bodyInfo, data);
                return;
            }
            GenericSkill[] skills = bodyInfo.skillSlots;
            int i = 0;
            /*List<GenericSkill> skills2 = skills.Where(x => x.skillName != null && x.skillName.StartsWith(Selectables.Prefix)).ToList();
            foreach (GenericSkill skill in skills2) {
                List<GenericSkill> guh = skills.ToList();
                guh.Remove(skill);
                skills = guh.ToArray();
            }*/

            foreach (GenericSkill skill in skills) {
                if (skill.skillName != null && skill.skillName.ToLower().Contains("passive") && skill.hideInCharacterSelect) {
                    SkillDef def = skill.skillFamily.variants[loadout.bodyLoadoutManager.GetSkillVariant(bodyInfo.bodyIndex, i)].skillDef;
                    CharacterSelectController.StripDisplayData display = new CharacterSelectController.StripDisplayData {
                        enabled = true,
                        primaryColor = bodyInfo.bodyColor,
                        icon = def.icon,
                        actionName = "",
                        descriptionString = Language.GetString(def.skillDescriptionToken),
                        titleString = Language.GetString(def.skillNameToken),
                        keywordString = ""
                    };

                    data.Add(display);
                }
                i++;
            }

            orig(self, loadout, bodyInfo, data);
        }

        private static void HopooGamesTheSequel(orig_Rebuild orig, LoadoutPanelController self) {
            try {
                self.DestroyRows();
                CharacterBody prefab = BodyCatalog.GetBodyPrefabBodyComponent(self.currentDisplayData.bodyIndex);
                if (prefab) {
                    GenericSkill[] skills = prefab.GetComponents<GenericSkill>();

                    for (int i = 0; i < skills.Length; i++) {
                        /*if (skills[i].skillName != null && skills[i].skillName.StartsWith(Selectables.Prefix)) {
                            skills[i].hideInCharacterSelect = true;
                        }*/
                        if (skills[i].skillName != null && skills[i].skillName.ToLower().Contains("passive") && skills[i].hideInCharacterSelect) {
                            self.rows.Add(LoadoutPanelController.Row.FromSkillSlot(self, self.currentDisplayData.bodyIndex, i, skills[i]));
                        }
                        else {
                            continue;
                        }
                    }

                    // misc selectables
                    /*for (int i = 0; i < skills.Length; i++) {
                        if (skills[i].skillName == null) continue;
                        Loadout loadout = self.currentDisplayData.userProfile.loadout;
                        Loadout.BodyLoadoutManager manager = loadout.bodyLoadoutManager;
                        uint variant = manager.GetSkillVariant(self.currentDisplayData.bodyIndex, i);
                        // Debug.Log("currently on: " + skills[i].skillName);
                        SkillDef def = skills[i].skillFamily.variants[variant].skillDef;
                        if (def && def.skillName != null && def.skillName.StartsWith(Selectables.Prefix)) {
                            GenericSkill misc = null;
                            try {
                                misc = skills.First(x => x.skillName != null && x.skillName == def.skillName);
                            } catch {
                                Main.ModLogger.LogError("[SELECTABLES]: SkillDef used the MiscSelectable/ prefix, however no matching GenericSkill could be found");
                            }

                            if (misc) {
                                List<GenericSkill> skillstmp = skills.ToList();
                                skillstmp.Remove(misc);
                                skillstmp.Insert(i + 1, misc);
                                skills = skillstmp.ToArray();
                                misc.hideFlags -= HideFlags.DontSave;
                            }
                        }
                    };*/
                    

                    for (int i = 0; i < skills.Length; i++) {
                        bool skillIsMisc = skills[i].hideFlags.HasFlag(HideFlags.DontSave);
                        if (skills[i].skillName != null && skills[i].skillName.ToLower().Contains("passive") && skills[i].hideInCharacterSelect) {
                            continue;
                        }
                        else if (!skillIsMisc){
                            /*if (skills[i].skillName != null && skills[i].skillName.StartsWith(Selectables.Prefix)) {
                                skills[i].hideFlags |= HideFlags.DontSave;
                            }*/
                            self.rows.Add(LoadoutPanelController.Row.FromSkillSlot(self, self.currentDisplayData.bodyIndex, i, skills[i]));
                        }
                    }

                    self.rows.Add(LoadoutPanelController.Row.FromSkin(self, self.currentDisplayData.bodyIndex));
                }
            } catch {
                orig(self);
            }
        }
    }
}