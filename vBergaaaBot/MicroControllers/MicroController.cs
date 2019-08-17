using System.Collections.Generic;
using System.Linq;

namespace vBergaaaBot.MicroControllers
{
    public abstract class MicroController
    {
        internal bool Active;
        internal List<Agent> AssignedAgents = new List<Agent>();

        public abstract void OnFrame();
        public abstract void CheckRequirements();

        public void Activate()
        {
            Active = true;
        }

        public virtual void Deactivate()
        {
            Active = false;
        }

        public void AssignAgents(Agent agent)
        {
            AssignedAgents.Add(agent);
        }
        public void AssignAgents(HashSet<Agent> agents)
        {
            AssignedAgents.AddRange(agents);
        }
        public void RemoveAgents(Agent agent)
        {
            if (AssignedAgents.Contains(agent))
                AssignedAgents.Remove(agent);
        }
        public void RemoveAgents(HashSet<Agent> agents)
        {
            foreach (Agent a in agents)
                AssignedAgents.Remove(a);
        }

        public void RemoveDeadAgents()
        {
            if (VBot.Bot.Observation.Observation.RawData.Event != null)
            {
                foreach (ulong tag in VBot.Bot.Observation.Observation.RawData.Event.DeadUnits)
                {
                    foreach (var unit in AssignedAgents)
                        if (unit.Unit.Tag == tag)
                        {
                            AssignedAgents.Remove(unit);
                            break;
                        }
                }
            }
            
        }
    }
}
