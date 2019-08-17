using System.Collections.Generic;

namespace vBergaaaBot.MicroControllers
{
    public class InjectController : MicroController
    {
        internal List<ulong> bases = new List<ulong>();
        public override void OnFrame()
        {
            CheckRequirements();
            if (!Active)
                return;

            if (VBot.Bot.StateManager.GetCompletedCount(Units.HATCHERY) > bases.Count)
                foreach (Agent b in Controller.GetAgents(Units.HATCHERY))
                    if (b.Unit.BuildProgress > 0.9999 && !bases.Contains(b.Unit.Tag))
                        bases.Add(b.Unit.Tag);

            if (AssignedAgents.Count < Controller.GetCompletedCount(Units.HATCHERY))
            {

                //try to assign a new queen 
                if (Controller.GetAvailableAgent(Units.QUEEN) != null)
                {
                    Agent q = Controller.GetAvailableAgent(Units.QUEEN);
                    q.Busy = true;
                    AssignAgents(q);
                }
            }

            foreach(Agent q in AssignedAgents)
            {
                int index = AssignedAgents.IndexOf(q);
                if (q.Unit.Energy >= 25)
                    q.Order(Abilities.INJECT_LARVA, bases[index]);

            }
        }

        public override void CheckRequirements()
        {
            if (!Active && VBot.Bot.StateManager.GetCompletedCount(Units.QUEEN) > 0)
                Activate();
            
            if (Active && VBot.Bot.StateManager.GetCompletedCount(Units.QUEEN) == 0)
                Deactivate();
        }
    }
}
