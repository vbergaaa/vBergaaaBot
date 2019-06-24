using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Threading;
using SC2APIProtocol;
using Action = SC2APIProtocol.Action;
// ReSharper disable MemberCanBePrivate.Global

namespace vBergaaaBot {
    public static class Controller {
        //editable
        private static readonly int frameDelay = 0; //too fast? increase this to e.g. 20
        private static readonly bool realTime = true; // change this to watch the game at the speed of a player
        private static readonly Stopwatch stopwatch = new Stopwatch();
        private static long milliseconds;

        //don't edit
        private static readonly List<Action> actions = new List<Action>();
        private static readonly Random random = new Random();
        private const double FRAMES_PER_SECOND = 22.4;

        public static ResponseGameInfo gameInfo;
        public static ResponseData gameData;
        public static ResponseObservation obs;
        public static ulong frame;
        public static uint currentSupply;
        public static uint maxSupply;
        public static uint minerals;
        public static uint vespene;

       
        public static readonly List<string> chatLog = new List<string>();

        public static void Pause() {
            Console.WriteLine("Press any key to continue...");
            while (Console.ReadKey().Key != ConsoleKey.Enter) {
                //do nothing
            }
        }

        public static ulong SecsToFrames(int seconds) {
            return (ulong) (FRAMES_PER_SECOND * seconds);
        }


        public static List<Action> CloseFrame() {
            int delay = 16 - (int)(stopwatch.ElapsedMilliseconds - milliseconds); // 16 = 23 ms/f (fastest) - 7 ms to process the timer
            if (realTime && delay>0)
            {
                Thread.Sleep(delay);
            }
            return actions;
        }


        public static void OpenFrame() {
            if (gameInfo == null || gameData == null || obs == null) {
                if (gameInfo == null)
                    Logger.Info("GameInfo is null! The application will terminate.");
                else if (gameData == null)
                    Logger.Info("GameData is null! The application will terminate.");
                else
                    Logger.Info("ResponseObservation is null! The application will terminate.");
                Pause();
                Environment.Exit(0);
            }

            actions.Clear();
            obs = VBergaaaBot.Bot.Observation;

            foreach (var chat in obs.Chat) 
                chatLog.Add(chat.Message);

            frame = obs.Observation.GameLoop;
            currentSupply = obs.Observation.PlayerCommon.FoodUsed;
            maxSupply = obs.Observation.PlayerCommon.FoodCap;
            minerals = obs.Observation.PlayerCommon.Minerals;
            vespene = obs.Observation.PlayerCommon.Vespene;
            

            //initialization
            if (frame == 0) {
                if (realTime)
                    stopwatch.Start();
                var resourceCenters = GetUnits(Units.ResourceCenters);
                if (resourceCenters.Count > 0) {
                    var rcPosition = resourceCenters[0].position;
                }
            }

            if (frameDelay > 0)
                Thread.Sleep(frameDelay);

            if (realTime)
                milliseconds = stopwatch.ElapsedMilliseconds;
        }


        public static string GetUnitName(uint unitType) {
            return gameData.Units[(int) unitType].Name;
        }

        public static string GetUpgradeName(int upgradeId)
        {
            return gameData.Upgrades[upgradeId].Name;
        }

        public static uint GetUpgradeAbilityId(int upgradeId)
        {
            return gameData.Upgrades[upgradeId].AbilityId;
        }

        public static void AddAction(Action action) {
            actions.Add(action);
        }

        public static int GetActionsCount()
        {
            return actions.Count;
        }

        public static void Chat(string message, bool team = false) {
            var actionChat = new ActionChat();
            actionChat.Channel = team ? ActionChat.Types.Channel.Team : ActionChat.Types.Channel.Broadcast;
            actionChat.Message = message;

            var action = new Action();
            action.ActionChat = actionChat;
            AddAction(action);
        }

