using BotUpgrades = vBergaaaBot.Upgrades;
using System.Collections.Generic;
using vBergaaaBot.MicroControllers;
using vBergaaaBot.Managers;
using vBergaaaBot.Builds;

namespace vBergaaaBot.Builds.ZergBuilds
{
    public class LingRush : Build
    {
        public override string Name => "LingRush";

        public override List<BuildStep> GetOpener()
        {
            List<BuildStep> order = new List<BuildStep>();
            order.Add(new BuildStep(Units.DRONE, 13));
            order.Add(new BuildStep(Units.DRONE, 14, () => { return Controller.GetTotalCount(Units.SPAWNING_POOL) > 0; }));
            order.Add(new BuildStep(Units.OVERLORD, 2));
            order.Add(new BuildStep(Units.ZERGLING, 6));
            order.Add(new BuildStep(Units.QUEEN, 1));
            return order;
        }

        public override List<BuildStep> GetMainBuild()
        {
            List<BuildStep> order = new List<BuildStep>();
            order.Add(new BuildStep(Units.EXTRACTOR, 1,() => { return Controller.GetTotalCount(Units.DRONE) > 12; }));
            order.Add(new BuildStep(Units.SPAWNING_POOL, 1));
            return order;
        }

        public override List<BuildStep> GetUpgrades()
        {
            List<BuildStep> order = new List<BuildStep>();
            order.Add(new BuildStep(BotUpgrades.ZERGLING_MOVESPEED));
            return order;
        }

        public override void OnStart()
        {
            base.OnStart();
            // add build things after micro manager?
            
        }

        internal override int SetGasWorkerCount()
        {
            return 3;
        }

        public override void OnFrame()
        {
            if ((VBot.Bot.Gas() >= 100 || Controller.CheckUpgrade(BotUpgrades.ZERGLING_MOVESPEED)) && IdealGasWorkers > 0)
            {
                IdealGasWorkers = 0;
            }
            base.OnFrame();
        }

        public override List<MicroController> AddControllers()
        {
            List<MicroController> controllers = new List<MicroController>();
            controllers.Add(new InjectController());
            controllers.Add(new CreepController());
            controllers.Add(new AttackController(20, 0));
            controllers.Add(new ZergScoutController());
            return controllers;
        }

        public override List<BuildStep> GetProduceList()
        {
            List<BuildStep> order = new List<BuildStep>();
            order.Add(new BuildStep(Units.ZERGLING, 400));
            order.Add(new BuildStep(Units.QUEEN, 2));
            return order;
        }
    }
}
