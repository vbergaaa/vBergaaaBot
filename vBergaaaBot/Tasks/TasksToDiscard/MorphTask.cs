//using vBergaaaBot.Helpers;

//namespace vBergaaaBot.Tasks
//{
//    public class MorphTask : Task
//    {
//        public override void OnFrame()
//        {
//            if (Controller.CanMakeUnit(ToType))
//            {
//                FromAgent = Controller.GetAvailableAgent(MorphHelper.GetPreMorphType(ToType));
//                FromAgent.Order(Units.GetAbilityId(ToType));
//            }
//            Clear();
//        }

//        public static void Morph(uint unit)
//        {
//            if (!Controller.CanMakeUnit(unit))
//                return;

//            MorphTask task = new MorphTask(unit);
//            VBot.Bot.TaskManager.AddTask(task);
//        }

//        public MorphTask(uint unit)
//        {
//            ToType = unit;
//        }
        
//    }
//}
