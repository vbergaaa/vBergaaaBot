using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SC2APIProtocol;
using vBergaaaBot.Entity;

namespace vBergaaaBot.Builds
{
    public abstract class Build
    {
        public abstract string Name();
        public abstract List<BuildOrderStep> GetBuildOrder();
        public abstract void OnStart(VBergaaaBot vBergaaaBot);
        public abstract void OnFrame(VBergaaaBot vBergaaaBot);
        public abstract List<BuildOrderStep> MaxOutComp();
    }
}
