using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vBergaaaBot;

namespace vBergaaaBot.Helpers
{
    public class TrainHelper
    {
        public static List<TrainStep> TrainSteps = GetTrainSteps();

        private static List<TrainStep> GetTrainSteps()
        {
            List<TrainStep> steps = new List<TrainStep>();
            steps.Add(new TrainStep(Units.QUEEN, new HashSet<uint> { Units.HATCHERY, Units.LAIR, Units.HIVE }));
            // add other units here if we start other races.
            return steps;
        }
    }

    public class TrainStep
    {
        public HashSet<uint> FromBuildings { get; set; }
        public uint Unit { get; set; }
        public TrainStep(uint to, uint from)
        {
            HashSet<uint> buildings = new HashSet<uint> { from };
            FromBuildings = buildings;
            Unit = to;
        }
        public TrainStep(uint to, HashSet<uint> from)
        {
            FromBuildings = from;
            Unit = to;
        }
    }
}
