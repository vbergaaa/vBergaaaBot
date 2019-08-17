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
    }
}