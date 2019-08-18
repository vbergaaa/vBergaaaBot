using SC2APIProtocol;
using System.Collections.Generic;
using vBergaaaBot.Helpers;
using vBergaaaBot.Managers;

namespace vBergaaaBot.Managers
{
    public class EnemyStrategyManager : Manager
    {
        List<Unit> KnownBuildings = new List<Unit>();
        List<Unit> KnownArmy = new List<Unit>();
        

        public override void OnFrame()
        {
            KnownBuildings = new List<Unit>();
            KnownArmy = new List<Unit>();
            var enemyUnits = Controller.GetEnemyUnits();
            foreach (Unit u in enemyUnits)
            {
                if (Units.Structures.Contains(u.UnitType))
                    KnownBuildings.Add(u);
                else
                    KnownArmy.Add(u);
            }
                

            if (KnownBuildings.Count > 0)
            {
                VBot.Bot.Map.TargetAttackLocation = Sc2Util.To2D(KnownBuildings[0].Pos);
            }
            else if (KnownArmy.Count > 2)  // set to 2 to avoid chasing single workers that are scouting
            {
                VBot.Bot.Map.TargetAttackLocation = Sc2Util.To2D(KnownArmy[0].Pos);
            }
            else
            {
                if (VBot.Bot.Map.TargetAttackLocation != VBot.Bot.Map.EnemyStartLocations[0])
                    VBot.Bot.Map.TargetAttackLocation = VBot.Bot.Map.EnemyStartLocations[0];
            }
        }
    }
}
