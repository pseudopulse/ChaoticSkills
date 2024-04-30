using System;

namespace ChaoticSkills.Content.MULT {
    public class RecipeManager {
        public class Recipe {
            public ItemDef[] Items;
            public ItemDef Result;
            public int ResultCount = 1;
            public bool shapeless = false;
        }

        public static List<Recipe> recipes = new();

        public static void CollectRecipes() {
            recipes.Add(new Recipe () {
                Items = new ItemDef[9] {
                    Items.Clover, Items.Clover, Items.Clover,
                    null, Items.Syringe, null,
                    null, Items.Syringe, null
                },
                Result = Items.PersonalShield,
                ResultCount = 13
            });
        }
        
        public static Recipe CheckRecipe(CraftingSlot[] slots) {
            int c = 0;
            foreach (Recipe recipe in recipes) {
                if (recipe.shapeless) {
                    for (int i = 0; i < recipe.Items.Length; i++) {
                        if (recipe.Items[i] == null) {
                            c++;
                            continue;
                        }

                        if (slots.Where(x => x.storedItem.index == recipe.Items[i].itemIndex).Count() != 0) {
                            c++;
                        }
                    }

                    if (c >= recipe.Items.Length) {
                        return recipe;
                    }

                    return null;
                }

                for (int i = 0; i < 9; i++) {
                    // Debug.Log(i + " _ " + recipe.Items[i]);

                    if (recipe.Items[i] == null) {
                        c++;
                        continue;
                    }

                    if (slots[i].storedItem != null && recipe.Items[i].itemIndex == slots[i].storedItem.index) {
                        c++;
                    }

                    if (c >= 8) {
                        return recipe;
                    }
                }
            }

            return null;
        }
    }

