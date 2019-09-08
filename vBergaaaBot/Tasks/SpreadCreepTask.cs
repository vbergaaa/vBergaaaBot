using SC2APIProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vBergaaaBot.Helpers;

namespace vBergaaaBot.Tasks
{
    public class SpreadCreepTask : Task
    {

        private Agent Queen = null;
        private Point2D Location = null;
        private Dictionary<Agent,uint> ActiveTumors = new Dictionary<Agent, uint>();

        public override void OnFrame()
        {
            // queen logic
            if (Queen.Unit.Energy >= 25)
                Queen.Order(Abilities.SPREAD_CREEP_QUEEN, Controller.GetTumorLocation(Sc2Util.To2D(Queen.Unit.Pos), 12)); // Acts as EXECUTE statement

            // add active tumors to tumor list
            foreach (Agent t in Controller.GetAgents(Units.CREEP_TUMOR_BURROWED))
                if (t.Command == null)
                    if (!ActiveTumors.ContainsKey(t))
                        ActiveTumors.Add(t,VBot.Bot.Observation.Observation.GameLoop);

            List<Agent> removeTumors = new List<Agent>();
            // tumor logic
            foreach (var tumor in ActiveTumors)
            {
                if ((VBot.Bot.Observation.Observation.GameLoop - tumor.Value) < 400)
                    continue;
                else
                {
                    tumor.Key.Order(Abilities.SPREAD_CREEP_TUMOR, Controller.GetTumorLocation(Sc2Util.To2D(tumor.Key.Unit.Pos), 7)); // acts as EXECUTE statement
                    removeTumors.Add(tumor.Key);
                }
                   
            }

            foreach (var tumor in removeTumors)
                ActiveTumors.Remove(tumor);
        }

        public SpreadCreepTask(Agent agent)
        {
            Queen = agent;
            Commit();
        }
    }
}
