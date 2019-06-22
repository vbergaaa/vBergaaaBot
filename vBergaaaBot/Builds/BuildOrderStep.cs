using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SC2APIProtocol;

namespace Bot.Builds
{
    public class BuildOrderStep
    {
        public BuildOrderType Type;
        public uint Unit;
        public int Qty;
        public int Upgrade;
        public int Ability;
        public bool Completed;

        public BuildOrderStep(uint unit, int qty)
        {
            this.Type = BuildOrderType.Unit;
            this.Unit = unit;
            this.Qty = qty;
        }


        public BuildOrderStep(int upgrade, bool completed)
        {
            this.Type = BuildOrderType.Upgrade;
            this.Upgrade = upgrade;
            this.Completed = completed;
        }
    }

    public enum BuildOrderType
    {
        Unit,
        Upgrade,
        Building
    }
}
