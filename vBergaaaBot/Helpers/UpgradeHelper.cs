using System.Collections.Generic;

namespace vBergaaaBot.Helpers
{
    public class UpgradeHelper
    {
        // props
        public static Dictionary<int, HashSet<uint>> UpgradeSteps = GetUpgradeSteps();
        public static Dictionary<int, uint> UpgradeTechBuildingRequirements = GetTechBuildingRequirements();
        public static Dictionary<int, int> UpgradeTechUpgradeRequirements = GetTechUpgradeRequirements();

        // Edit these as new upgrades are added to the bot
        private static Dictionary<int, HashSet<uint>> GetUpgradeSteps()
        {
            Dictionary<int, HashSet<uint>> steps = new Dictionary<int, HashSet<uint>>();
            steps.Add(Upgrades.ZERGLING_MOVESPEED, new HashSet<uint> { Units.SPAWNING_POOL });
            steps.Add(Upgrades.ZERG_MELEE_WEAPONS_1, new HashSet<uint> { Units.EVOLUTION_CHAMBER });
            steps.Add(Upgrades.ZERG_MELEE_WEAPONS_2, new HashSet<uint> { Units.EVOLUTION_CHAMBER });
            steps.Add(Upgrades.ZERG_MELEE_WEAPONS_3, new HashSet<uint> { Units.EVOLUTION_CHAMBER });
            steps.Add(Upgrades.ZERG_CARAPACE_1, new HashSet<uint> { Units.EVOLUTION_CHAMBER });
            steps.Add(Upgrades.ZERG_CARAPACE_2, new HashSet<uint> { Units.EVOLUTION_CHAMBER });
            steps.Add(Upgrades.ZERG_CARAPACE_3, new HashSet<uint> { Units.EVOLUTION_CHAMBER });

            // add all terran in and protoss upgrades as required

            return steps;
        }
        private static Dictionary<int, uint> GetTechBuildingRequirements()
        {
            Dictionary<int, uint> steps = new Dictionary<int, uint>();
            steps.Add(Upgrades.ZERG_MELEE_WEAPONS_2, Units.LAIR);
            steps.Add(Upgrades.ZERG_MELEE_WEAPONS_3, Units.HIVE);
            return steps;
        }
        private static Dictionary<int, int> GetTechUpgradeRequirements()
        {
            Dictionary<int, int> steps = new Dictionary<int, int>();
            steps.Add(Upgrades.ZERG_MELEE_WEAPONS_2, Upgrades.ZERG_MELEE_WEAPONS_1);
            steps.Add(Upgrades.ZERG_MELEE_WEAPONS_3, Upgrades.ZERG_MELEE_WEAPONS_2);
            return steps;
        }

        // These are the static methods to reference from other classes
        /// <summary>
        /// Searchs the GetUpgradeSteps to find what unit type creates the desired upgrade
        /// </summary>
        /// <param name="upgradeType">the type of the desired upgrade</param>
        /// <returns>the type of the unit that researchs the upgrade</returns>
        public static HashSet<uint> GetUpgradeBuildingTypes(int upgradeType)
        {
            if (UpgradeSteps.ContainsKey(upgradeType))
                return UpgradeSteps[upgradeType];

            // log error if cant find unit
            Logger.Error("Unable to find upgrade step for {0} - Type: {1}.", VBot.Bot.Data.Upgrades[(int)upgradeType].Name, upgradeType);
            return new HashSet<uint> { 0 };
        }

        /// <summary>
        /// Checks to see if an upgrade requires a building to exist before it can be researched
        /// </summary>
        /// <param name="upgradeType">the type of the desrired upgrade</param>
        /// <returns>the type of the unit requred to research if one exist, 0 otherwise</returns>
        public static uint GetUpgradeTechBuildingReq(int upgradeType)
        {
            if (UpgradeTechBuildingRequirements.ContainsKey(upgradeType))
                return UpgradeTechBuildingRequirements[upgradeType];
            else
                return 0;
        }

        /// <summary>
        /// Checks to see if an upgrade requires another upgrade to exist before it can be researched
        /// </summary>
        /// <param name="upgradeType">the type of the desrired upgrade</param>
        /// <returns>the type of the upgrade requred to research if one is required, 0 otherwise</returns>
        public static int GetUpgradeTechUpgradeReq(int upgradeType)
        {
            if (UpgradeTechUpgradeRequirements.ContainsKey(upgradeType))
                return UpgradeTechUpgradeRequirements[upgradeType];
            else
                return 0;
        }
    }
}
