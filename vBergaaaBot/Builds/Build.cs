using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SC2APIProtocol;

namespace Bot.Builds
{
    public abstract class Build
    {
        public abstract string Name();
        public abstract List<BuildOrderStep> BuildOrder();
        public abstract void OnStart(VBergaaaBot vBergaaaBot);
        public abstract IEnumerable<SC2APIProtocol.Action> OnFrame(VBergaaaBot vBergaaaBot);
    }
}
