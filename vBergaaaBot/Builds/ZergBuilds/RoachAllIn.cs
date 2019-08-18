using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vBergaaaBot.MicroControllers;

namespace vBergaaaBot.Builds.ZergBuilds
{
    public class RoachAllIn : Build
    {
        public override string Name => "19Drone-RoachAllIn";

        public override List<BuildStep> GetOpener()
        {
            List<BuildStep> order = new List<BuildStep>();
            order.Add(new BuildStep(Units.DRONE, 13));
            order.Add(new BuildStep(Units.OVERLORD, 2));
            order.Add(new BuildStep(Units.DRONE, 17));
            order.Add(new BuildStep(Units.EXTRACTOR, 1));
            order.Add(new BuildStep(Units.DRONE, 17));
            order.Add(new BuildStep(Units.SPAWNING_POOL, 1));
            order.Add(new BuildStep(Units.DRONE, 18));
            order.Add(new BuildStep(Units.HATCHERY, 2));
            order.Add(new BuildStep(Units.DRONE, 19));
            order.Add(new BuildStep(Units.QUEEN, 1));
            order.Add(new BuildStep(Upgrades.ZERGLING_MOVESPEED));
            order.Add(new BuildStep(Units.ROACH_WARREN, 1));
            order.Add(new BuildStep(Units.DRONE, 19));
            order.Add(new BuildStep(Units.OVERLORD, 4));
            order.Add(new BuildStep(Units.ROACH, 8));
            return order;
        }

        public override List<BuildStep> GetMainBuild()
        {
            List<BuildStep> order = new List<BuildStep>();
            order.Add(new BuildStep(Units.HATCHERY, 1));
            order.Add(new BuildStep(Units.DRONE, 5));
            order.Add(new BuildStep(Units.SPAWNING_POOL, 1));
            order.Add(new BuildStep(Units.ROACH_WARREN, 1));
            order.Add(new BuildStep(Units.DRONE, 12));
            return order;
        }

        public override List<BuildStep> GetUpgrades()
        {
            List<BuildStep> order = new List<BuildStep>();
            order.Add(new BuildStep(Upgrades.ZERGLING_MOVESPEED));
            return order;
        }

        internal override int SetGasWorkerCount()
        {
            return 3;
        }

        public override List<MicroController> AddControllers()
        {
            List<MicroController> controllers = new List<MicroController>();
            controllers.Add(new InjectController());
            controllers.Add(new CreepController());
            controllers.Add(new AttackController(16, 0));
            controllers.Add(new ZergScoutController());
            return controllers;
        }

        public override List<BuildStep> GetProduceList()
        {
            List<BuildStep> order = new List<BuildStep>();
            order.Add(new BuildStep(Units.ZERGLING, 400));
            order.Add(new BuildStep(Units.QUEEN, 3));
            return order;
        }
    }
}
