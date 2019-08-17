using System.Collections.Generic;
using SC2APIProtocol;

namespace vBergaaaBot {
    public interface Bot {
        IEnumerable<Action> OnFrame(ResponseObservation observation);
        void OnStart(ResponseGameInfo gameInfo, ResponseData data, ResponseObservation observation, uint playerId);
    }
}