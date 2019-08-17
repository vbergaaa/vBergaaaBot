using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vBergaaaBot.Helpers;

namespace vBergaaaBot.Tasks
{
    public class MacroTask : Task
    {
        Agent FromAgent;
        uint UnitType = 0;
        int UpgradeType = 0;

        // Constructors
        /// <summary>
        /// this constructor is for the creation of an upgrade
        /// </summary>
        /// <param name="upgradeId">upgrade id of the desired upgrade</param>
        internal MacroTask(int upgradeId)
        {
            UpgradeType = upgradeId;
        }

        /// <summary>
        /// this constructor is for the creation of a unit
        /// </summary>
        /// <param name="unitId">unitId of the desired unit</param>
        internal MacroTask(uint unitId)
        {
            UnitType = unitId;
        }

        // Activators
        /// <summary>
        /// Calling this method attempts to add a task to build the desired unit to the task manager
        /// </summary>
        /// <param name="unitId">unit type of desired unit</param>
        public static void MakeUnit(uint unitId)
        {
            if (Controller.CanMakeUnit(unitId))
            {
                Task t = new MacroTask(unitId);
                t.Commit();
            }
        }

        /// <summary>
        /// Calling this method attempts to add a task to research the desired upgrade to the task manager
        /// </summary>
        /// <param name="upgradeId">upgrade type to research</param>a
        public static void ResearchUpgrade(int upgradeId)
        {
            if (Controller.CanMakeUpgrade(upgradeId))
            {
                Task t = new MacroTask(upgradeId);
                t.Commit();
            }
        }

        // Methods
        /// <summary>
        /// checks if it is possible to build/research unit/upgrade, then gets an agent and orders it to complete the task.
        /// TODO: ADD RESOURCE MANAGEMENT (+= reservedminerals etc.)
        /// </summary>
        public override void OnFrame()
        {
            if (UnitType != 0) // make a unit
            {
                if (Controller.CanMakeUnit(UnitType) && FromAgent == null)
                {
                    // set the from type and execute the task
                    if (MorphHelper.MorpSteps.ContainsKey(UnitType))
                    {
                        FromAgent = Controller.GetAvailableAgent(MorphHelper.GetPreMorphType(UnitType));
                        FromAgent.Order(Units.GetAbilityId(UnitType)); // acts as execute()
                        Clear(); // only dismissed as it is morph type and eggs are useless and don't need to be busy
                    }
                    else if (TrainHelper.TrainSteps.ContainsKey(UnitType))
                    {
                        FromAgent = Controller.GetAvailableAgent(TrainHelper.GetTrainingBuildingTypes(UnitType));
                        FromAgent.Busy = true;
                        FromAgent.Order(Units.GetAbilityId(UnitType)); // acts as execute()
                    }
                    else
                    {
                        FromAgent = Controller.GetAvailableAgent(Units.Workers);
                        FromAgent.Busy = true;
                        Controller.BuildStructure(FromAgent, UnitType); // acts as Execute()
                    }
                }
                else if (FromAgent != null)
                {
                    if (FromAgent.Unit.Orders.Count == 0)
                    {
                        // agent is idle. clear it
                        FromAgent.Busy = false;
                        Clear();
                    }
                }
                else
                {
                    Clear();
                }
            }
            else
            {
                if (Controller.CanMakeUpgrade(UpgradeType) && FromAgent == null)
                {
                    FromAgent = Controller.GetAvailableAgent(UpgradeHelper.GetUpgradeBuildingTypes(UpgradeType));
                    FromAgent.Busy = true;
                    FromAgent.Order(Upgrades.GetAbilityId(UpgradeType));
                }
                else if (FromAgent != null)
                {
                    if (FromAgent.Unit.Orders.Count == 0) {
                        FromAgent.Busy = false;
                        Clear();
                    }
                }
                else
                {
                    Clear();
                }
            }
            Clear();
        }
    }
}
