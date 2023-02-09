using System;
using EntityStates.VoidSurvivor.CorruptMode;

namespace ChaoticSkills.Misc {
    public class VoidSurvivorAlts {
        public struct VoidSurvivorAlt {
            public SkillDef uncorrupted;
            public SkillDef corrupted;
        }

        private static List<VoidSurvivorAlt> alts = new();

        public static void RegisterVoidSurvivorAlt(SkillDef uncorrupted, SkillDef corrupted) {
            alts.Add(new VoidSurvivorAlt {
                uncorrupted = uncorrupted,
                corrupted = corrupted
            });
        }

        public static void Hooks() {
            On.EntityStates.VoidSurvivor.CorruptMode.CorruptMode.OnEnter += ReplaceSkills;
        }

        private static void ReplaceSkills(On.EntityStates.VoidSurvivor.CorruptMode.CorruptMode.orig_OnEnter orig, CorruptMode self) {
            SkillLocator sl = self.skillLocator;
            VoidSurvivorAlt guh1 = alts.FirstOrDefault(x => x.uncorrupted == sl.primary.skillDef);
            bool hasPrimary = guh1.uncorrupted != null;

            VoidSurvivorAlt guh2 = alts.FirstOrDefault(x => x.uncorrupted == sl.secondary.skillDef);
            bool hasSecondary = guh2.uncorrupted != null;

            VoidSurvivorAlt guh3 = alts.FirstOrDefault(x => x.uncorrupted == sl.utility.skillDef);
            bool hasUtility = guh3.uncorrupted != null;

            VoidSurvivorAlt guh4 = alts.FirstOrDefault(x => x.uncorrupted == sl.special.skillDef);
            bool hasSpecial = guh4.uncorrupted != null;

            if (hasPrimary) self.primaryOverrideSkillDef = guh1.corrupted;
            if (hasSecondary) self.secondaryOverrideSkillDef = guh2.corrupted;
            if (hasUtility) self.utilityOverrideSkillDef = guh3.corrupted;
            if (hasSpecial) self.specialOverrideSkillDef = guh4.corrupted;

            orig(self);
        }

    }
}