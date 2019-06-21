//using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SC2APIProtocol;
using VBergaaaBot.Agents;

namespace VBergaaaBot.Controllers
{
    class Controller
    {
        private static List<Action> Actions = new List<Action>();
        public static Observation obs { get; set; }
        public static ResponseData data;
        private static uint minerals;
        private static uint gas;
        private static uint currentSupply;
        private static uint maxSupply;

        public static void Open(Observation obvs)
        {
            Actions.Clear();
            obs = obvs;
            minerals = obvs.PlayerCommon.Minerals;
            gas = obvs.PlayerCommon.Vespene;
            currentSupply = obvs.PlayerCommon.FoodUsed;
            maxSupply = obvs.PlayerCommon.FoodCap;
        }
        public static List<Action> Close()
        {
            return Actions;
        }

        // simplify actions
        private static Action CreateRawUnitCommand(int abilityId)
        {
            Action action = new Action();
            action.ActionRaw = new ActionRaw();
            action.ActionRaw.UnitCommand = new ActionRawUnitCommand();
            action.ActionRaw.UnitCommand.AbilityId = abilityId;
            return action;
        }

        // check command is possible
        public static bool CanAfford(uint unitId)
        {
            var unitData = data.Units[(int)unitId];
            if (minerals >= unitData.MineralCost && 
                gas >= unitData.VespeneCost && 
                maxSupply - currentSupply >= unitData.FoodRequired)
                return true;
            return false;
        }

        // get single unit of type

        // get single unit near location of type

        // Get
        public static List<Unit> GetUnits(HashSet<uint>hashset, Alliance alliance = Alliance.Self)
        {
            List<Unit> units = new List<Unit>();
            foreach (Unit unit in obs.RawData.Units)
            {
                if (unit.Alliance != alliance)
                    continue;
                if (!hashset.Contains(unit.UnitType))
                    continue;
                units.Add(unit);
            }
            return units;
        } 
        public static List<Unit> GetUnits(uint unitId, Alliance alliance  = Alliance.Self)
        {
            List<Unit> units = new List<Unit>();
            foreach (Unit unit in obs.RawData.Units)
            {
                if (unit.Alliance != alliance)
                    continue;
                if (unit.UnitType != unitId)
                    continue;
                units.Add(unit);
            }
            return units;
        }
        public static Unit GetUnit(uint unitId, Alliance alliance = Alliance.Self)
        {
            List<Unit> units = GetUnits(unitId, alliance: alliance);
            if (units.Count > 0)
                return units[0];
            return null;
        }
        public static Unit GetAvailableWorker()
        {
            List<Unit> workers = GetUnits(Units.WorkerTypes);
            foreach (Unit worker in workers)
                if (worker.Orders.Count == 0)
                    return worker;
            foreach (Unit worker in workers)
                if (worker.Orders[0].AbilityId == Abilities.HARVEST_GATHER_DRONE)
                    return worker;
            return null;
        }

        // Move
        public static void Move(List<Unit> units, Point2D target)
        {
            Action action = CreateRawUnitCommand(Abilities.MOVE);
            action.ActionRaw.UnitCommand.TargetWorldSpacePos = target;
            foreach (var unit in units)
                action.ActionRaw.UnitCommand.UnitTags.Add(unit.Tag);
            Actions.Add(action);
        }
        public static void Move(Unit unit, Point2D target)
        {
            List<Unit> units = new List<Unit>();
            Move(units, target);
        }

        // Attack
        public static void Attack(List<Unit> units, Point2D target)
        {
            Action action = CreateRawUnitCommand(Abilities.ATTACK);
            action.ActionRaw.UnitCommand.TargetWorldSpacePos = target;
            foreach (var unit in units)
                action.ActionRaw.UnitCommand.UnitTags.Add(unit.Tag);
            Actions.Add(action);
        }
        public static void Attack(Unit unit, Point2D target)
        {
            List<Unit> units = new List<Unit>();
            units.Add(unit);
            Attack(units, target);
        }
        public static void Attack(List<Unit> units, ulong unitTag)
        {
            Action action = CreateRawUnitCommand(Abilities.ATTACK);
            action.ActionRaw.UnitCommand.TargetUnitTag = unitTag;
            foreach (var unit in units)
                action.ActionRaw.UnitCommand.UnitTags.Add(unit.Tag);
            Actions.Add(action);
        }
        public static void Attack(Unit unit, ulong unitTag)
        {
            List<Unit> units = new List<Unit>();
            units.Add(unit);
            Attack(units, unitTag);
        }

        // Build
        public static void Build(uint unitType, Point2D target)
        {
            Unit worker = GetAvailableWorker();
            if (worker == null)
            {
                Logger.WriteLine("could not find available worker");
                return;
            }
            int abilityId = Abilities.GetID(unitType);
            Action action = CreateRawUnitCommand(abilityId);
            action.ActionRaw.UnitCommand.UnitTags.Add(worker.Tag);
            action.ActionRaw.UnitCommand.TargetWorldSpacePos = target;
            Actions.Add(action);
            Logger.WriteLine("Constructing: " + data.Units[(int)unitType].Name);
        }

        // worker build building location

        // larva morph unit
        public static void MorphFromLarva()
        {

        }
        // unit use ability

        // group of units use abiltity

    }
}
