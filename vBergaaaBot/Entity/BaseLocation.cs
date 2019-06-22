using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SC2APIProtocol;

namespace Bot.Entity
{
    public class BaseLocation
    {
        public Point2D Location;
        public List<MineralField> MineralPatches;
        public List<VespeneGeyser> VespeneGeysers;

        public BaseLocation()
        {
            MineralPatches = new List<MineralField>();
            VespeneGeysers = new List<VespeneGeyser>();
        }

        public string LocationAsString()
        {
            return Location.X + "," + Location.Y;
        }
    }
}
