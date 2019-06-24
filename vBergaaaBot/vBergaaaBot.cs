
using System.Collections.Generic;
using SC2APIProtocol;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vBergaaaBot.Builds;
using vBergaaaBot.Helpers;
using vBergaaaBot.Entity;

namespace vBergaaaBot
{
    public class VBergaaaBot : Bot
    {
        // bot information
        public GameConnection GameConnection;
        public ResponseData Data;
        public Race MyRace;
        public Race EnemyRace;
        public ResponseGameInfo GameInfo;
        public ResponseObservation Observation;
        public uint PlayerID;
        public MapInformation MapInformation = new MapInformation();
        public static VBergaaaBot Bot;
        public Build Build;
        public InternalData InternalData;
        List<BuildOrderStep> BuildOrder;
        List<BuildOrderStep> MaxOutComp;

        public int ReservedMinerals { get; set; }
        public int ReservedGas { get; set; }
        public int ReservedSupply { get; set; }

        public int Minerals { get; set; }
        public int Vespene { get; set; }
        public int AvailableSupply { get; set; }

        // make a manager to spead creep? (build depending?)

        // constructor
        public VBergaaaBot(Race race)
        {
            MyRace = race;
            Bot = this;
            InternalData = new InternalData(this);
        }

        public void OnStart(ResponseGameInfo gameInfo, ResponseData data, ResponseObservation observation, uint playerID)
        {
            GameInfo = gameInfo;

            Observation = observation;

            Data = data;
            PlayerID = playerID;

            //Build = new Builds.FirstBuild();
            Build = new Roach_All_In();
            BuildOrder = Build.GetBuildOrder();
            MaxOutComp = Build.MaxOutComp();

            // load data might essential
            MapInformation.Analyse(this);
        }

        public IEnumerable<Action> OnFrame(ResponseObservation observation)
        {
            Observation = observation;
            RefreshResources();
            Controller.OpenFrame();
            BuildOrderStep nextStep = BotHelper.GetNextStep(BuildOrder, MaxOutComp, this);
            BotHelper.ExecuteBuildOrderStep(nextStep, this);
            if (ReservedMinerals > 0)
                ReservedMinerals = ReservedMinerals;
            Build.OnFrame(this);
            return Controller.CloseFrame();
        }

        public void RefreshResources()
        {
            Minerals = (int)Observation.Observation.PlayerCommon.Minerals - ReservedMinerals;
            Vespene = (int)Observation.Observation.PlayerCommon.Vespene - ReservedGas;
            AvailableSupply = (int)Observation.Observation.PlayerCommon.FoodCap - (int)Observation.Observation.PlayerCommon.FoodUsed - ReservedSupply;
        }
    }
}

