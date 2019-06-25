using SC2APIProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vBergaaaBot;
using vBergaaaBot.Helpers;

namespace vBergaaaBot.Entity
{
    public class InternalData
    {
        public Dictionary<int, bool> Upgrades = new Dictionary<int, bool>();
        public Dictionary<Unit, int> CompletedUnits = new Dictionary<Unit, int>();
        public Dictionary<PendingUnit, int> PendingUnits = new Dictionary<PendingUnit, int>();

        public void AddUpgrade(int upgradeId)
        {
            Upgrades.Add(upgradeId, true);
        }

        public bool CheckUpgrade(int upgradeId)
        {
            if (!Upgrades.ContainsKey(upgradeId))
                return false;

            return Upgrades.Where(u => u.Key == upgradeId)
                .Select(u => u.Value)
                .FirstOrDefault();
        }

        public void AddUnit(Unit u)
        {
            CompletedUnits.Add(u, 1);
        }

        public void AddPendingUnit(uint unitId, long expFrame)
        {
            PendingUnits.Add(new PendingUnit(unitId, expFrame),1);
        }
        public void AddPendingUnit(uint unitId, ulong tag)
        {
            PendingUnits.Add(new PendingUnit(unitId, tag),1);
        }

        public int GetTotalUnitCount(uint unitId)
        {
            // edge case where unit is worker (inaccurate due to hiden in geyser)
            if (Units.Workers.Contains(unitId))
                return (int)VBergaaaBot.Bot.Observation.Observation.PlayerCommon.FoodWorkers + PendingUnits.Count(u => u.Key.UnitType == unitId);
            return CompletedUnits.Count(u => u.Key.unitType == unitId) + PendingUnits.Count(u => u.Key.UnitType == unitId);
        }

        public int GetActiveUnitCount(uint unitId)
        {
            return CompletedUnits.Count(u => u.Key.unitType == unitId);
        }

        public void OnFrame(Observation obsv)
        {
            CompletedUnits = new Dictionary<Unit, int>();
            // reads the game state and adds all units into CompletedUnits Dictionary
            foreach (var unit in obsv.RawData.Units.Where(u=>u.Alliance==Alliance.Self))
            {
                AddUnit(new Unit(unit));
            }

            // reads pending units and removes any that should be completed
            var unitsToUpdate = PendingUnits.Where(u => u.Key.ExpectedFrame == obsv.GameLoop).ToList();
            foreach (var pending in unitsToUpdate)
            {
                PendingUnits.Remove(pending.Key);
            }

            // reads pending buildings and removes any that should have started or drone has died.
            var friendlyUnits = CompletedUnits.Select(u => u.Key.tag);
            var buildingsToRemove = PendingUnits.Where(p => !friendlyUnits.Contains(p.Key.UnitTag) && p.Key.UnitTag!=0).ToList();
            foreach (var pending in buildingsToRemove)
            {
                PendingUnits.Remove(pending.Key);
            }
        }

        public void PrintUnitState()
        {
            string message = "";
            var unitCounts = CompletedUnits.GroupBy(u => u.Key.unitType);
            foreach (var unit in unitCounts)
            {
                message += "\nUnit: " + Controller.GetUnitName(unit.Key) + ", Code: " + unit.Key + " Count: " + unit.Count();
            }
            Logger.Info(message);
        }
    }
}
