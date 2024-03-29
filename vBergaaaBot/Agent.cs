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
        public uint LastCommandFrame;
        
        public Agent(SC2APIProtocol.Unit unit) {
            this.Unit = unit;
        }

        public void Order(uint abilityId)
        {
            Order(abilityId, null);
        }
        public void Order(uint abilityId, ulong targetTag)
        {
            // ignore similar orders
            if (Unit.Orders.Count > 0)
                if (Unit.Orders[0].AbilityId == abilityId)
                    return;

            // update resources if macro task
            if (Abilities.CreatesUnit.ContainsKey(abilityId)) 
            {
                var unitInfo = VBot.Bot.Data.Units[(int)Abilities.CreatesUnit[abilityId]];
                VBot.Bot.ReservedMinerals += (int)unitInfo.MineralCost;
                VBot.Bot.ReservedGas += (int)unitInfo.VespeneCost;
                VBot.Bot.ReservedSupply += (int)unitInfo.FoodRequired - (int)unitInfo.FoodProvided;
            }

            Command = new ActionRawUnitCommand
            {
                AbilityId = (int)abilityId,
                TargetUnitTag = targetTag
            };
            Command.UnitTags.Add(Unit.Tag);
            VBot.Bot.AddAction(Command);
            LastCommandFrame = VBot.Bot.Observation.Observation.GameLoop;
        }
        public void Order(uint abilityId, Point2D position)
        {

            // ignore similar orders
            if (Unit.Orders.Count > 0 )
                if (Unit.Orders[0].AbilityId == abilityId)
                    return;

            // update resources if macro task
            if (Abilities.CreatesUnit.ContainsKey(abilityId))
            {
                var unitInfo = VBot.Bot.Data.Units[(int)Abilities.CreatesUnit[abilityId]];
                VBot.Bot.ReservedMinerals += (int)unitInfo.MineralCost;
                VBot.Bot.ReservedGas += (int)unitInfo.VespeneCost;
                VBot.Bot.ReservedSupply += (int)unitInfo.FoodRequired - (int)unitInfo.FoodProvided;
            }

            Command = new ActionRawUnitCommand
            {
                AbilityId = (int)abilityId,
                TargetWorldSpacePos = position
            };
            Command.UnitTags.Add(Unit.Tag);
            VBot.Bot.AddAction(Command);
            LastCommandFrame = VBot.Bot.Observation.Observation.GameLoop;
        }

        public void Update(Unit unit)
        {
            Unit = unit;
        }
    }
}