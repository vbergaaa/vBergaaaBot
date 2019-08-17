
using System.Collections.Generic;

namespace vBergaaaBot.Helpers
{
    public static class MorphHelper
    {
        public static Dictionary<uint, uint> MorpSteps = GetMorphSteps();

        private static Dictionary<uint, uint> GetMorphSteps()
        {
            Dictionary<uint, uint> steps = new Dictionary<uint, uint>();
            steps.Add(Units.DRONE, Units.LARVA);
            steps.Add(Units.ZERGLING, Units.LARVA);
            steps.Add(Units.BANELING, Units.ZERGLING);
            steps.Add(Units.ROACH, Units.LARVA);
            steps.Add(Units.RAVAGER, Units.ROACH);
            steps.Add(Units.HYDRALISK, Units.LARVA);
            steps.Add(Units.LURKER, Units.HYDRALISK);
            steps.Add(Units.MUTALISK, Units.LARVA);
            steps.Add(Units.CORRUPTOR, Units.LARVA);
            steps.Add(Units.BROOD_LORD, Units.CORRUPTOR);
            steps.Add(Units.INFESTOR, Units.LARVA);
            steps.Add(Units.ULTRALISK, Units.LARVA);
            // get number for swarm host
            // get number for viper
            steps.Add(Units.OVERLORD, Units.LARVA);
            steps.Add(Units.OVERSEER, Units.OVERLORD);
            // get number for dropperlord
            return steps;
        }

        /// <summary>
        /// searches morph steps to find what unit type morphs into the desired unit
        /// </summary>
        /// <param name="unitType">the desired unit to morph into</param>
        /// <returns>the unit type of the unit required to morph into the desired unit</returns>
        public static uint GetPreMorphType(uint unitType)
        {
            if (MorpSteps.ContainsKey(unitType))
                return MorpSteps[unitType];

            // log error if cant find unit
            Logger.Error("Unable to find morph step for {0} - Type: {1}.", VBot.Bot.Data.Units[(int)unitType].Name, unitType);
            return 0;
        }
    }
}
