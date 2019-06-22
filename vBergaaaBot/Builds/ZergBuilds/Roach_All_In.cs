using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SC2APIProtocol;

namespace Bot.Builds
{
    public class Roach_All_In : Build
    {

        public override string Name()
        {
            return "Roach_All_In";
        }

        public override List<BuildOrderStep> BuildOrder()
        {
            List<BuildOrderStep> bo = new List<BuildOrderStep>();
            bo.Add(new BuildOrderStep(Units.DRONE, 13));
            bo.Add(new BuildOrderStep(Units.OVERLORD, 2));
            bo.Add(new BuildOrderStep(Units.DRONE, 17));
            bo.Add(new BuildOrderStep(Units.EXTRACTOR, 1));
            bo.Add(new BuildOrderStep(Units.DRONE, 17));
            bo.Add(new BuildOrderStep(Units.SPAWNING_POOL, 1));
            bo.Add(new BuildOrderStep(Units.DRONE, 17));
            bo.Add(new BuildOrderStep(Units.HATCHERY, 2));
            bo.Add(new BuildOrderStep(Units.DRONE, 18));
            bo.Add(new BuildOrderStep(Units.QUEEN, 1));
            bo.Add(new BuildOrderStep(Units.ZERGLING, 1));
            bo.Add(new BuildOrderStep(Upgrades.ZERGLING_MOVE_SPEED, true));
            bo.Add(new BuildOrderStep(Units.ROACH_WARREN, 1));
            bo.Add(new BuildOrderStep(Units.DRONE, 18));
            bo.Add(new BuildOrderStep(Units.OVERLORD, 4));
            bo.Add(new BuildOrderStep(Units.ROACH, 8));
            bo.Add(new BuildOrderStep(Units.ZERGLING, 6));
            bo.Add(new BuildOrderStep(Units.OVERLORD, 5));
            bo.Add(new BuildOrderStep(Units.ZERGLING, 14));
            return bo;
        }

        private bool attack = false;
        private Unit leadRoach = null;
        private int gasCount = 0;
        private ulong lastMakeUnitFrame = 0;
        public override IEnumerable<SC2APIProtocol.Action> OnFrame(VBergaaaBot vBergaaaBot)
        {

            Controller.OpenFrame();
            try
            {
                if (gasCount < Controller.GetUnits(Units.EXTRACTOR, onlyCompleted: true).Count)
                {
                    Controller.PrioritiseGas();
                    gasCount = Controller.GetUnits(Units.EXTRACTOR, onlyCompleted: true).Count;
                }

                // macro
                BuildOrderStep nextstep = GetNextStep();

                if (lastMakeUnitFrame + 3 < Controller.frame)
                {
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
                        // check if unit is a unit
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
                            if (nextstep.Unit == Units.ROACH_WARREN)
                                if (Controller.CanAfford(Units.ROACH_WARREN) && Controller.GetUnits(Units.SPAWNING_POOL, onlyCompleted: true).Count > 0)
                                    Controller.Construct(Units.ROACH_WARREN);
                            if (nextstep.Unit == Units.EXTRACTOR)
                                if (Controller.CanAfford(Units.EXTRACTOR))
                                    Controller.Construct(Units.EXTRACTOR);
                            if (nextstep.Unit == Units.QUEEN)
                                foreach (Unit rc in Controller.GetUnits(Units.ResourceCenters, onlyIdle: true, onlyCompleted: true))
                                    if (Controller.CanAfford(Units.QUEEN) && Controller.GetUnits(Units.SPAWNING_POOL, onlyCompleted: true).Count > 0)
                                        rc.Train(Units.QUEEN);
                            if (nextstep.Unit == Units.ZERGLING)
                            {
                                if (Controller.CanAfford(Units.ZERGLING) && Controller.GetUnits(Units.LARVA).Count > 0)
                                    if (Controller.GetUnits(Units.SPAWNING_POOL, onlyCompleted: true).Count > 0)
                                        Controller.GetUnits(Units.LARVA)[0].Train(Units.ZERGLING);
                            }

                            if (nextstep.Unit == Units.ROACH)
                                if (Controller.CanAfford(Units.ROACH) && Controller.GetUnits(Units.LARVA).Count > 0)
                                    if (Controller.GetUnits(Units.ROACH_WARREN, onlyCompleted: true).Count > 0)
                                        Controller.GetUnits(Units.LARVA)[0].Train(Units.ROACH);
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
                    lastMakeUnitFrame = Controller.frame;
                }
                


                // micro
                if (Controller.frame % 20 == 0)
                    Controller.DistributeWorkers();


                List<Unit> roaches = new List<Unit>();
                roaches.AddRange(Controller.GetUnits(Units.ROACH));
                List<Unit> lings = new List<Unit>();
                lings.AddRange(Controller.GetUnits(Units.ZERGLING));

                if (roaches.Count > 0 && !attack && leadRoach == null)
                {
                    leadRoach = roaches[0];
                }

                if (roaches.Count > 0 && !attack && leadRoach != null)
                {
                    Controller.Move(roaches, leadRoach.Pos);
                    Controller.Move(roaches, leadRoach.Pos);
                }

                if (roaches.Count >= 8 && !attack)
                {
                    attack = true;
                }

                if (attack && Controller.frame % 15 == 0)
                {
                    leadRoach = Controller.GetUnitByTag(leadRoach.Tag);
                    List<Unit> army = new List<Unit>();
                    army.AddRange(roaches);
                    army.AddRange(lings);
                    army.Remove(leadRoach);

                    List<Unit> groupies = new List<Unit>();
                    List<Unit> outCasts = new List<Unit>();

                    foreach (Unit unit in army)
                    {
                        if (Controller.Get2dDistanceSquared(leadRoach.Pos, unit.Pos) < 25)
                        {
                            groupies.Add(unit);
                        }
                        else
                        {
                            outCasts.Add(unit);
                        }
                    }

                    Controller.Attack(groupies, vBergaaaBot.MapInformation.EnemyStartLocations[0].Location);

                    if (leadRoach != null)
                    {
                        Controller.Attack(outCasts, leadRoach.Pos);
                    }
                    else
                    {
                        Controller.Attack(outCasts, vBergaaaBot.MapInformation.EnemyStartLocations[0].Location);
                    }
                }

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
            catch
            {
                Logger.Error("The Most Recent Item Failed...");
                
            }

            Controller.OpenFrame();
            return Controller.CloseFrame();


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
