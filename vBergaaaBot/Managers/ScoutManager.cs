using SC2APIProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vBergaaaBot.Entity;

namespace vBergaaaBot.Managers
{
    public class ScoutManager : Manager
    {
        private Unit ScoutOverlord1;
        private Unit ScoutOverlord2;
        private Unit ScoutOverlord3;
        private Point2D Location1;
        private Point2D Location2;
        private Point2D Location3;
        int i;

        public ScoutManager()
        {
             i = locations.Count() - 2;
        }

        private IEnumerable<BaseLocation> locations = VBergaaaBot.Bot.MapInformation.BaseLocations.ToList();

        public override void OnFrame(VBergaaaBot bot)
        {
            
            if (Controller.frame % 30 == 1)
            {
                if (ScoutOverlord1 == null && Controller.GetUnits(Units.OVERLORD, onlyIdle:true).Count()>0)
                {
                    ScoutOverlord1 = Controller.GetUnits(Units.OVERLORD, onlyIdle: true)[0];
                }
                else if (ScoutOverlord2 == null && Controller.GetUnits(Units.OVERLORD, onlyIdle:true).Count()>0)
                {
                    ScoutOverlord2 = Controller.GetUnits(Units.OVERLORD, onlyIdle: true)[0];
                }
                else if (ScoutOverlord3 == null && Controller.GetUnits(Units.OVERLORD, onlyIdle:true).Count()>0)
                {
                    ScoutOverlord3 = Controller.GetUnits(Units.OVERLORD, onlyIdle: true)[0];
                }

                if (ScoutOverlord1 != null)
                    ScoutOverlord1 = Controller.GetUnitByTag(ScoutOverlord1.Tag);
                if (ScoutOverlord2 != null)
                    ScoutOverlord2 = Controller.GetUnitByTag(ScoutOverlord2.Tag);
                if (ScoutOverlord3 != null)
                    ScoutOverlord3 = Controller.GetUnitByTag(ScoutOverlord3.Tag);
                

                if (ScoutOverlord1 != null && ScoutOverlord1.orders.Count == 0)
                {
                    Location1 = locations.ToList()[i].Location;
                    i--;
                    Controller.Move(ScoutOverlord1, Location1);
                }
                if (ScoutOverlord2 != null && ScoutOverlord2.orders.Count == 0)
                {
                    Location2 = locations.ToList()[i].Location;
                    i--;
                    Controller.Move(ScoutOverlord2, Location2);
                }
                if (ScoutOverlord3 != null && ScoutOverlord3.orders.Count == 0)
                {
                    Location3 = locations.ToList()[i].Location;
                    i--;
                    Controller.Move(ScoutOverlord3, Location3);
                }
            }
        }
    }
}
