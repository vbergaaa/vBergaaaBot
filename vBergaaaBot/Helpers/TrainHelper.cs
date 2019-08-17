using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vBergaaaBot.Helpers
{
    public class TrainHelper
    {
        public static Dictionary<uint, HashSet<uint>> TrainSteps = GetTrainSteps();

        private static Dictionary<uint, HashSet<uint>> GetTrainSteps()
        {
            Dictionary<uint, HashSet<uint>> steps = new Dictionary<uint, HashSet<uint>>();
            steps.Add(Units.QUEEN, new HashSet<uint> { Units.HATCHERY, Units.LAIR, Units.HIVE });

            // add all terran in and protoss units as required

            return steps;
        }

        public static HashSet<uint> GetTrainingBuildingTypes(uint unitType)
        {
            if (TrainSteps.ContainsKey(unitType))
                return TrainSteps[unitType];

            // log error if cant find unit
            Logger.Error("Unable to find train step for {0} - Type: {1}.", VBot.Bot.Data.Units[(int)unitType].Name, unitType);
            return new HashSet<uint> { 0 };
        }
    }
}
