using SC2APIProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vBergaaaBot.Tasks
{
    /// <summary>
    /// A Task is a proposal for an action to be sent as a request to the api.
    /// A task is generally created by a micro controller, and is stored in the task manager.
    /// 
    /// each onframe, the task will try to execute. once a task it completed, it should remove itself from the task manager
    /// </summary>
    public abstract class Task
    {
        // Properties
        /// <summary>
        /// Setting discard to true will remove the task from the task manager on the next onframe.
        /// </summary>
        public bool Discard = false;
        public int Id { get; set; }

        public ActionRawUnitCommand Command = new ActionRawUnitCommand();

        // Instance Methods
        /// <summary>
        /// The OnFrame method should double check logic to test if the action is possible
        /// (e.g. ensure minerals are still available or if another task used them)
        /// then it should execute the task
        /// </summary>
        public abstract void OnFrame();

        /// <summary>
        /// Committing a task adds it to the task manager.
        /// Once a task is committed, its OnFrame() will trigger in the task manager until it gets cleared.
        /// </summary>
        public virtual void Commit()
        {
            VBot.Bot.TaskManager.AddTask(this);
        }

        /// <summary>
        /// Clearing a task will cause the task to be removed from the bot, ready for garbage collection
        /// The task will be cleared after it's OnFrame() is completed.
        /// </summary>
        public virtual void Clear()
        {
            Discard = true;
        }
        
        /// <summary>
        /// Executing a task will push the Command Property into the VBot Action List to be requested by the api
        /// </summary>
        public virtual void Execute()
        {
            VBot.Bot.AddAction(Command);
        }
    }
}
