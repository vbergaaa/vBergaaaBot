using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vBergaaaBot.Tasks;

namespace vBergaaaBot.Managers
{
    public class TaskManager : Manager
    {
        private List<Task> tasks = new List<Task>();

        public override void OnFrame()
        {
            foreach (Task t in tasks)
                t.OnFrame();

            // remove tasks that are done
            var tasksToRemove = new List<Task>();
            foreach (Task t in tasks)
                if (t.Discard)
                    tasksToRemove.Add(t);

            foreach (Task t in tasksToRemove)
                RemoveTask(t);
        }
        public void AddTask(Task task)
        {
            tasks.Add(task);
        }

        public void RemoveTask(Task task)
        {
            tasks.Remove(task);
        }
    }
    
}
