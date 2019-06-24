using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vBergaaaBot;
namespace vBergaaaBot.Entity
{
    public class InternalData
    {
        public Dictionary<int, bool> Upgrades = new Dictionary<int, bool>();
        public Dictionary<uint, int> Units = new Dictionary<uint, int>();
        public Dictionary<uint, int> Buildings = new Dictionary<uint, int>();

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

        public InternalData(VBergaaaBot bot)
        {
            if (bot.MyRace == SC2APIProtocol.Race.Zerg)
            {
                Units.Add(vBergaaaBot.Units.DRONE, 12);
                Units.Add(vBergaaaBot.Units.OVERLORD,1);
                Buildings.Add(vBergaaaBot.Units.HATCHERY, 1);
            }  
        }

        public void AddUnits(uint unitId, int qty)
        {
            if (Units.ContainsKey(unitId))
            {
                int oldValue = Units.First(u => u.Key == unitId).Value;
                Units.Remove(unitId);
                Units.Add(unitId, qty + oldValue);
            }
            else
                Units.Add(unitId, qty);
        }

        public void AddUnits(uint unitId)
        {
            AddUnits(unitId, 1);
        }

        public void RemoveUnit(uint unitId)
        {
            AddUnits(unitId, -1);
        }

        public void AddBuildings(uint unitId, int qty)
        {
            // need to code logic to remove the total supply of drones here
            if (vBergaaaBot.Units.Zerg.Contains(unitId))
                RemoveUnit(104); //drone
            if (Buildings.ContainsKey(unitId))
            {
                int oldValue = Buildings.First(u => u.Key == unitId).Value;
                Buildings.Remove(unitId);
                Buildings.Add(unitId, qty + oldValue);
            }
            else
                Buildings.Add(unitId, qty);
        }

        public void AddBuildings(uint unitId)
        {
            AddBuildings(unitId, 1);
        }

        public int GetUnitCount(uint unitId)
        {
            if (Units.ContainsKey(unitId))
                return Units.FirstOrDefault(u => u.Key == unitId).Value;
            return 0;
        }
        public int GetBuildingCount(uint unitId)
        {
            if (Buildings.ContainsKey(unitId))
                return Buildings.FirstOrDefault(u => u.Key == unitId).Value;
            return 0;
        }

    }
}
