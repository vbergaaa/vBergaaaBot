using System.Collections.Generic;
using System.Linq;

namespace vBergaaaBot
{
    internal static class Upgrades
    {
        public static uint GetAbilityId(int upgradeId)
        {
            return VBot.Bot.Data.Upgrades[upgradeId].AbilityId;
        }

        public static int ZERGLING_MOVESPEED = 66;
        public static int ZERG_MELEE_WEAPONS_1 = 53;
        public static int ZERG_MELEE_WEAPONS_2 = 54;
        public static int ZERG_MELEE_WEAPONS_3 = 55;
        public static int ZERG_CARAPACE_1 = 56;
        public static int ZERG_CARAPACE_2 = 57;
        public static int ZERG_CARAPACE_3 = 58;
    }
}