using SC2APIProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vBergaaaBot.Managers
{
    public class AttackManager : Manager
    {
        private AttackType type;
        private int attackArmySize;
        private int retreatArmySize;
        private bool attack = false;
        private Point2D rallyPoint;
        private Point2D target;

        public AttackManager(AttackType type, int atkSup, int retreatSup)
        {
            this.type = type;
            attackArmySize = atkSup;
            retreatArmySize = retreatSup;
        }
        public AttackManager(int atkSup)
        {
            this.type = AttackType.AllIn;
            attackArmySize = atkSup;
            retreatArmySize = 0;
        }

        public override void OnFrame(VBergaaaBot bot)
        {
            if (bot.Observation.Observation.GameLoop % 10 != 0)
                return;

            var army = Controller.GetUnits(Units.ArmyUnits);
            var armySupply = army.Sum(u => u.supply);
            if (armySupply >= attackArmySize)
                attack = true;
            if (armySupply <= retreatArmySize)
                attack = false;
            if (!attack)
            {
                rallyPoint = new Point2D { X = 128, Y = 128 };
                Controller.Attack(army, rallyPoint);
            }
            else
            {
                if (Controller.GetUnits(Units.Structures, alliance: Alliance.Enemy).Count > 0)
                    target = Controller.GetUnits(Units.Structures, alliance: Alliance.Enemy)[0].Pos;
                else
                    target = bot.MapInformation.EnemyStartLocations[0];
                
                Controller.Attack(army, target);
            }
        }
    }

    public enum AttackType
    {
        Timing,
        AllIn
    }
}
