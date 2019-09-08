using SC2APIProtocol;
using System.Collections.Generic;

namespace vBergaaaBot.Map
{
    public class Base
    {
        public Point Location;
        public List<MineralField> MineralPatches;
        public List<VespeneGeyser> VespeneGeysers;

        public Base()
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