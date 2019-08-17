using vBergaaaBot.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vBergaaaBot.MicroControllers
{
    public class CreepController : MicroController
    {
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
                if (AssignedAgents.Count==0)
                {
                    Agent q = Controller.GetAvailableAgent(Units.QUEEN);
                    AssignedAgents.Add(q);
                    q.Busy = true;

                }

                if (AssignedAgents[0].Unit.Energy >= 25 && AssignedAgents[0].Unit.Orders.Count == 0)
                    AssignedAgents[0].Order(Abilities.SPREAD_CREEP_QUEEN, Controller.GetTumorLocation(Sc2Util.To2D(AssignedAgents[0].Unit.Pos),12));
                if (VBot.Bot.Observation.Observation.GameLoop % 320 == 0)
                    foreach (var tumor in Controller.GetAgents(Units.CREEP_TUMOR_BURROWED))
                    {
                        tumor.Order(Abilities.SPREAD_CREEP_TUMOR, Controller.GetTumorLocation(Sc2Util.To2D(tumor.Unit.Pos),7));
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
