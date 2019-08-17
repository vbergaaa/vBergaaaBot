

//using vBergaaaBot.Helpers;
//using vBergaaaBot.Managers;

//namespace vBergaaaBot.Tasks
//{
//    class TrainTask : Task
//    {
//        public TrainTask(uint unitType, ulong fromStruct)
//        {
//            throw new System.NotImplementedException();
//        }

//        public TrainTask(uint unitType)
//        {

//            this.ToType = unitType;
//        }

//        public override void OnFrame()
//        {
//            if (FromAgent != null && FromAgent.Busy == true) // this should only occur while training is in progress
//            {
//                if (FromAgent.Unit.Orders.Count == 0)
//                {
//                    Clear();
//                    FromAgent.Command = null;
//                    FromAgent.Busy = false;
//                }
//                return;
//            }

//            if (Controller.CanMakeUnit(ToType))
//            {
//                // start the training
//                FromAgent = Controller.GetAvailableAgent(TrainHelper.GetTrainingBuildingTypes(ToType));
//                if (FromAgent == null)
//                    Clear();
//                FromAgent.Order(Units.GetAbilityId(ToType));
//                FromAgent.Busy = true;
//            }
//            // todo: clear task if building gets destroyed ?? no effect just dont want floating tasks
//        }

//        public static void Train(uint unitType)
//        {
//            if (!Controller.CanMakeUnit(unitType))
//                return;

//            TrainTask t = new TrainTask(unitType);
//            VBot.Bot.TaskManager.AddTask(t);
//        }
//        public static void Train(uint UnitType, ulong fromStruct)
//        {
//            TrainTask t = new TrainTask(UnitType, fromStruct);
//        }
//    }
//}
