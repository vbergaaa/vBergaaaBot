using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SC2APIProtocol;

namespace Bot.Builds
{
    public class FirstBuild:Build
    {
        
        public override string Name()
        {
            return "FirstBuild";
        }

        public override List<BuildOrderStep> BuildOrder()
        {
            List<BuildOrderStep> bo = new List<BuildOrderStep>();
            bo.Add(new BuildOrderStep(Units.DRONE, 13));
            bo.Add(new BuildOrderStep(Units.OVERLORD, 2));
            bo.Add(new BuildOrderStep(Units.DRONE, 17));
            bo.Add(new BuildOrderStep(Units.HATCHERY, 2));
            bo.Add(new BuildOrderStep(Units.DRONE, 18));
            bo.Add(new BuildOrderStep(Units.SPAWNING_POOL, 1));
            bo.Add(new BuildOrderStep(Units.EXTRACTOR, 1));
            bo.Add(new BuildOrderStep(Units.DRONE, 19));
            bo.Add(new BuildOrderStep(Units.OVERLORD, 3));
            bo.Add(new BuildOrderStep(Units.DRONE, 20));
            bo.Add(new BuildOrderStep(Units.ZERGLING, 3));
            bo.Add(new BuildOrderStep(Units.QUEEN, 2));
            bo.Add(new BuildOrderStep(Upgrades.ZERGLING_MOVE_SPEED, true));
            bo.Add(new BuildOrderStep(Units.ZERGLING, 3));

            return bo;
        }

        public override IEnumerable<SC2APIProtocol.Action> OnFrame(VBergaaaBot vBergaaaBot)
        {
            Controller.OpenFrame();

            // macro
            BuildOrderStep nextstep = GetNextStep();

            // make lings otherwise
            if (nextstep == null)
            {
                if (Controller.maxSupply - Controller.currentSupply < 5 - 5 * Controller.GetPendingCount(Units.OVERLORD))
                    if (Controller.CanAfford(Units.OVERLORD) && Controller.GetUnits(Units.LARVA).Count > 0)
                        Controller.GetUnits(Units.LARVA)[0].Train(Units.OVERLORD);
                if (Controller.CanAfford(Units.ZERGLING) && Controller.GetUnits(Units.LARVA).Count > 0)
                    Controller.GetUnits(Units.LARVA)[0].Train(Units.ZERGLING);
            } 
            else
            {
                // check if unit is a building
                if (nextstep.Type == BuildOrderType.Unit)
                {
                    if (nextstep.Unit == Units.DRONE)
                        if (Controller.CanAfford(Units.DRONE) && Controller.GetUnits(Units.LARVA).Count > 0)
                            Controller.GetUnits(Units.LARVA)[0].Train(Units.DRONE);
                    if (nextstep.Unit == Units.OVERLORD)
                        if (Controller.CanAfford(Units.OVERLORD) && Controller.GetUnits(Units.LARVA).Count > 0)
                            Controller.GetUnits(Units.LARVA)[0].Train(Units.OVERLORD);
                    if (nextstep.Unit == Units.HATCHERY)
                        if (Controller.CanAfford(Units.HATCHERY))
                            Controller.Construct(Units.HATCHERY, vBergaaaBot.MapInformation.NaturalLocation.Location);
                    if (nextstep.Unit == Units.SPAWNING_POOL)
                        if (Controller.CanAfford(Units.SPAWNING_POOL))
                            Controller.Construct(Units.SPAWNING_POOL);
                    if (nextstep.Unit == Units.EXTRACTOR)
                        if (Controller.CanAfford(Units.EXTRACTOR))
                            Controller.Construct(Units.EXTRACTOR);
                    if (nextstep.Unit == Units.QUEEN)
                        foreach (Unit rc in Controller.GetUnits(Units.ResourceCenters, onlyIdle: true, onlyCompleted: true))
                            if (Controller.CanAfford(Units.QUEEN))
                                rc.Train(Units.QUEEN);
                    if (nextstep.Unit == Units.ZERGLING)
                        if (Controller.CanAfford(Units.ZERGLING) && Controller.GetUnits(Units.LARVA).Count > 0)
                            if (Controller.GetUnits(Units.SPAWNING_POOL, onlyCompleted: true).Count > 0)
                                Controller.GetUnits(Units.LARVA)[0].Train(Units.ZERGLING);
                }
                else if (nextstep.Type == BuildOrderType.Upgrade)
                {
                    if (nextstep.Upgrade == Upgrades.ZERGLING_MOVE_SPEED)
                        if (Controller.CanAffordUpgrade(Upgrades.ZERGLING_MOVE_SPEED))
                        {
                            Controller.Upgrade(Abilities.RESEARCH_ZERGLING_METABOLIC_BOOST, Controller.GetUnits(Units.SPAWNING_POOL)[0]);
                            VBergaaaBot.Bot.GameMilestones.ZerglingSpeedUpgraded = true;
                        }
                            
                }
            }


            // micro
            if (Controller.frame % 20 == 0)
                Controller.DistributeWorkers();

            List<Unit> lings = new List<Unit>();
            lings.AddRange(Controller.GetUnits(Units.ZERGLING));
            if (lings.Count > 30)
                Controller.Attack(lings, vBergaaaBot.MapInformation.EnemyStartLocations[0].Location);

            foreach (var queen in Controller.GetUnits(Units.QUEEN))
            {
                if (queen.Energy >= 25)
                {
                    Controller.Inject(queen);
                }
            }

            return Controller.CloseFrame();

            //if (makeGas)
            //    if (Controller.CanAfford(Units.EXTRACTOR))
            //        Controller.Construct(Units.EXTRACTOR, vBergaaaBot.MapInformation.StartLocation.VespeneGeysers[0].Location);
        }
        public override void OnStart(VBergaaaBot vBergaaaBot)
        {
            
        }

        public BuildOrderStep GetNextStep()
        {
            List<BuildOrderStep> buildOrder = BuildOrder();

            foreach (BuildOrderStep step in buildOrder)
            {
                // check if we want it.
                if (step.Type == BuildOrderType.Unit)
                    if (Controller.GetTotalCount(step.Unit) < step.Qty)
                        return step;
                if (step.Type == BuildOrderType.Upgrade)
                    if (!Controller.HasUpgrade(step.Upgrade))
                        return step;
            }

            return null;
        }
    }
}
