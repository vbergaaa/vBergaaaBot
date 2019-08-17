using vBergaaaBot.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace vBergaaaBot.Managers
{
    public class StateManager : Manager
    {
        Dictionary<uint, int> Counts = new Dictionary<uint, int>();
        Dictionary<uint, int> CompletedCounts = new Dictionary<uint, int>();
        Dictionary<ulong, Agent> Agents = new Dictionary<ulong, Agent>();
        HashSet<uint> UpgradesFinished = new HashSet<uint>();
        HashSet<uint> UpgradesInProgress = new HashSet<uint>();
        public override void OnFrame()
        {
            Counts = new Dictionary<uint, int>();
            CompletedCounts = new Dictionary<uint, int>();
            UpgradesInProgress = new HashSet<uint>();
            foreach (var unit in VBot.Bot.Observation.Observation.RawData.Units)
            {
                if (unit.Alliance == SC2APIProtocol.Alliance.Self)
                {
                    // get counts
                    if (!Units.Workers.Contains(unit.UnitType))// avoid worker units as count is wrong while inside gas geyser, get from obs.obs.playercommon instead
                        DictionaryHelper.Increment(Counts, unit.UnitType); 
                    if (unit.BuildProgress > 0.9999)
                        DictionaryHelper.Increment(CompletedCounts, unit.UnitType);
                    else
                        if (Units.GrantsSupply.Contains(unit.UnitType))
                            VBot.Bot.PendingSupply += (int)VBot.Bot.Data.Units[(int)unit.UnitType].FoodProvided;
                    // adds to Agents if not an agent
                    if (!Agents.ContainsKey(unit.Tag))
                        Agents.Add(unit.Tag, new Agent(unit));
                    else
                    {
                        Agents[unit.Tag].Update(unit);
                    }

                    // adds units that are under construction
                    if (unit.Orders.Count > 0 && unit.Orders[0] != null && Abilities.CreatesUnit.ContainsKey(unit.Orders[0].AbilityId))
                    {
                        DictionaryHelper.Increment(Counts, Abilities.CreatesUnit[unit.Orders[0].AbilityId]);
                        if (Abilities.CreatesUnit[unit.Orders[0].AbilityId] == Units.ZERGLING)
                            DictionaryHelper.Increment(Counts, Abilities.CreatesUnit[unit.Orders[0].AbilityId]);
                        if (Abilities.CreatesUnit[unit.Orders[0].AbilityId] == Units.OVERLORD)
                            VBot.Bot.PendingSupply += 8;
                    }
                    if (unit.Orders.Count > 0 && unit.Orders[0] != null && Abilities.ResearchsUpgrade.ContainsKey(unit.Orders[0].AbilityId))
                    {
                        UpgradesInProgress.Add(Abilities.ResearchsUpgrade[unit.Orders[0].AbilityId]);
                    }

                }
            }

            if (VBot.Bot.Observation.Observation.RawData.Event != null)
                foreach (var unit in VBot.Bot.Observation.Observation.RawData.Event.DeadUnits)
                    Agents.Remove(unit);

            foreach (var upgrade in VBot.Bot.Observation.Observation.RawData.Player.UpgradeIds)
            {
                if (!UpgradesFinished.Contains(upgrade))
                    UpgradesFinished.Add(upgrade);
            }
        }

        public int GetCount(uint unitType)
        {
            if (Units.Workers.Contains(unitType))
            {
                if (Counts.ContainsKey(unitType)) // populated if worker in construction
                    return (int)VBot.Bot.Observation.Observation.PlayerCommon.FoodWorkers + Counts[unitType];
                return (int)VBot.Bot.Observation.Observation.PlayerCommon.FoodWorkers;
            }
                
            if (Counts.ContainsKey(unitType))
                return Counts[unitType];
            return 0;
        }
        public int GetCompletedCount(uint unitType)
        {
            if (Units.Workers.Contains(unitType))
                return (int)VBot.Bot.Observation.Observation.PlayerCommon.FoodWorkers;
            if (CompletedCounts.ContainsKey(unitType))
                return CompletedCounts[unitType];
            return 0;
        }
        public int GetCompletedCount(HashSet<uint> unitTypes)
        {
            int count = 0;

            foreach(var u in unitTypes)
                if (CompletedCounts.ContainsKey(u))
                    count += CompletedCounts[u];
            return count;
        }

        public Agent GetAvailableAgent(HashSet<uint> unitTypes)
        {
            return Agents.Where(u => !u.Value.Busy && unitTypes.Contains(u.Value.Unit.UnitType)).FirstOrDefault().Value;
        }

        public List<Agent> GetAgents(HashSet<uint> unitTypes)
        {
            return Agents.Where(a => unitTypes.Contains(a.Value.Unit.UnitType)).Select(a => a.Value).ToList();
        }

        public Agent GetAgentByTag(ulong tag)
        {
            if (Agents.ContainsKey(tag))
                return Agents[tag];
            return null;
        }

        /// <summary>
        /// Checks to see if an upgrade has been completed
        /// </summary>
        /// <param name="upgrade">id of the upgrade to check</param>
        /// <returns>true if upgrade is completed, false otherwise</returns>
        public bool CheckUpgradeFinished(int upgrade)
        {
            if (UpgradesFinished.Contains((uint)upgrade))
                return true;
            return false;
        }
        /// <summary>
        /// Checks to see if an upgrade has been started
        /// </summary>
        /// <param name="upgrade">id of the upgrade to check</param>
        /// <returns>true if upgrade has started, false otherwise</returns>
        public bool CheckUpgradeInProgress(int upgrade)
        {
            if (UpgradesFinished.Contains((uint)upgrade))
                return true;
            return false;
        }
    }
}
