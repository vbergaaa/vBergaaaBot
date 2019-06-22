
//using System.Collections.Generic;
//using SC2APIProtocol;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Bot
//{
//    public class TestBot : Bot
//    {
//        //
//        public uint PlayerID;
//        private bool DroneTime;
//        private bool MakeHatch;

//        // analyse the map
//        MapInformation mapInfo = new MapInformation();
        

//        //the following will be called every frame
//        //you can increase the amount of frames that get processed for each step at once in Wrapper/GameConnection.cs: stepSize  
//        public IEnumerable<Action> OnFrame()
//        {
//            Controller.OpenFrame();

//            if (Controller.frame == 10)
//            {
//                mapInfo.Analyse(this);
//                Controller.Chat("gl hf");
//            }

//            if (Controller.frame == 11)
//            {
//                var army = Controller.GetUnits(Units.Workers);
               
//                if (Controller.enemyLocations.Count > 0)
//                    Controller.Attack(army, mapInfo.ExpansionLocation);

//            }

//            //// decide what happens next
//            //Assess();
//            //var larvae = Controller.GetUnits(Units.LARVA);
//            //// start debug session
//            //if (Controller.frame == 0)
//            //{
//            //    mapInfo.Analyse();
//            //    DroneTime = true;
//            //    MakeHatch = false;
//            //    Logger.Info("vBergaaBot");
//            //    Logger.Info("--------------------------------------");
//            //    Logger.Info("Map: {0}", Controller.gameInfo.MapName);
//            //    Logger.Info("--------------------------------------");
//            //}

//            //// say glhf
//            //if (Controller.frame == Controller.SecsToFrames(1))
//            //    Controller.Chat("gl hf");


//            //// say gg
//            //var structures = Controller.GetUnits(Units.Structures);
//            //if (structures.Count == 1)
//            //{
//            //    //last building                
//            //    if (structures[0].integrity < 0.4) //being attacked or burning down                 
//            //        if (!Controller.chatLog.Contains("gg"))
//            //            Controller.Chat("gg");
//            //}

//            ////keep on building overlords if supply is tight
//            //larvae = Controller.GetUnits(Units.LARVA);
//            //foreach (var larva in larvae)
//            //{
//            //    if (Controller.maxSupply - Controller.currentSupply <= 5)
//            //        if (Controller.CanConstruct(Units.OVERLORD))
//            //            if (Controller.GetPendingCount(Units.OVERLORD) == 0)
//            //                larva.Train(Units.OVERLORD);
//            //}

//            //// script to build drone
//            //larvae = Controller.GetUnits(Units.LARVA);
//            //foreach (var larva in larvae)
//            //{
//            //    if (Controller.CanConstruct(Units.DRONE) && DroneTime)
//            //        larva.Train(Units.DRONE);
//            //}

//            //// script to build a new hatch
//            ////if (MakeHatch)
//            ////    if (Controller.CanConstruct(Units.HATCHERY))
//            ////        Controller.Construct(Units.HATCHERY,mapInfo.ExpansionLocation);
//            //if (MakeHatch)
//            //{
//            //    Controller.Move(Controller.GetAvailableWorker(), mapInfo.ExpansionLocation);
//            //}







//            ////distribute workers optimally every 10 frames
//            //if (Controller.frame % 10 == 0)
//            //    Controller.DistributeWorkers();



//            //////build up to 4 barracks at once
//            ////if (Controller.CanConstruct(Units.BARRACKS))
//            ////    if (Controller.GetTotalCount(Units.BARRACKS) < 4)
//            ////        Controller.Construct(Units.BARRACKS);

//            //////train marine
//            ////foreach (var barracks in Controller.GetUnits(Units.BARRACKS, onlyCompleted: true))
//            ////{
//            ////    if (Controller.CanConstruct(Units.MARINE))
//            ////        barracks.Train(Units.MARINE);
//            ////}

//            //////attack when we have enough units
//            ////var army = Controller.GetUnits(Units.ArmyUnits);
//            ////if (army.Count > 20)
//            ////{
//            ////    if (Controller.enemyLocations.Count > 0)
//            ////        Controller.Attack(army, Controller.enemyLocations[0]);
//            ////}

//            return Controller.CloseFrame();
//        }

//        public void OnStart(uint playerID)
//        {
//            PlayerID = playerID;
//        }
//        public void Assess()
//        {
//            var totalDrones = Controller.GetUnits(Units.DRONE).Count + Controller.GetPendingCount(Units.DRONE);
//            var totalOverlords = Controller.GetUnits(Units.OVERLORD).Count + Controller.GetPendingCount(Units.OVERLORD);
//            var totalHatcheries = Controller.GetUnits(Units.ResourceCenters).Count + Controller.GetPendingCount(Units.HATCHERY);

//            if (totalDrones == 13 && totalOverlords == 1)
//                DroneTime = false;

//            if (totalDrones == 13 && totalOverlords == 2)
//                DroneTime = true;

//            if (totalDrones == 17 && totalHatcheries == 1)
//            {
//                DroneTime = false;
//                MakeHatch = true;
//            }

//            if (totalDrones == 16 && totalHatcheries >= 2)
//            {
//                DroneTime = true;
//                MakeHatch = false;
//            }


//        }
//    }
//}

