
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

        // constructor
        public VBergaaaBot(Race race)
        {
            MyRace = race;
            Bot = this;
            InternalData = new InternalData();
        }

        public void OnStart(ResponseGameInfo gameInfo, ResponseData data, ResponseObservation observation, uint playerID)
        {
            GameInfo = gameInfo;
            Observation = observation;
            Data = data;
            PlayerID = playerID;
            MapInformation.Analyse(this);


            //Build = new Builds.FirstBuild();
            if (MyRace == Race.Zerg)
                Build = new Roach_All_In();
            Build.OnStart(this);
            BuildOrder = Build.GetBuildOrder();
            MaxOutComp = Build.MaxOutComp();

            
        }

        public IEnumerable<Action> OnFrame(ResponseObservation observation)
        {
            // refresh state and cache inportant info
            Observation = observation;
            InternalData.OnFrame(observation.Observation);
            //InternalData.PrintUnitState();
            Controller.OpenFrame();
            BuildOrderStep nextStep = BotHelper.GetNextStep(BuildOrder, MaxOutComp, this);
            BotHelper.ExecuteBuildOrderStep(nextStep, this);
            Build.OnFrame(this);
            return Controller.CloseFrame();
        }
    }
}

