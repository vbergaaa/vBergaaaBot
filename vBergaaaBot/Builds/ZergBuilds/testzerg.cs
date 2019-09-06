using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vBergaaaBot.MicroControllers;

namespace vBergaaaBot.Builds.ZergBuilds
{
    public class Testzerg : Build
    {
        public override string Name => "test";

        public override List<MicroController> AddControllers()
        {
            List<MicroController> controllers = new List<MicroController>();
            controllers.Add(new InjectController());
            return controllers;
        }

        public override List<BuildStep> GetMainBuild()
        {
            return new List<BuildStep> {
                new BuildStep(Units.EXTRACTOR,2),
                new BuildStep(Units.EVOLUTION_CHAMBER,2),
                new BuildStep(Units.LAIR,1,() => Controller.CheckUpgrade(Upgrades.ZERG_CARAPACE_1)),
                new BuildStep(Units.INVESTATION_PIT,1,() => Controller.CheckUpgrade(Upgrades.ZERG_CARAPACE_2)),
                new BuildStep(Units.HIVE,1)
            };
        }

        public override List<BuildStep> GetOpener()
        {
            List<BuildStep> buildSteps = new List<BuildStep>();
            buildSteps.Add(new BuildStep(Units.DRONE, 13));
            buildSteps.Add(new BuildStep(Units.OVERLORD, 2));
            buildSteps.Add(new BuildStep(Units.DRONE, 17));
            buildSteps.Add(new BuildStep(Units.HATCHERY, 2));
            buildSteps.Add(new BuildStep(Units.DRONE, 17));
            buildSteps.Add(new BuildStep(Units.SPAWNING_POOL, 1));
            buildSteps.Add(new BuildStep(Units.EXTRACTOR, 2));
            buildSteps.Add(new BuildStep(Units.DRONE, 19));
            return buildSteps;
        }

        public override List<BuildStep> GetProduceList()
        {
            List<BuildStep> buildSteps = new List<BuildStep>();
            buildSteps.Add(new BuildStep(Units.QUEEN, 3, () => Controller.GetTotalCount(Units.HATCHERY)-1 >= Controller.GetTotalCount(Units.QUEEN)));
            buildSteps.Add(new BuildStep(Units.HATCHERY, 2, () => Controller.GetTotalCount(Units.HATCHERY)*16 + 3 < Controller.GetTotalCount(Units.DRONE)));
            buildSteps.Add(new BuildStep(Units.DRONE, 38));
            buildSteps.Add(new BuildStep(Units.ZERGLING, 400, () => Controller.GetTotalCount(Units.DRONE) >= 38));
            return buildSteps;
        }

        public override List<BuildStep> GetUpgrades()
        {
            return new List<BuildStep> {
                new BuildStep(Upgrades.ZERG_MELEE_WEAPONS_1),
                new BuildStep(Upgrades.ZERG_CARAPACE_1),
                new BuildStep(Units.LAIR,1),
                new BuildStep(Upgrades.ZERG_MELEE_WEAPONS_2),
                new BuildStep(Upgrades.ZERG_CARAPACE_2),
                new BuildStep(Units.INVESTATION_PIT,1),
                new BuildStep(Units.HIVE,1),
                new BuildStep(Upgrades.ZERG_MELEE_WEAPONS_3),
                new BuildStep(Upgrades.ZERG_CARAPACE_3),
            };
        }

        internal override int SetGasWorkerCount()
        {
            return 6;
        }
    }
}
