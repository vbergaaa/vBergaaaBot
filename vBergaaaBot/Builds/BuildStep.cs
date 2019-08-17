
using vBergaaaBot.Helpers;
using vBergaaaBot.Tasks;
using System;

namespace vBergaaaBot.Builds
{
    public class BuildStep
    {
        public uint UnitId { get; set; }
        public Condition Requirement;
        public delegate bool Condition();
        public Condition WaitFor = null;
        public int UpgradeId { get; set; }
        public int Quantity { get; set; }

        public BuildStep(int upgradeId)
        {
            UnitId = 0;
            Quantity = 0;
            UpgradeId = upgradeId;
            Requirement = () => { return !Controller.CheckUpgrade(upgradeId); };
        }

        public BuildStep(int upgradeId, Condition waitFor) : this(upgradeId)
        {
            WaitFor = waitFor;
        }

        public BuildStep(uint unitType, int qty)
        {
            UnitId = unitType;
            Quantity = qty;
            UpgradeId = 0;
            Requirement = () => { return Controller.GetTotalCount(unitType) < qty; };
        }

        public BuildStep(uint unitType, int qty, Condition waitFor) : this(unitType, qty)
        {
            WaitFor = waitFor;
        }

        /// <summary>
        /// Checks to see if there is enough
        /// </summary>
        /// <returns></returns>
        public bool CheckQty()
        {
            return Requirement();
        }

        /// <summary>
        /// Checks to see if the build step has a condition that needs to be met.
        /// </summary>
        /// <returns>returns false if the condition hasn't been met. returns true if there is no condition or the condition has been met</returns>
        public bool CheckWaitFor()
        {
            if (WaitFor != null)
                return WaitFor();
            return true;
        }
        public void CreateTask()
        {
            if (UnitId != 0)
            {
                MacroTask.MakeUnit(UnitId);
            }
            if (UpgradeId != 0)
            {
                MacroTask.ResearchUpgrade(UpgradeId);
            }
        }
    }
}