        public static void Inject(Unit queen)
        {
            List<Unit> bases = new List<Unit>();
            foreach (Unit rc in GetUnits(Units.ResourceCenters))
            {
                bases.Add(rc);
            }
            //var action = new ActionRawUnitCommand();
            //action.AbilityId = Abilities.EFFECT_INJECTLARVA;
            //action.TargetUnitTag = GetNearestTag(queen.Pos, bases);
            //action.UnitTags.Add(queen.tag);
            //AddAction(action);
            var action = CreateRawUnitCommand(Abilities.EFFECT_INJECTLARVA);
            action.ActionRaw.UnitCommand.TargetUnitTag = GetNearestTag(queen.Pos, bases);
            action.ActionRaw.UnitCommand.UnitTags.Add(queen.tag);
            AddAction(action);
        }

        public static void Attack(List<Unit> units, Point2D target)
        {
            var action = CreateRawUnitCommand(Abilities.ATTACK);
            action.ActionRaw.UnitCommand.TargetWorldSpacePos = target;
            foreach (var unit in units)
                action.ActionRaw.UnitCommand.UnitTags.Add(unit.tag);
            AddAction(action);
        }

        public static void Attack(Unit unit, Point2D target)
        {
            var action = CreateRawUnitCommand(Abilities.ATTACK);
            action.ActionRaw.UnitCommand.TargetWorldSpacePos = target;
            action.ActionRaw.UnitCommand.UnitTags.Add(unit.tag);
            AddAction(action);
        }

        public static void Move(List<Unit> units, Point2D target)
        {
            var action = CreateRawUnitCommand(Abilities.MOVE);
            action.ActionRaw.UnitCommand.TargetWorldSpacePos = target;

            foreach (var unit in units)
                action.ActionRaw.UnitCommand.UnitTags.Add(unit.tag);
            AddAction(action);
        }

        public static void Move(Unit unit, Point2D target)
        {
            var action = CreateRawUnitCommand(Abilities.MOVE);
            action.ActionRaw.UnitCommand.TargetWorldSpacePos = target;
            action.ActionRaw.UnitCommand.UnitTags.Add(unit.tag);
            AddAction(action);
        }

        public static void Upgrade(int ability, Unit unit)
        {
            var action = CreateRawUnitCommand(ability);
            action.ActionRaw.UnitCommand.UnitTags.Add(unit.tag);
            AddAction(action);
            Logger.Info("started upgradeing: {0}",ability);
        }

        public static int GetTotalCount(uint unitType) {
            var pendingCount = GetPendingCount(unitType, inConstruction: false);
            var constructionCount = GetUnits(unitType).Count;
            return pendingCount + constructionCount;
        }

        public static int GetPendingCount(uint unitType, bool inConstruction=true) {
            var workers = GetUnits(Units.Workers);
            var abilityID = Abilities.GetTrainUnitId(unitType);
            var cocoons = GetUnits(Units.EGG);
            var production = GetUnits(Units.Production);

            var counter = 0;
            
            //count workers that have been sent to build this structure
            foreach (var worker in workers) {
                if (worker.order.AbilityId == abilityID)
                    counter += 1;
            }

            foreach (var building in production)
            {
                if (building.order.AbilityId == abilityID)
                    counter++;
            }

            //count buildings that are already in construction
            if (inConstruction) {  
                foreach (var unit in GetUnits(unitType))
                    if (unit.buildProgress < 1)
                        counter += 1;
            }

            //count larva morphing into this structure
            foreach (var cocoon in cocoons)
            {
                if (cocoon.order.AbilityId == abilityID)
                    counter++;
            }

            return counter;
        }

