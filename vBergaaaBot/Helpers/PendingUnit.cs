using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vBergaaaBot.Helpers
{
    public class PendingUnit
    {
        public uint UnitType { get; set; }
        public long ExpectedFrame { get; set; }
        public ulong UnitTag { get; set; }

        public PendingUnit(uint unitType, long expFrame)
        {
            UnitType = unitType;
            ExpectedFrame = expFrame;
        }
        public PendingUnit(uint unitType, ulong tag)
        {
            UnitType = unitType;
            UnitTag = tag;
        }
    }
}
