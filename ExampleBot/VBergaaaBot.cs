using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SC2API_CSharp;
using SC2APIProtocol;
using VBergaaaBot.MapInfo;
using VBergaaaBot.Controllers;
using VBergaaaBot.Agents;

namespace VBergaaaBot
{
    class VBergaaaBot : Bot
    {
        public static VBergaaaBot Bot;
        public ResponseObservation Observation { get; set; }
        public ResponseGameInfo GameInfo { get; set; }
        public ResponseData Data { get; set; }
        public uint PlayerId { get; set; }
        public MapAnalyzer MapAnalyzer { get; set; }

        public void OnStart(ResponseGameInfo gameInfo, ResponseData data, ResponseObservation observation, uint playerId)
        {
            Bot = this;
            PlayerId = playerId;
            Observation = observation;
            GameInfo = gameInfo;
            Data = data;
            MapAnalyzer = new MapAnalyzer(this);
            MapAnalyzer.PrintBaseLocationOrder();
        }
    
        public IEnumerable<SC2APIProtocol.Action> OnFrame(ResponseGameInfo gameInfo, ResponseObservation observation, uint playerId)
        {
            Observation = observation;
            Controller.Open(observation.Observation);

            if (Observation.Observation.PlayerCommon.Minerals >= 300)
                Controller.Build(Units.HATCHERY, MapAnalyzer.GetNextBaseLocation(this));
                //Controller.Move(Controller.GetUnits(Units.DRONE), MapAnalyzer.GetNextBaseLocation(this));

            return Controller.Close();
        }
        
        public void OnEnd(ResponseGameInfo gameInfo, ResponseObservation observation, uint playerId, Result result)
        { }
    }
}
