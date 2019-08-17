using vBergaaaBot.MicroControllers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace vBergaaaBot.Managers
{
    public class MicroManager
    {
        public static List<MicroController> Controllers = new List<MicroController>();
        public void OnFrame()
        {
            foreach (MicroController c in Controllers)
            {
                c.RemoveDeadAgents();
                c.OnFrame();
            }
        }

        public static void AddController(MicroController microController)
        {
            Controllers.Add(microController);
        }
    }
}
