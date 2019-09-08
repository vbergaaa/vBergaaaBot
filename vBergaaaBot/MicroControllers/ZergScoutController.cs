using SC2APIProtocol;
using vBergaaaBot.Managers;
using System.Collections.Generic;
using System.Linq;
using vBergaaaBot.Tasks;

namespace vBergaaaBot.MicroControllers
{
    public class ZergScoutController : MicroController
    {
        private List<Point2D> scoutLocations = VBot.Bot.Map.GetScoutLocations();
        public override void CheckRequirements()
        {
            if (Controller.GetCompletedCount(new HashSet<uint> { Units.ZERGLING, Units.OVERLORD }) > 0)
                Activate();
            else
                Deactivate();
        }

        public override void OnFrame()
        {
            List<Agent> overlords = AssignedAgents.Where(a => a.Unit.UnitType == Units.OVERLORD).ToList();
            if (overlords.Count() < 2 && overlords.Count() < Controller.GetCompletedCount(Units.OVERLORD))
            {
                Agent ov = Controller.GetAvailableAgent(Units.OVERLORD);
                ov.Busy = true;
                AssignAgents(ov);
            }

            int ovieCount = overlords.Count() ;

            foreach (Point2D loc in scoutLocations)
            {
                if (ovieCount == 0)
                    break;
                if (EnemyStrategyManager.EnemyBuildingAtLocation(loc))
                    continue;
                new SingleUnitScoutTask(overlords[overlords.Count() - ovieCount], loc);
                ovieCount--;
            }


        }
    }
}
