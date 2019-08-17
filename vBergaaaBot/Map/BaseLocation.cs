using SC2APIProtocol;
using System.Collections.Generic;

namespace vBergaaaBot.Map
{
    public class BaseLocation
    {
        public Point Location;
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