        public static List<Unit> GetUnits(HashSet<uint> hashset, Alliance alliance = Alliance.Self, bool onlyCompleted = false, bool onlyVisible = false, bool onlyIdle = false)
        {
            //ideally this should be cached in the future and cleared at each new frame
            var count = 0;
            var units = new List<Unit>();
            foreach (SC2APIProtocol.Unit unit in obs.Observation.RawData.Units)
            {
                count++;
                if (hashset.Contains(unit.UnitType) && unit.Alliance == alliance)
                {

                    if (onlyCompleted && unit.BuildProgress < 1)
                        continue;

                    if (onlyVisible && (unit.DisplayType != DisplayType.Visible))
                        continue;

                    if (onlyIdle && (unit.Orders.Count != 0))
                        continue;

                    units.Add(new Unit(unit));

                }
            }
            return units;
        }
        public static List<Unit> GetNeutralUnits(HashSet<uint> hashset, Alliance alliance = Alliance.Self, bool onlyCompleted = false, bool onlyVisible = false)
        {
            //ideally this should be cached in the future and cleared at each new frame
            var count = 0;
            var units = new List<Unit>();
            foreach (SC2APIProtocol.Unit unit in obs.Observation.RawData.Units)
            {
                count++;
                if (hashset.Contains(unit.UnitType) && unit.Alliance == Alliance.Neutral)
                {
                    units.Add(new Unit(unit));
                }
            }
            return units;
        }

        public static List<Unit> GetNeutralUnits(uint unitCode, Alliance alliance = Alliance.Self, bool onlyCompleted = false, bool onlyVisible = false)
        {
            //ideally this should be cached in the future and cleared at each new frame
            var count = 0;
            var units = new List<Unit>();
            foreach (SC2APIProtocol.Unit unit in obs.Observation.RawData.Units)
            {
                count++;
                if (unitCode == unit.UnitType && unit.Alliance == Alliance.Neutral)
                {
                    units.Add(new Unit(unit));
                }
            }
            return units;
        }

        public static List<Unit> GetUnits(uint unitType, Alliance alliance=Alliance.Self, bool onlyCompleted=false, bool onlyVisible=false, bool onlyIdle=false) {
            //ideally this should be cached in the future and cleared at each new frame
            var units = obs.Observation.RawData.Units.Where(u => u.UnitType == unitType && u.Alliance == alliance);

            if (onlyVisible)
                units = units.Where(u => u.DisplayType == DisplayType.Visible);

            if (onlyCompleted)
                units = units.Where(u => u.BuildProgress == 1);

            if (onlyIdle)
                units = units.Where(u => u.Orders.Count == 0);

            return units.Select(u=>new Unit(u)).ToList();
        }

        public static bool CanAfford(uint unitType)
        {
            var unitData = gameData.Units[(int)unitType];
            if (Units.Zerg.Contains(unitType) && !Units.Structures.Contains(unitType))
            {
                if (unitType != 105) { 
                    return ((VBergaaaBot.Bot.Minerals >= unitData.MineralCost) && (VBergaaaBot.Bot.Vespene >= unitData.VespeneCost) && (unitData.FoodRequired <= VBergaaaBot.Bot.AvailableSupply && GetUnits(Units.LARVA).Count > 0));
                }
                else // edge case were unit is a zergling (gamedata stats are wrong) 
                {
                    return ((VBergaaaBot.Bot.Minerals >= 50) && (VBergaaaBot.Bot.Vespene >= unitData.VespeneCost) && (unitData.FoodRequired <= VBergaaaBot.Bot.AvailableSupply && GetUnits(Units.LARVA).Count > 0));
                }
            }
            return (VBergaaaBot.Bot.Minerals >= unitData.MineralCost) && (VBergaaaBot.Bot.Vespene >= unitData.VespeneCost) && (unitData.FoodRequired <= maxSupply - currentSupply);
           
        }


        public static bool CanMake(uint unitType)
        {
            throw new NotImplementedException();
        }

        public static bool MeetsTechRequirements(uint unitType)
        {
            // possiblity here if tech requirement is hatchery but we have lair to return false. investigate if encountered.
            if (gameData.Units[(int)unitType].TechRequirement == 0)
                return true;
            if (GetUnits(gameData.Units[(int)unitType].TechRequirement, onlyCompleted: true).Count > 0)
                return true;
            else
                return false;            
        }

        public static bool IsAlive(ulong tag)
        {
            foreach (SC2APIProtocol.Unit unit in obs.Observation.RawData.Units)
            {
                if (unit.Tag == tag)
                    return true;
            }
            return false;
        }
        public static bool IsAlive()
        {
            return false;
        }

