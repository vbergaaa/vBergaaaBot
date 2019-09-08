using System.Collections.Generic;
using vBergaaaBot.Builds;
using vBergaaaBot.Managers;
using vBergaaaBot.Map;
using vBergaaaBot.MicroControllers;
using vBergaaaBot.Tasks;
using SC2APIProtocol;
using vBergaaaBot.Builds.ZergBuilds;
using System.Diagnostics;
using System.Threading;
using vBergaaaBot.Helpers;

namespace vBergaaaBot {
    internal class VBot : Bot
    {
        bool test = false;
        bool realTime = false;
        Stopwatch sw;
        private List<Action> actions;
        // Properties
        public ResponseGameInfo GameInfo;
        public ResponseData Data;
        public ResponseObservation Observation;
        public int OppenentsPlayerId;
        public int PlayerId;
        public static VBot Bot;
        public MapAnalyser Map;
        public TaskManager TaskManager;
        public StateManager StateManager;
        public MicroManager MicroManager = new MicroManager();
        public EnemyStrategyManager EnemyStrategyManager = new EnemyStrategyManager();
        public Build Build;
        public Race Race;
        public Race OpponentsRace;

        public int ReservedMinerals = 0;
        public int ReservedGas = 0;
        public int ReservedSupply = 0;
        public int PendingSupply = 0;


        public VBot()
        {
            Bot = this;
        }


        public void OnStart(ResponseGameInfo gameInfo, ResponseData data, ResponseObservation observation, uint playerId)
        {
            if (realTime)
            {
                sw = new Stopwatch();
                sw.Start(); 
            }
            GameInfo = gameInfo;
            Data = data;
            Observation = observation;
            PlayerId = (int)playerId - 1;
            OppenentsPlayerId = PlayerId == 1 ? 0 : 1;
            Race = GameInfo.PlayerInfo[PlayerId].RaceActual;
            OpponentsRace = GameInfo.PlayerInfo[OppenentsPlayerId].RaceActual;
            Units.LoadData();
            StateManager = new StateManager();
            StateManager.OnFrame();

            Map = new MapAnalyser();
            Map.Analyse(this);
            Build = new RoachAllIn();
            if (test)
                Build = new Testzerg();
            Build.OnStart();

            TaskManager = new TaskManager();
            
        }
        public IEnumerable<Action> OnFrame(ResponseObservation observation)
        {
            Observation = observation;
            actions = new List<Action>();

            ReservedGas = 0;
            ReservedMinerals = 0;
            ReservedSupply = 0;
            PendingSupply = 0;

            StateManager.OnFrame();
            EnemyStrategyManager.OnFrame();
            Build.OnFrame();
            MicroManager.OnFrame();
            TaskManager.OnFrame();

            if (Observation.Observation.GameLoop % 32 == 0)
                Controller.DistributeWorkers();

            if (realTime)
            {
                long delayTime = 30 - sw.ElapsedMilliseconds;
                if (delayTime > 0)
                    Thread.Sleep((int)delayTime);
                sw.Restart();
            }

            return actions;
        }

        public void AddAction(Action action)
        {
            actions.Add(action);
        }
        public void AddAction(ActionRaw action)
        {
            Action a = new Action();
            a.ActionRaw = action;
            AddAction(a);
        }
        public void AddAction(ActionRawUnitCommand action)
        {
            var a = new Action();
            a.ActionRaw = new ActionRaw();
            a.ActionRaw.UnitCommand = action;
            AddAction(a);
        }

        public int Minerals()
        {
            return (int)Observation.Observation.PlayerCommon.Minerals - ReservedMinerals;
        }
        public int Gas()
        {
            return (int)Observation.Observation.PlayerCommon.Vespene - ReservedGas;
        }
        public int AvailableSupply()
        {
            return (int)(Observation.Observation.PlayerCommon.FoodCap - Observation.Observation.PlayerCommon.FoodUsed - ReservedSupply);
        }
        public int GetAvaibleSupplyPending()
        {
            return (int)(Observation.Observation.PlayerCommon.FoodCap + PendingSupply - Observation.Observation.PlayerCommon.FoodUsed - ReservedSupply);
        }
        
    }
}