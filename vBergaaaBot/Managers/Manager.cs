using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vBergaaaBot.Managers
{
    // A Manager is an object that takes care of specific micro tasks
    // Each manager will focus on different things, and is activated by calling .Enable() in the specific build file
    public abstract class Manager
    {
        public abstract void OnFrame(VBergaaaBot bot);
    }
}