        public static Unit GetUnitByTag(ulong tag)
        {
            foreach (SC2APIProtocol.Unit unit in obs.Observation.RawData.Units)
            {
                if (unit.Tag == tag)
                    return new Unit(unit);
            }
            return null;

        }

        public static bool CanAffordUpgrade(int upgrade) {
            var unitData = gameData.Upgrades[(int)upgrade];
            return (minerals >= unitData.MineralCost) && (vespene >= unitData.VespeneCost);
        }

        public static bool CanConstruct(uint unitType) {
            //is it a structure?
            if (Units.Structures.Contains(unitType)) {
                //we need worker for every structure
                if (GetUnits(Units.Workers).Count == 0) return false;

                //we need an RC for any structure
                var resourceCenters = GetUnits(Units.ResourceCenters, onlyCompleted:true);
                if (resourceCenters.Count == 0) return false;
                
                if ((unitType == Units.COMMAND_CENTER) || (unitType == Units.SUPPLY_DEPOT))
                    return CanAfford(unitType);
                
                //we need supply depots for the following structures
                var depots = GetUnits(Units.SupplyDepots, onlyCompleted:true);
                if (depots.Count == 0) return false;
                
                if (unitType == Units.BARRACKS)
                    return CanAfford(unitType);
            }
            
            //it's an actual unit
            else {                
                //do we have enough supply?
                var requiredSupply = Controller.gameData.Units[(int) unitType].FoodRequired;
                if (requiredSupply > (maxSupply - currentSupply))
                    return false;

                //do we construct the units from barracks? 
                if (Units.FromBarracks.Contains(unitType)) {
                    var barracks = GetUnits(Units.BARRACKS, onlyCompleted:true);
                    if (barracks.Count == 0) return false;
                }
                                
            }
            
            return CanAfford(unitType);
        }

        public static Action CreateRawUnitCommand(int ability) {
            var action = new Action();
            action.ActionRaw = new ActionRaw();
            action.ActionRaw.UnitCommand = new ActionRawUnitCommand();
            action.ActionRaw.UnitCommand.AbilityId = ability;
            return action;
        }

        public static bool CanPlace(uint unitType, Point2D targetPos) {
            //Note: this is a blocking call! Use it sparingly, or you will slow down your execution significantly!
            var abilityID = Abilities.GetTrainUnitId(unitType);
            
            RequestQueryBuildingPlacement queryBuildingPlacement = new RequestQueryBuildingPlacement();
            queryBuildingPlacement.AbilityId = abilityID;
            queryBuildingPlacement.TargetPos = targetPos;
            
            Request requestQuery = new Request();
            requestQuery.Query = new RequestQuery();
            requestQuery.Query.Placements.Add(queryBuildingPlacement);

            var result = Program.gc.SendQuery(requestQuery.Query);
            if (result.Result.Placements.Count > 0)
                return (result.Result.Placements[0].Result == ActionResult.Success);
            return false;
        }

