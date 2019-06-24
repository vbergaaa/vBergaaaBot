using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vBergaaaBot;

namespace vBergaaaBot.Helpers
{
    public static class UpgradeHelper
    {
        public static List<UpgradeStep> UpgradeSteps = GetUpgradeSteps();

        private static List<UpgradeStep> GetUpgradeSteps()
        {
            List<UpgradeStep> steps = new List<UpgradeStep>();
            steps.Add(new UpgradeStep(Upgrades.ZERGLING_MOVE_SPEED, Units.SPAWNING_POOL));
            
            return steps;
        }
    }

    public class UpgradeStep
    {
        public HashSet<uint> FromBuilding { get; set; }
        public int Upgrade { get; set; }
        public UpgradeStep(int to, uint from)
        {
            FromBuilding = new HashSet<uint> { from };
            Upgrade = to;
        }
    }
}
