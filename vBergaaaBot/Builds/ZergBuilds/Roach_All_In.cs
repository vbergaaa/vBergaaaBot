using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SC2APIProtocol;

namespace vBergaaaBot.Builds
{
    public class Roach_All_In : Build
    {

        public override string Name()
        {
            return "Roach_All_In";
        }

        public override List<BuildOrderStep> GetBuildOrder()
        {
            List<BuildOrderStep> bo = new List<BuildOrderStep>();
            bo.Add(new BuildOrderStep(Units.DRONE, 13));
            bo.Add(new BuildOrderStep(Units.OVERLORD, 2));
            bo.Add(new BuildOrderStep(Units.DRONE, 17));
            bo.Add(new BuildOrderStep(Units.EXTRACTOR, 1));
            bo.Add(new BuildOrderStep(Units.DRONE, 17));
            bo.Add(new BuildOrderStep(Units.SPAWNING_POOL, 1));
            bo.Add(new BuildOrderStep(Units.DRONE, 18));
            bo.Add(new BuildOrderStep(Units.HATCHERY, 2));
            bo.Add(new BuildOrderStep(Units.DRONE, 19));
            bo.Add(new BuildOrderStep(Units.QUEEN, 1));
            bo.Add(new BuildOrderStep(Units.ZERGLING, 1));
            bo.Add(new BuildOrderStep(Upgrades.ZERGLING_MOVE_SPEED, true));
            bo.Add(new BuildOrderStep(Units.ROACH_WARREN, 1));
            bo.Add(new BuildOrderStep(Units.DRONE, 19));
            bo.Add(new BuildOrderStep(Units.OVERLORD, 4));
            bo.Add(new BuildOrderStep(Units.ROACH, 8));
            bo.Add(new BuildOrderStep(Units.ZERGLING, 6));
            bo.Add(new BuildOrderStep(Units.OVERLORD, 5));
            bo.Add(new BuildOrderStep(Units.ZERGLING, 14));
            bo.Add(new BuildOrderStep(Units.OVERLORD, 6));
            bo.Add(new BuildOrderStep(Units.ZERGLING, 22));
            bo.Add(new BuildOrderStep(Units.OVERLORD, 8));
            bo.Add(new BuildOrderStep(Units.ZERGLING, 30));
            return bo;
        }
        public override List<BuildOrderStep> MaxOutComp()
        {
            List<BuildOrderStep> bo = new List<BuildOrderStep>();
            bo.Add(new BuildOrderStep(Units.DRONE, 19));
            bo.Add(new BuildOrderStep(Units.ZERGLING, 1000));
            return bo;
        }

        private bool attack = false;
        private Unit leadRoach = null;
        private int gasCount = 0;
        public override void OnFrame(VBergaaaBot vBergaaaBot)
        {

            if (gasCount < Controller.GetUnits(Units.EXTRACTOR, onlyCompleted: true).Count)
            {
                Controller.PrioritiseGas();
                gasCount = Controller.GetUnits(Units.EXTRACTOR, onlyCompleted: true).Count;
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

                Controller.Attack(groupies, vBergaaaBot.MapInformation.EnemyStartLocations[0]);

                if (leadRoach != null)
                {
                    Controller.Attack(outCasts, leadRoach.Pos);
                }
                else
                {
                    Controller.Attack(outCasts, vBergaaaBot.MapInformation.EnemyStartLocations[0]);
                }
            }

            foreach (var queen in Controller.GetUnits(Units.QUEEN))
            {
                if (queen.Energy >= 25)
                {
                    Controller.Inject(queen);
                }
            }

            //if (makeGas)
            //    if (Controller.CanAfford(Units.EXTRACTOR))
            //        Controller.Construct(Units.EXTRACTOR, vBergaaaBot.MapInformation.StartLocation.VespeneGeysers[0].Location);
        }
        
        public override void OnStart(VBergaaaBot vBergaaaBot)
        {
            // load up managers?
        }


    }
}
