using System.Collections.Generic;
using System.Numerics;
using Google.Protobuf.Collections;
using SC2APIProtocol;

// ReSharper disable MemberCanBePrivate.Global

namespace vBergaaaBot {
    public class Agent {
        public SC2APIProtocol.Unit Unit;
        public ActionRawUnitCommand Command;
        public bool Busy;
        
        public Agent(SC2APIProtocol.Unit unit) {
            this.Unit = unit;
        }

        public void Order(uint abilityId)
        {
            Order(abilityId, null);
        }
        public void Order(uint abilityId, ulong targetTag)
        {
            Command = new ActionRawUnitCommand();
            Command.AbilityId = (int)abilityId;
            Command.TargetUnitTag = targetTag;
            Command.UnitTags.Add(Unit.Tag);
            VBot.Bot.AddAction(Command);
        }
        public void Order(uint abilityId, Point2D position)
        {
            // ignore similar orders
            if (Unit.Orders.Count > 0 )
                if (Unit.Orders[0].AbilityId == abilityId)
                    return;


            Command = new ActionRawUnitCommand();
            Command.AbilityId = (int)abilityId;
            Command.TargetWorldSpacePos = position;
            Command.UnitTags.Add(Unit.Tag);
            VBot.Bot.AddAction(Command);
        }

        public void Update(Unit unit)
        {
            Unit = unit;
        }
    }
}