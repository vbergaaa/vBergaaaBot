using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SC2APIProtocol;

namespace VBergaaaBot.MapInfo
{
    class Base
    {
        public Point2D Location { get; set; }
        public List<MineralField> MineralFields { get; set; }
        public List<GasGeyser> GasGeysers { get; set; } 
        
        public Base()
        {
            MineralFields = new List<MineralField>();
            GasGeysers = new List<GasGeyser>();
        }
    }
}
