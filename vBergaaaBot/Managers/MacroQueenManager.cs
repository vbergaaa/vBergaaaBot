using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vBergaaaBot.Managers
{
    public class MacroQueenManager : Manager
    {
        public override void OnFrame(VBergaaaBot bot)
        {
            // simple inject pattern
            foreach (var queen in Controller.GetUnits(Units.QUEEN))
                if (queen.Energy >= 25)
                    Controller.Inject(queen);
        }
    }
}
