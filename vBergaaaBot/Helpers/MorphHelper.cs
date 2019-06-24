using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vBergaaaBot;

namespace vBergaaaBot.Helpers
{
    public static class MorphHelper
    {
        public static List<MorphStep> MorpSteps = GetMorphSteps();

        private static List<MorphStep> GetMorphSteps()
        {
            List<MorphStep> steps = new List<MorphStep>();
            steps.Add(new MorphStep(Units.DRONE, Units.LARVA));
            steps.Add(new MorphStep(Units.ZERGLING, Units.LARVA));
            steps.Add(new MorphStep(Units.BANELING, Units.ZERGLING));
            steps.Add(new MorphStep(Units.ROACH, Units.LARVA));
            steps.Add(new MorphStep(Units.RAVAGER, Units.ROACH));
            steps.Add(new MorphStep(Units.HYDRALISK, Units.LARVA));
            steps.Add(new MorphStep(Units.LURKER, Units.HYDRALISK));
            steps.Add(new MorphStep(Units.MUTALISK, Units.LARVA));
            steps.Add(new MorphStep(Units.CORRUPTOR, Units.LARVA));
            steps.Add(new MorphStep(Units.BROOD_LORD, Units.CORRUPTOR));
            steps.Add(new MorphStep(Units.INFESTOR, Units.LARVA));
            steps.Add(new MorphStep(Units.ULTRALISK, Units.LARVA));
            // get number for swarm host
            // get number for viper
            steps.Add(new MorphStep(Units.OVERLORD, Units.LARVA));
            steps.Add(new MorphStep(Units.OVERSEER, Units.OVERLORD));
            // get number for dropperlord
            return steps;
        }
    }

    public class MorphStep
    {
        public uint FromUnit { get; set; }
        public uint ToUnit { get; set; }
        public MorphStep(uint to, uint from)
        {
            FromUnit = from;
            ToUnit = to;
        }
    }
}
