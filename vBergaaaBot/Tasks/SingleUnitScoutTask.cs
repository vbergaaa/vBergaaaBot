using SC2APIProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vBergaaaBot.Helpers;

namespace vBergaaaBot.Tasks
{
    public class SingleUnitScoutTask : Task
    {
        private Agent scoutUnit;
        private Point2D point2D;

        public SingleUnitScoutTask(Agent unit, Point2D location)
        {
            if ((unit.Unit.Orders.Count > 0 &&
                unit.Unit.Orders[0].AbilityId == Abilities.MOVE &&
                Math.Abs(unit.Unit.Orders[0].TargetWorldSpacePos.X - location.X) < 1 &&
                Math.Abs(unit.Unit.Orders[0].TargetWorldSpacePos.Y - location.Y) < 1)
                ||
                (Math.Abs(unit.Unit.Pos.X - location.X) < 1 &&
                Math.Abs(unit.Unit.Pos.Y - location.Y) < 1))
                return;
            else
            {
                this.scoutUnit = unit;
                this.point2D = location;
                Commit();
            }
        }

        public override void OnFrame()
        {
            if ((scoutUnit.Unit.Orders.Count > 0 &&
                scoutUnit.Unit.Orders[0].AbilityId == Abilities.MOVE &&
                Math.Abs(scoutUnit.Unit.Orders[0].TargetWorldSpacePos.X - point2D.X) < 1 &&
                Math.Abs(scoutUnit.Unit.Orders[0].TargetWorldSpacePos.Y - point2D.Y) < 1)
                || (Math.Abs(scoutUnit.Unit.Pos.X - point2D.X) < 1 &&
                Math.Abs(scoutUnit.Unit.Pos.Y - point2D.Y) < 1))
                Clear();
            else
            {
                Command.AbilityId = (int)Abilities.MOVE;
                Command.TargetWorldSpacePos = point2D;
                Command.UnitTags.Add(scoutUnit.Unit.Tag);
                Execute();
            }
        }
    }
}
