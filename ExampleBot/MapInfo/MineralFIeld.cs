using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SC2APIProtocol;

namespace VBergaaaBot.MapInfo
{
    class MineralField
    {
        public Point2D Location { get; set; }
        public int InitialMinerals { get; set; }
        public bool RichMineralField { get; set; }
    }
}
