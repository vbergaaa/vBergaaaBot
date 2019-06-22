
using System.Collections.Generic;
using SC2APIProtocol;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot
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
        public Builds.Build Build;
        public Entity.InternalData GameMilestones = new Entity.InternalData();

        // make a manager to spead creep? (build depending?)

        // constructor
        public VBergaaaBot()
        {
            Bot = this;
        }

        public void OnStart(ResponseGameInfo gameInfo, ResponseData data, ResponseObservation observation, uint playerID)
        {
            GameInfo = gameInfo;

            Observation = observation;

            Data = data;
            PlayerID = playerID;

            //Build = new Builds.FirstBuild();
            Build = new Builds.Roach_All_In();

            // load data might essential

            MapInformation.Analyse(this);
        }

        public IEnumerable<Action> OnFrame(ResponseObservation observation)
        {
            Observation = observation;

            return Build.OnFrame(this);
        }
    }
}

