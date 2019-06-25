using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vBergaaaBot.Managers
{
    public class GeneralBaseManager : Manager
    {
        private int gasCount;

        public override void OnFrame(VBergaaaBot bot)
        {
            if (bot.Observation.Observation.GameLoop % 10 == 1)
                Controller.DistributeWorkers();
            if (gasCount < Controller.GetUnits(Units.EXTRACTOR, onlyCompleted: true).Count)
            {
                Controller.PrioritiseGas();
                gasCount = Controller.GetUnits(Units.EXTRACTOR, onlyCompleted: true).Count;
            }
        }
    }
}