    public class Items {
        public static RoR2.ItemDef AdaptiveArmor => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/AdaptiveArmor/AdaptiveArmor.asset").WaitForCompletion();
            public static RoR2.ItemDef AlienHead => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/AlienHead/AlienHead.asset").WaitForCompletion();
            public static RoR2.ItemDef ArmorPlate => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/ArmorPlate/ArmorPlate.asset").WaitForCompletion();
            public static RoR2.ItemDef ArmorReductionOnHit => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/ArmorReductionOnHit/ArmorReductionOnHit.asset").WaitForCompletion();
            public static RoR2.ItemDef ArtifactKey => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/ArtifactKey/ArtifactKey.asset").WaitForCompletion();
            public static RoR2.ItemDef AttackSpeedOnCrit => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/AttackSpeedOnCrit/AttackSpeedOnCrit.asset").WaitForCompletion();
            public static RoR2.ItemDef AutoCastEquipment => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/AutoCastEquipment/AutoCastEquipment.asset").WaitForCompletion();
            public static RoR2.ItemDef Bandolier => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/Bandolier/Bandolier.asset").WaitForCompletion();
            public static RoR2.ItemDef BarrierOnKill => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/BarrierOnKill/BarrierOnKill.asset").WaitForCompletion();
            public static RoR2.ItemDef BarrierOnOverHeal => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/BarrierOnOverHeal/BarrierOnOverHeal.asset").WaitForCompletion();
            public static RoR2.ItemDef Bear => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/Bear/Bear.asset").WaitForCompletion();
            public static RoR2.ItemDef BeetleGland => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/BeetleGland/BeetleGland.asset").WaitForCompletion();
            public static RoR2.ItemDef Behemoth => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/Behemoth/Behemoth.asset").WaitForCompletion();
            public static RoR2.ItemDef BleedOnHit => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/BleedOnHit/BleedOnHit.asset").WaitForCompletion();
            public static RoR2.ItemDef BleedOnHitAndExplode => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/BleedOnHitAndExplode/BleedOnHitAndExplode.asset").WaitForCompletion();
            public static RoR2.ItemDef BonusGoldPackOnKill => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/BonusGoldPackOnKill/BonusGoldPackOnKill.asset").WaitForCompletion();
            public static RoR2.ItemDef BoostAttackSpeed => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/BoostAttackSpeed/BoostAttackSpeed.asset").WaitForCompletion();
            public static RoR2.ItemDef BoostDamage => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/BoostDamage/BoostDamage.asset").WaitForCompletion();
            public static RoR2.ItemDef BoostEquipmentRecharge => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/BoostEquipmentRecharge/BoostEquipmentRecharge.asset").WaitForCompletion();
            public static RoR2.ItemDef BoostHp => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/BoostHp/BoostHp.asset").WaitForCompletion();
            public static RoR2.ItemDef BossDamageBonus => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/BossDamageBonus/BossDamageBonus.asset").WaitForCompletion();
            public static RoR2.ItemDef BounceNearby => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/BounceNearby/BounceNearby.asset").WaitForCompletion();
            public static RoR2.ItemDef CaptainDefenseMatrix => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/CaptainDefenseMatrix/CaptainDefenseMatrix.asset").WaitForCompletion();
            public static RoR2.ItemDef ChainLightning => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/ChainLightning/ChainLightning.asset").WaitForCompletion();
            public static RoR2.ItemDef Clover => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/Clover/Clover.asset").WaitForCompletion();
            public static RoR2.ItemDef CrippleWardOnLevel => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/CrippleWardOnLevel/CrippleWardOnLevel.asset").WaitForCompletion();
            public static RoR2.ItemDef CritGlasses => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/CritGlasses/CritGlasses.asset").WaitForCompletion();
            public static RoR2.ItemDef Crowbar => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/Crowbar/Crowbar.asset").WaitForCompletion();
            public static RoR2.ItemDef CutHp => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/CutHp/CutHp.asset").WaitForCompletion();
            public static RoR2.ItemDef Dagger => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/Dagger/Dagger.asset").WaitForCompletion();
            public static RoR2.ItemDef DeathMark => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/DeathMark/DeathMark.asset").WaitForCompletion();
            public static RoR2.ItemDef DrizzlePlayerHelper => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/DrizzlePlayerHelper/DrizzlePlayerHelper.asset").WaitForCompletion();
            public static RoR2.ItemDef FireRing => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/ElementalRings/FireRing.asset").WaitForCompletion();
            public static RoR2.ItemDef IceRing => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/ElementalRings/IceRing.asset").WaitForCompletion();
            public static RoR2.ItemDef EnergizedOnEquipmentUse => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/EnergizedOnEquipmentUse/EnergizedOnEquipmentUse.asset").WaitForCompletion();
            public static RoR2.ItemDef EquipmentMagazine => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/EquipmentMagazine/EquipmentMagazine.asset").WaitForCompletion();
            public static RoR2.ItemDef ExecuteLowHealthElite => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/ExecuteLowHealthElite/ExecuteLowHealthElite.asset").WaitForCompletion();
            public static RoR2.ItemDef ExplodeOnDeath => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/ExplodeOnDeath/ExplodeOnDeath.asset").WaitForCompletion();
            public static RoR2.ItemDef ExtraLife => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/ExtraLife/ExtraLife.asset").WaitForCompletion();
            public static RoR2.ItemDef ExtraLifeConsumed => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/ExtraLife/ExtraLifeConsumed.asset").WaitForCompletion();
            public static RoR2.ItemDef FallBoots => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/FallBoots/FallBoots.asset").WaitForCompletion();
            public static RoR2.ItemDef Feather => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/Feather/Feather.asset").WaitForCompletion();
            public static RoR2.ItemDef FireballsOnHit => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/FireballsOnHit/FireballsOnHit.asset").WaitForCompletion();
            public static RoR2.ItemDef Firework => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/Firework/Firework.asset").WaitForCompletion();
            public static RoR2.ItemDef FlatHealth => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/FlatHealth/FlatHealth.asset").WaitForCompletion();
            public static RoR2.ItemDef FocusConvergence => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/FocusConvergence/FocusConvergence.asset").WaitForCompletion();
            public static RoR2.ItemDef Ghost => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/Ghost/Ghost.asset").WaitForCompletion();
            public static RoR2.ItemDef GhostOnKill => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/GhostOnKill/GhostOnKill.asset").WaitForCompletion();
            public static RoR2.ItemDef GoldOnHit => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/GoldOnHit/GoldOnHit.asset").WaitForCompletion();
            public static RoR2.ItemDef HeadHunter => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/HeadHunter/HeadHunter.asset").WaitForCompletion();
            public static RoR2.ItemDef HealOnCrit => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/HealOnCrit/HealOnCrit.asset").WaitForCompletion();
            public static RoR2.ItemDef HealthDecay => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/HealthDecay/HealthDecay.asset").WaitForCompletion();
            public static RoR2.ItemDef HealWhileSafe => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/HealWhileSafe/HealWhileSafe.asset").WaitForCompletion();
            public static RoR2.ItemDef Hoof => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/Hoof/Hoof.asset").WaitForCompletion();
            public static RoR2.ItemDef Icicle => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/Icicle/Icicle.asset").WaitForCompletion();
            public static RoR2.ItemDef IgniteOnKill => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/IgniteOnKill/IgniteOnKill.asset").WaitForCompletion();
            public static RoR2.ItemDef IncreaseHealing => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/IncreaseHealing/IncreaseHealing.asset").WaitForCompletion();
            public static RoR2.ItemDef Infusion => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/Infusion/Infusion.asset").WaitForCompletion();
            public static RoR2.ItemDef InvadingDoppelganger => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/InvadingDoppelganger/InvadingDoppelganger.asset").WaitForCompletion();
            public static RoR2.ItemDef JumpBoost => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/JumpBoost/JumpBoost.asset").WaitForCompletion();
            public static RoR2.ItemDef KillEliteFrenzy => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/KillEliteFrenzy/KillEliteFrenzy.asset").WaitForCompletion();
            public static RoR2.ItemDef Knurl => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/Knurl/Knurl.asset").WaitForCompletion();
            public static RoR2.ItemDef LaserTurbine => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/LaserTurbine/LaserTurbine.asset").WaitForCompletion();
            public static RoR2.ItemDef LevelBonus => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/LevelBonus/LevelBonus.asset").WaitForCompletion();
            public static RoR2.ItemDef LightningStrikeOnHit => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/LightningStrikeOnHit/LightningStrikeOnHit.asset").WaitForCompletion();
            public static RoR2.ItemDef LunarBadLuck => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/LunarBadLuck/LunarBadLuck.asset").WaitForCompletion();
            public static RoR2.ItemDef LunarDagger => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/LunarDagger/LunarDagger.asset").WaitForCompletion();
            public static RoR2.ItemDef LunarPrimaryReplacement => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/LunarSkillReplacements/LunarPrimaryReplacement.asset").WaitForCompletion();
            public static RoR2.ItemDef LunarSecondaryReplacement => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/LunarSkillReplacements/LunarSecondaryReplacement.asset").WaitForCompletion();
            public static RoR2.ItemDef LunarSpecialReplacement => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/LunarSkillReplacements/LunarSpecialReplacement.asset").WaitForCompletion();
            public static RoR2.ItemDef LunarUtilityReplacement => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/LunarSkillReplacements/LunarUtilityReplacement.asset").WaitForCompletion();
            public static RoR2.ItemDef LunarTrinket => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/LunarTrinket/LunarTrinket.asset").WaitForCompletion();
            public static RoR2.ItemDef Medkit => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/Medkit/Medkit.asset").WaitForCompletion();
            public static RoR2.ItemDef MinHealthPercentage => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/MinHealthPercentage/MinHealthPercentage.asset").WaitForCompletion();
            public static RoR2.ItemDef MinionLeash => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/MinionLeash/MinionLeash.asset").WaitForCompletion();
            public static RoR2.ItemDef Missile => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/Missile/Missile.asset").WaitForCompletion();
            public static RoR2.ItemDef MonsoonPlayerHelper => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/MonsoonPlayerHelper/MonsoonPlayerHelper.asset").WaitForCompletion();
            public static RoR2.ItemDef MonstersOnShrineUse => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/MonstersOnShrineUse/MonstersOnShrineUse.asset").WaitForCompletion();
            public static RoR2.ItemDef Mushroom => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/Mushroom/Mushroom.asset").WaitForCompletion();
            public static RoR2.ItemDef NearbyDamageBonus => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/NearbyDamageBonus/NearbyDamageBonus.asset").WaitForCompletion();
            public static RoR2.ItemDef NovaOnHeal => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/NovaOnHeal/NovaOnHeal.asset").WaitForCompletion();
            public static RoR2.ItemDef NovaOnLowHealth => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/NovaOnLowHealth/NovaOnLowHealth.asset").WaitForCompletion();
            public static RoR2.ItemDef ParentEgg => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/ParentEgg/ParentEgg.asset").WaitForCompletion();
            public static RoR2.ItemDef Pearl => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/Pearl/Pearl.asset").WaitForCompletion();
            public static RoR2.ItemDef PersonalShield => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/PersonalShield/PersonalShield.asset").WaitForCompletion();
            public static RoR2.ItemDef Phasing => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/Phasing/Phasing.asset").WaitForCompletion();
            public static RoR2.ItemDef Plant => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/Plant/Plant.asset").WaitForCompletion();
            public static RoR2.ItemDef RandomDamageZone => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/RandomDamageZone/RandomDamageZone.asset").WaitForCompletion();
            public static RoR2.ItemDef RepeatHeal => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/RepeatHeal/RepeatHeal.asset").WaitForCompletion();
            public static RoR2.ItemDef RoboBallBuddy => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/RoboBallBuddy/RoboBallBuddy.asset").WaitForCompletion();
            public static RoR2.ItemDef ScrapGreen => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/Scrap/ScrapGreen.asset").WaitForCompletion();
            public static RoR2.ItemDef ScrapRed => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/Scrap/ScrapRed.asset").WaitForCompletion();
            public static RoR2.ItemDef ScrapWhite => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/Scrap/ScrapWhite.asset").WaitForCompletion();
            public static RoR2.ItemDef ScrapYellow => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/Scrap/ScrapYellow.asset").WaitForCompletion();
            public static RoR2.ItemDef SecondarySkillMagazine => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/SecondarySkillMagazine/SecondarySkillMagazine.asset").WaitForCompletion();
            public static RoR2.ItemDef Seed => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/Seed/Seed.asset").WaitForCompletion();
            public static RoR2.ItemDef ShieldOnly => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/ShieldOnly/ShieldOnly.asset").WaitForCompletion();
            public static RoR2.ItemDef ShinyPearl => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/ShinyPearl/ShinyPearl.asset").WaitForCompletion();
            public static RoR2.ItemDef ShockNearby => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/ShockNearby/ShockNearby.asset").WaitForCompletion();
            public static RoR2.ItemDef SiphonOnLowHealth => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/SiphonOnLowHealth/SiphonOnLowHealth.asset").WaitForCompletion();
            public static RoR2.ItemDef SlowOnHit => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/SlowOnHit/SlowOnHit.asset").WaitForCompletion();
            public static RoR2.ItemDef SprintArmor => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/SprintArmor/SprintArmor.asset").WaitForCompletion();
            public static RoR2.ItemDef SprintBonus => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/SprintBonus/SprintBonus.asset").WaitForCompletion();
            public static RoR2.ItemDef SprintOutOfCombat => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/SprintOutOfCombat/SprintOutOfCombat.asset").WaitForCompletion();
            public static RoR2.ItemDef SprintWisp => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/SprintWisp/SprintWisp.asset").WaitForCompletion();
            public static RoR2.ItemDef Squid => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/Squid/Squid.asset").WaitForCompletion();
            public static RoR2.ItemDef StickyBomb => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/StickyBomb/StickyBomb.asset").WaitForCompletion();
            public static RoR2.ItemDef StunChanceOnHit => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/StunChanceOnHit/StunChanceOnHit.asset").WaitForCompletion();
            public static RoR2.ItemDef Syringe => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/Syringe/Syringe.asset").WaitForCompletion();
            public static RoR2.ItemDef Talisman => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/Talisman/Talisman.asset").WaitForCompletion();
            public static RoR2.ItemDef TeamSizeDamageBonus => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/TeamSizeDamageBonus/TeamSizeDamageBonus.asset").WaitForCompletion();
            public static RoR2.ItemDef TeleportWhenOob => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/TeleportWhenOob/TeleportWhenOob.asset").WaitForCompletion();
            public static RoR2.ItemDef Thorns => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/Thorns/Thorns.asset").WaitForCompletion();
            public static RoR2.ItemDef TitanGoldDuringTP => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/TitanGoldDuringTP/TitanGoldDuringTP.asset").WaitForCompletion();
            public static RoR2.ItemDef TonicAffliction => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/TonicAffliction/TonicAffliction.asset").WaitForCompletion();
            public static RoR2.ItemDef Tooth => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/Tooth/Tooth.asset").WaitForCompletion();
            public static RoR2.ItemDef TPHealingNova => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/TPHealingNova/TPHealingNova.asset").WaitForCompletion();
            public static RoR2.ItemDef TreasureCache => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/TreasureCache/TreasureCache.asset").WaitForCompletion();
            public static RoR2.ItemDef UseAmbientLevel => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/UseAmbientLevel/UseAmbientLevel.asset").WaitForCompletion();
            public static RoR2.ItemDef UtilitySkillMagazine => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/UtilitySkillMagazine/UtilitySkillMagazine.asset").WaitForCompletion();
            public static RoR2.ItemDef WarCryOnMultiKill => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/WarCryOnMultiKill/WarCryOnMultiKill.asset").WaitForCompletion();
            public static RoR2.ItemDef WardOnLevel => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Base/WardOnLevel/WardOnLevel.asset").WaitForCompletion();
            public static RoR2.ItemDef VoidmanPassiveItem => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/DLC1/VoidSurvivor/VoidmanPassiveItem.asset").WaitForCompletion();
            public static RoR2.ItemDef GummyCloneIdentifier => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/DLC1/GummyClone/GummyCloneIdentifier.asset").WaitForCompletion();
            public static RoR2.ItemDef AttackSpeedAndMoveSpeed => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/DLC1/AttackSpeedAndMoveSpeed/AttackSpeedAndMoveSpeed.asset").WaitForCompletion();
            public static RoR2.ItemDef BearVoid => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/DLC1/BearVoid/BearVoid.asset").WaitForCompletion();
            public static RoR2.ItemDef BleedOnHitVoid => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/DLC1/BleedOnHitVoid/BleedOnHitVoid.asset").WaitForCompletion();
            public static RoR2.ItemDef ChainLightningVoid => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/DLC1/ChainLightningVoid/ChainLightningVoid.asset").WaitForCompletion();
            public static RoR2.ItemDef CloverVoid => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/DLC1/CloverVoid/CloverVoid.asset").WaitForCompletion();
            public static RoR2.ItemDef ConvertCritChanceToCritDamage => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/DLC1/ConvertCritChanceToCritDamage/ConvertCritChanceToCritDamage.asset").WaitForCompletion();
            public static RoR2.ItemDef CritDamage => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/DLC1/CritDamage/CritDamage.asset").WaitForCompletion();
            public static RoR2.ItemDef CritGlassesVoid => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/DLC1/CritGlassesVoid/CritGlassesVoid.asset").WaitForCompletion();
            public static RoR2.ItemDef DroneWeapons => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/DLC1/DroneWeapons/DroneWeapons.asset").WaitForCompletion();
            public static RoR2.ItemDef DroneWeaponsBoost => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/DLC1/DroneWeapons/DroneWeaponsBoost.asset").WaitForCompletion();
            public static RoR2.ItemDef DroneWeaponsDisplay1 => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/DLC1/DroneWeapons/DroneWeaponsDisplay1.asset").WaitForCompletion();
            public static RoR2.ItemDef DroneWeaponsDisplay2 => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/DLC1/DroneWeapons/DroneWeaponsDisplay2.asset").WaitForCompletion();
            public static RoR2.ItemDef ElementalRingVoid => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/DLC1/ElementalRingVoid/ElementalRingVoid.asset").WaitForCompletion();
            public static RoR2.ItemDef EmpowerAlways => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/DLC1/EmpowerAlways/EmpowerAlways.asset").WaitForCompletion();
            public static RoR2.ItemDef EquipmentMagazineVoid => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/DLC1/EquipmentMagazineVoid/EquipmentMagazineVoid.asset").WaitForCompletion();
            public static RoR2.ItemDef ExplodeOnDeathVoid => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/DLC1/ExplodeOnDeathVoid/ExplodeOnDeathVoid.asset").WaitForCompletion();
            public static RoR2.ItemDef ExtraLifeVoid => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/DLC1/ExtraLifeVoid/ExtraLifeVoid.asset").WaitForCompletion();
            public static RoR2.ItemDef ExtraLifeVoidConsumed => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/DLC1/ExtraLifeVoid/ExtraLifeVoidConsumed.asset").WaitForCompletion();
            public static RoR2.ItemDef FragileDamageBonus => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/DLC1/FragileDamageBonus/FragileDamageBonus.asset").WaitForCompletion();
            public static RoR2.ItemDef FragileDamageBonusConsumed => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/DLC1/FragileDamageBonus/FragileDamageBonusConsumed.asset").WaitForCompletion();
            public static RoR2.ItemDef FreeChest => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/DLC1/FreeChest/FreeChest.asset").WaitForCompletion();
            public static RoR2.ItemDef GoldOnHurt => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/DLC1/GoldOnHurt/GoldOnHurt.asset").WaitForCompletion();
            public static RoR2.ItemDef HalfAttackSpeedHalfCooldowns => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/DLC1/HalfAttackSpeedHalfCooldowns/HalfAttackSpeedHalfCooldowns.asset").WaitForCompletion();
            public static RoR2.ItemDef HalfSpeedDoubleHealth => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/DLC1/HalfSpeedDoubleHealth/HalfSpeedDoubleHealth.asset").WaitForCompletion();
            public static RoR2.ItemDef HealingPotion => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/DLC1/HealingPotion/HealingPotion.asset").WaitForCompletion();
            public static RoR2.ItemDef HealingPotionConsumed => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/DLC1/HealingPotion/HealingPotionConsumed.asset").WaitForCompletion();
            public static RoR2.ItemDef ImmuneToDebuff => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/DLC1/ImmuneToDebuff/ImmuneToDebuff.asset").WaitForCompletion();
            public static RoR2.ItemDef LunarSun => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/DLC1/LunarSun/LunarSun.asset").WaitForCompletion();
            public static RoR2.ItemDef LunarWings => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/DLC1/LunarWings/LunarWings.asset").WaitForCompletion();
            public static RoR2.ItemDef MinorConstructOnKill => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/DLC1/MinorConstructOnKill/MinorConstructOnKill.asset").WaitForCompletion();
            public static RoR2.ItemDef MissileVoid => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/DLC1/MissileVoid/MissileVoid.asset").WaitForCompletion();
            public static RoR2.ItemDef MoreMissile => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/DLC1/MoreMissile/MoreMissile.asset").WaitForCompletion();
            public static RoR2.ItemDef MoveSpeedOnKill => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/DLC1/MoveSpeedOnKill/MoveSpeedOnKill.asset").WaitForCompletion();
            public static RoR2.ItemDef MushroomVoid => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/DLC1/MushroomVoid/MushroomVoid.asset").WaitForCompletion();
            public static RoR2.ItemDef OutOfCombatArmor => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/DLC1/OutOfCombatArmor/OutOfCombatArmor.asset").WaitForCompletion();
            public static RoR2.ItemDef PermanentDebuffOnHit => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/DLC1/PermanentDebuffOnHit/PermanentDebuffOnHit.asset").WaitForCompletion();
            public static RoR2.ItemDef PrimarySkillShuriken => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/DLC1/PrimarySkillShuriken/PrimarySkillShuriken.asset").WaitForCompletion();
            public static RoR2.ItemDef RandomEquipmentTrigger => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/DLC1/RandomEquipmentTrigger/RandomEquipmentTrigger.asset").WaitForCompletion();
            public static RoR2.ItemDef RandomlyLunar => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/DLC1/RandomlyLunar/RandomlyLunar.asset").WaitForCompletion();
            public static RoR2.ItemDef RegeneratingScrap => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/DLC1/RegeneratingScrap/RegeneratingScrap.asset").WaitForCompletion();
            public static RoR2.ItemDef RegeneratingScrapConsumed => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/DLC1/RegeneratingScrap/RegeneratingScrapConsumed.asset").WaitForCompletion();
            public static RoR2.ItemDef ScrapGreenSuppressed => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/DLC1/ScrapVoid/ScrapGreenSuppressed.asset").WaitForCompletion();
            public static RoR2.ItemDef ScrapRedSuppressed => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/DLC1/ScrapVoid/ScrapRedSuppressed.asset").WaitForCompletion();
            public static RoR2.ItemDef ScrapWhiteSuppressed => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/DLC1/ScrapVoid/ScrapWhiteSuppressed.asset").WaitForCompletion();
            public static RoR2.ItemDef SlowOnHitVoid => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/DLC1/SlowOnHitVoid/SlowOnHitVoid.asset").WaitForCompletion();
            public static RoR2.ItemDef StrengthenBurn => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/DLC1/StrengthenBurn/StrengthenBurn.asset").WaitForCompletion();
            public static RoR2.ItemDef TreasureCacheVoid => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/DLC1/TreasureCacheVoid/TreasureCacheVoid.asset").WaitForCompletion();
            public static RoR2.ItemDef VoidMegaCrabItem => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/DLC1/VoidMegaCrabItem.asset").WaitForCompletion();
            public static RoR2.ItemDef SummonedEcho => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/InDev/SummonedEcho.asset").WaitForCompletion();
            public static RoR2.ItemDef AACannon => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Junk/AACannon/AACannon.asset").WaitForCompletion();
            public static RoR2.ItemDef BurnNearby => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Junk/BurnNearby/BurnNearby.asset").WaitForCompletion();
            public static RoR2.ItemDef CooldownOnCrit => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Junk/CooldownOnCrit/CooldownOnCrit.asset").WaitForCompletion();
            public static RoR2.ItemDef CritHeal => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Junk/CritHeal/CritHeal.asset").WaitForCompletion();
            public static RoR2.ItemDef Incubator => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Junk/Incubator/Incubator.asset").WaitForCompletion();
            public static RoR2.ItemDef MageAttunement => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Junk/MageAttunement/MageAttunement.asset").WaitForCompletion();
            public static RoR2.ItemDef PlantOnHit => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Junk/PlantOnHit/PlantOnHit.asset").WaitForCompletion();
            public static RoR2.ItemDef PlasmaCore => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Junk/PlasmaCore/PlasmaCore.asset").WaitForCompletion();
            public static RoR2.ItemDef SkullCounter => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Junk/SkullCounter/SkullCounter.asset").WaitForCompletion();
            public static RoR2.ItemDef TempestOnKill => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Junk/TempestOnKill/TempestOnKill.asset").WaitForCompletion();
            public static RoR2.ItemDef WarCryOnCombat => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Junk/WarCryOnCombat/WarCryOnCombat.asset").WaitForCompletion();
            public static RoR2.ItemDef EmpowerBuff => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Junk_DLC1/EmpowerItems/EmpowerBuff/EmpowerBuff.asset").WaitForCompletion();
            public static RoR2.ItemDef EmpowerMagazine => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Junk_DLC1/EmpowerItems/EmpowerMagazine/EmpowerMagazine.asset").WaitForCompletion();
            public static RoR2.ItemDef EmpowerOnKill => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Junk_DLC1/EmpowerItems/EmpowerOnKill/EmpowerOnKill.asset").WaitForCompletion();
            public static RoR2.ItemDef EmpowerRefresh => Addressables.LoadAssetAsync<RoR2.ItemDef>("RoR2/Junk_DLC1/EmpowerItems/EmpowerRefresh/EmpowerRefresh.asset").WaitForCompletion();
    }
}