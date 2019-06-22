using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Entity
{
    public class InternalData
    {
        public bool ZerglingSpeedUpgraded { get; set; }

        public InternalData()
        {
            ZerglingSpeedUpgraded = false;
        }
    }
}
