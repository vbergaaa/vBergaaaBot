//using vBergaaaBot.Helpers;

//namespace vBergaaaBot.Tasks
//{
//    public class UpgradeTask : Task
//    {
//        public UpgradeTask(int upgradeId)
//        {
//            ToType = (uint)upgradeId;
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

//            if (Controller.CanMakeUpgrade((int)ToType))
//            {
//                // start the training
//                FromAgent = Controller.GetAvailableAgent(UpgradeHelper.GetUpgradeBuildingTypes((int)ToType));
//                if (FromAgent == null)
//                    Clear();
//                FromAgent.Order(Upgrades.GetAbilityId(ToType));
//                FromAgent.Busy = true;
//            }
//            // todo: clear task if building gets destroyed ?? no effect just dont want floating tasks
//        }

//        public static void Upgrade(int upgradeId)
//        {
//            if (!Controller.CanMakeUpgrade(upgradeId))
//                return;

//            UpgradeTask t = new UpgradeTask(upgradeId);
//            VBot.Bot.TaskManager.AddTask(t);
//        }
        
//    }
//}