        public static void DistributeWorkers() {            
            var workers = GetUnits(Units.Workers);
            List<Unit> idleWorkers = new List<Unit>();
            foreach (var worker in workers) {
                if (worker.order.AbilityId != 0) continue;
                idleWorkers.Add(worker);
            }
            
            if (idleWorkers.Count > 0) {
                var resourceCenters = GetUnits(Units.ResourceCenters, onlyCompleted:true);
                var mineralFields = GetUnits(Units.MineralFields, onlyVisible: true, alliance:Alliance.Neutral);
                
                foreach (var rc in resourceCenters) {
                    //get one of the closer mineral fields
                    var mf = GetFirstInRange(rc.Pos, mineralFields, 7);
                    if (mf == null) continue;
                    
                    //only one at a time          
                    idleWorkers[0].Smart(mf);                                        
                    return;
                }
                //nothing to be done
                return;
            }
            else {
                //let's see if we can distribute between bases                
                var resourceCenters = GetUnits(Units.ResourceCenters, onlyCompleted:true);
                var geysers = GetUnits(Units.EXTRACTOR, onlyCompleted: true);
                Unit transferFrom = null;
                Unit transferTo = null;
                foreach (var rc in resourceCenters)
                {
                    if (rc.assignedWorkers <= rc.idealWorkers)
                        transferTo = rc;
                    else
                        transferFrom = rc;
                }
                foreach (var gas in geysers)
                {
                    if (gas.assignedWorkers <= gas.idealWorkers)
                        transferTo = gas;
                    else
                        transferFrom = gas;
                }

                if ((transferFrom != null) && (transferTo != null)) {

                    var mineralFields = GetUnits(Units.MineralFields, onlyVisible: true, alliance:Alliance.Neutral);
                    
                    var sqrDistance = 7 * 7;
                    foreach (var worker in workers) {
                        if (worker.order.AbilityId != Abilities.GATHER_MINERALS && worker.order.AbilityId != Abilities.HARVEST_GATHER_DRONE) continue;
                        if (Vector3.DistanceSquared(worker.position, transferFrom.position) > sqrDistance) continue;
                                                
                        var mf = GetFirstInRange(transferTo.Pos, mineralFields, 7);
                        if (mf == null) continue;
                    
                        //only one at a time
                        if (transferTo.unitType == Units.EXTRACTOR)
                        {
                            worker.Smart(transferTo);
                        }
                        else
                        {
                            worker.Smart(mf);
                        }               
                        return;
                    }
                }
                List<Unit> availableResources = new List<Unit>();
                foreach (var rc in resourceCenters)
                {
                    //foreach ()
                }
            }

            
        }

        public static void PrioritiseGas()
        {
            Logger.Info("Attempting to saturate gas");
            var availableWorkers = GetUnits(Units.Workers).Where(u=>u.orders[0].AbilityId == 1183).ToList();
            var geysers = GetUnits(Units.EXTRACTOR, onlyCompleted: true);
            foreach (var g in geysers)
            {
                int numberToTransfer;
                if (g.assignedWorkers < g.idealWorkers)
                {
                    numberToTransfer = g.idealWorkers - g.assignedWorkers;
                    if (availableWorkers.Count() >= numberToTransfer)
                    {
                        for (int i = 0; i < numberToTransfer; i++)
                            availableWorkers[i].Smart(g);
                    }
                }
            }
        }

        public static Unit GetAvailableWorker()
        {
            var workers = GetUnits(Units.Workers);
            foreach (var worker in workers)
            {
                if (worker.order.AbilityId != Abilities.GATHER_MINERALS && worker.order.AbilityId != Abilities.HARVEST_GATHER_DRONE)
                    continue;
                return worker;
            }

            return null;
        }

        public static bool IsInRange(Point2D targetPosition, List<Unit> units, float maxDistance) {
            return (GetFirstInRange(targetPosition, units, maxDistance) != null);
        }
        public static bool IsInRange(Point2D targetPosition, List<Point2D> patches, float maxDistance) {
            return (GetFirstInRange(targetPosition, patches, maxDistance) != null);
        }
        
        public static Unit GetFirstInRange(Point2D targetPosition, List<Unit> units, float maxDistance) {
            //squared distance is faster to calculate
            var maxDistanceSqr = maxDistance * maxDistance;
            foreach (var unit in units) {
                if ( Get2dDistanceSquared(targetPosition, unit.Pos) <= maxDistanceSqr)
                    return unit;
            }
            return null;
        }
        public static Point2D GetFirstInRange(Point2D targetPosition, List<Point2D> patches, float maxDistance) {
            //squared distance is faster to calculate
            var maxDistanceSqr = maxDistance * maxDistance;
            foreach (var patch in patches) {
                if ( Get2dDistanceSquared(targetPosition, patch) <= maxDistanceSqr)
                    return patch;
            }
            return null;
        }

