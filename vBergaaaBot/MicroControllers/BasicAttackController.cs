using SC2APIProtocol;
using System.Collections.Generic;
using System.Linq;
using vBergaaaBot.Helpers;

namespace vBergaaaBot.MicroControllers
{
    public class AttackController : MicroController
    {
        private Point2D targetLocation = VBot.Bot.Map.TargetAttackLocation;
        private uint lastAttackFrame= 0;
        private int supplyToAttack;
        private int supplyToRetreat;
        public override void CheckRequirements()
        {
            if (!Active && Controller.supplyOf(Units.ArmyUnits) >= supplyToAttack)
                Activate();
            else if (Active && Controller.supplyOf(Units.ArmyUnits) <= supplyToRetreat)
                Deactivate();
            if (VBot.Bot.Map.TargetAttackLocation == VBot.Bot.Map.EnemyStartLocations[0] 
                && Sc2Util.ReadTile(VBot.Bot.Observation.Observation.RawData.MapState.Visibility, VBot.Bot.Map.TargetAttackLocation))
                Deactivate();
            
        }

        public AttackController(int atkSup, int retreatSup)
        {
            supplyToAttack = atkSup;
            supplyToRetreat = retreatSup;
        }

        public override void OnFrame()
        {
            CheckRequirements();
            if (Active)
            {
                if (VBot.Bot.Observation.Observation.GameLoop - 32 > lastAttackFrame)
                {
                    foreach (var agent in Controller.GetAgents(Units.ArmyUnits).Where(a => !a.Busy))
                        if (!AssignedAgents.Contains(agent))
                            AssignedAgents.Add(agent);

                    var armyTags = AssignedAgents.Select(a => a.Unit.Tag);
                    targetLocation = VBot.Bot.Map.TargetAttackLocation;
                    var unitCommand = new ActionRawUnitCommand();
                    unitCommand.AbilityId = (int)Abilities.ATTACK;
                    unitCommand.TargetWorldSpacePos = targetLocation;
                    unitCommand.UnitTags.AddRange(armyTags);
                    lastAttackFrame = VBot.Bot.Observation.Observation.GameLoop;
                    VBot.Bot.AddAction(unitCommand);
                }
            }
        }
    }
}
