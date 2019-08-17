using SC2APIProtocol;
using System.Collections.Generic;

namespace vBergaaaBot.MicroControllers
{
    public class ZergScoutController : MicroController
    {
        bool scoutWithZergling = false;
        Agent zerglingScout;
        Queue<Point2D> ScoutLocations = VBot.Bot.Map.GetScoutLocations();
        public Point2D GetNextScoutLocation()
        {
            var scoutLoc = ScoutLocations.Dequeue();
            ScoutLocations.Enqueue(scoutLoc);
            return scoutLoc;
        }
        public override void CheckRequirements()
        {
            if (!scoutWithZergling)
            {
                if (Controller.GetAvailableAgent(Units.ZERGLING) != null)
                    scoutWithZergling = true;
            }
            else
            {
                if (zerglingScout == null || Controller.GetAgentByTag(zerglingScout.Unit.Tag) == null)
                {
                    scoutWithZergling = false;
                    zerglingScout = null;
                }
                    
            }   
                
        }

        public override void OnFrame()
        {
            CheckRequirements();
            if (scoutWithZergling)
            {
                if (zerglingScout == null)
                    zerglingScout = Controller.GetAvailableAgent(Units.ZERGLING);
                if (zerglingScout != null)
                {
                    if (!zerglingScout.Busy)
                        zerglingScout.Busy = true;
                    if (zerglingScout.Unit.Orders.Count > 0)
                        return;
                    zerglingScout.Order(Abilities.MOVE, GetNextScoutLocation());
                }
            }
            
        }
    }
}