        public static void Construct(uint unitType) {
            Logger.Info("{0}: Beginning Contructions of a {1}", frame.ToString(), GetUnitName(unitType));
            Entity.BaseLocation startLocation = VBergaaaBot.Bot.MapInformation.StartLocation;
            Point2D startingSpot = startLocation.Location;
            Point2D constructionSpot = null;
            const int radius = 12;
            var abilityID = Abilities.GetTrainUnitId(unitType);
            var constructAction = CreateRawUnitCommand(abilityID);

            var worker = GetAvailableWorker();
            if (worker == null)
            {
                Logger.Error("Unable to find worker to construct: {0}", GetUnitName(unitType));
                return;
            }

            // if gas geyser
            if (Units.GasGeysers.Contains(unitType))
            {
                ulong constructionLocation = 0;
                Logger.Info("attempting to build a gas");
                foreach (var rc in GetUnits(Units.ResourceCenters,onlyCompleted:true))
                    foreach (var gas in GetUnits(Units.GasGeysers, alliance: Alliance.Neutral))
                        if (gas.unitType != Units.EXTRACTOR && Get2dDistanceSquared(rc.Pos,gas.Pos) < 81)
                            constructionLocation = gas.Tag;

                if (constructionLocation != 0)
                {
                    constructAction.ActionRaw.UnitCommand.UnitTags.Add(worker.tag);
                    constructAction.ActionRaw.UnitCommand.TargetUnitTag = constructionLocation;
                    AddAction(constructAction);
                    Logger.Info("Constructing: {0} @ geyser {1}", GetUnitName(unitType), constructionLocation);
                    return;
                }
                
            }
            //trying to find a valid construction spot
            
            List<Point2D> mineralFields = new List<Point2D>();
            foreach (Entity.MineralField mf in startLocation.MineralPatches)
                mineralFields.Add(mf.Location);

            while (true)
            {
                constructionSpot = new Point2D();
                constructionSpot.X = startingSpot.X + random.Next(-radius, radius + 1);
                constructionSpot.Y = startingSpot.Y + random.Next(-radius, radius + 1);

                //avoid building in the mineral line
                if (IsInRange(constructionSpot, mineralFields, 5)) continue;

                //check if the building fits
                if (!CanPlace(unitType, constructionSpot)) continue;

                //ok, we found a spot
                break;
            }

            
            constructAction.ActionRaw.UnitCommand.UnitTags.Add(worker.tag);
            constructAction.ActionRaw.UnitCommand.TargetWorldSpacePos = constructionSpot;
            AddAction(constructAction);

            Logger.Info("Constructing: {0} @ {1} / {2}", GetUnitName(unitType), constructionSpot.X, constructionSpot.Y);

        }

        public static void Construct(uint unitType, Point2D position)
        {
            var worker = GetAvailableWorker();
            if (worker == null)
            {
                Logger.Error("Unable to find worker to construct: {0}", GetUnitName(unitType));
                return;
            }
            var abilityID = Abilities.GetTrainUnitId(unitType);
            var constructAction = CreateRawUnitCommand(abilityID);
            constructAction.ActionRaw.UnitCommand.UnitTags.Add(worker.tag);
            constructAction.ActionRaw.UnitCommand.TargetWorldSpacePos = position;
            AddAction(constructAction);
            Logger.Info("Constructing: {0}", GetUnitName(unitType));
        }

        public static ulong GetNearestTag(Point2D point, List<Unit> bases)
        {
            float nearestDistance = 10000000;
            ulong NearestTag = 0;
            foreach (Unit rc in bases)
            {
                var tempDist = MapInformation.GetDistance2D(rc.Pos, point);
                if (tempDist < nearestDistance)
                {
                    nearestDistance = tempDist;
                    NearestTag = rc.Tag;
                }
            }
            return NearestTag;
        }

        public static float Get2dDistanceSquared(Point2D point1, Point2D point2)
        {
            return (float)(point1.X - point2.X) * (point1.X - point2.X) + (point1.Y - point2.Y) * (point1.Y - point2.Y);
        }
    }
}