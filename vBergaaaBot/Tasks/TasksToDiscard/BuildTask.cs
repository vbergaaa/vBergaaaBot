//using vBergaaaBot.Helpers;

//namespace vBergaaaBot.Tasks
//{
//    public class BuildTask : Task
//    {
//        public override void OnFrame()
//        {
//            if (Controller.CanMakeUnit(ToType) && FromAgent == null)
//            {
//                FromAgent = Controller.GetWorker();
//                if (FromAgent != null)
//                {
//                    Controller.BuildStructure(FromAgent, ToType);
//                    FromAgent.Busy = true;
//                }
//                else
//                {
//                    Clear();
//                }
//            }
//            else if (FromAgent != null)
//            {
//                if (FromAgent.Unit.Orders.Count > 0 && FromAgent.Unit.Orders[0].AbilityId == Units.GetAbilityId(ToType))
//                    return;
//                Clear();
//            }
//        }

//        public override void Clear()
//        {
//            base.Clear();
//            if (FromAgent != null)
//                FromAgent.Busy = false;
//        }
//        public static void Build(uint unit)
//        {
//            if (!Controller.CanMakeUnit(unit))
//                return;

//            BuildTask task = new BuildTask(unit);
//            VBot.Bot.TaskManager.AddTask(task);
//        }
//        public BuildTask(uint unit)
//        {
//            ToType = unit;
//        }
//    }
//}
