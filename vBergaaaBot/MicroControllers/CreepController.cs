using vBergaaaBot.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vBergaaaBot.Tasks;

namespace vBergaaaBot.MicroControllers
{
    public class CreepController : MicroController
    {
        SpreadCreepTask t = null;
        public override void CheckRequirements()
        {
            if (Controller.GetCompletedCount(Units.HATCHERY) + 1 <=
                Controller.GetCompletedCount(Units.QUEEN))
                Activate();
            else if (Active)
                Deactivate();
        }

        public override void OnFrame()
        {
            CheckRequirements();
            if (Active)
            {
                if (AssignedAgents.Count == 0)
                {
                    Agent q = Controller.GetAvailableAgent(Units.QUEEN);
                    AssignedAgents.Add(q);
                    q.Busy = true;
                    t = new SpreadCreepTask(q);
                }
            }
        }
        public override void Deactivate()
        {
            base.Deactivate();
            Agent q = null;
            if (AssignedAgents.Count > 0)
            {
                q = AssignedAgents[0];
                q.Busy = false;
                AssignedAgents.Remove(q);
            }
        }
    }
}
