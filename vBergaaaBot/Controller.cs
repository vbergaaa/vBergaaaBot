using System;
using System.Collections.Generic;
using System.Linq;
using vBergaaaBot.Helpers;
using vBergaaaBot.Map;
using SC2APIProtocol;
using Action = SC2APIProtocol.Action;

namespace vBergaaaBot {
    public static class Controller {


        /// <summary>
        ///     Checks if there is enough minerals and gas to build a unit or structure.
        /// </summary>
        /// <param name="unitType">the unit type of the desired unit</param>
        /// <returns>true if resources are available, false otherwise</returns>
        public static bool CanAffordUnit(uint unitType)
        {
            UnitTypeData unitData = VBot.Bot.Data.Units[(int)unitType];
            if (Units.ZergStructures.Contains(unitType))
                if (VBot.Bot.Minerals() >= (unitData.MineralCost - 50) && VBot.Bot.Gas() >= unitData.VespeneCost)
                    return true;
            if (VBot.Bot.Minerals() >= unitData.MineralCost && VBot.Bot.Gas() >= unitData.VespeneCost)
                return true;
            return false;
        }

        /// <summary>
        ///     Checks if there is enough minerals and gas to research an upgrade.
        /// </summary>
        /// <param name="upgradeId">the unit type of the desired upgrade</param>
        /// <returns>true if resources are available, false otherwise</returns>
        public static bool CanAffordUpgrade(int upgradeId)
        {
            UpgradeData unitData = VBot.Bot.Data.Upgrades[upgradeId];
            if (VBot.Bot.Minerals() >= unitData.MineralCost && VBot.Bot.Gas() >= unitData.VespeneCost)
                return true;
            return false;
        }

        /// <summary>
        ///     Checks if there is enough supply to build a unit.
        /// </summary>
        /// <param name="unitType">the unit type of the desired unit</param>
        /// <returns>true if supply is available, false otherwise</returns>
        public static bool CheckSupply(uint unitType)
        {
            UnitTypeData unitData = VBot.Bot.Data.Units[(int)unitType];
            if (VBot.Bot.AvailableSupply() >= unitData.FoodRequired)
                return true;
            return false;
        }

        /// <summary>
        /// Checks if it's possible to make a unit
        /// </summary>
        ///  <param name="unitType">the unit type of the desired unit</param>
        /// <returns>true if possible to make, false otherwise</returns>
        public static bool CanMakeUnit(uint unitType)

        {
            if (!CheckUnitTechRequirements(unitType))
                return false;

            if (!CheckSupply(unitType))
                return false;

            if (!CanAffordUnit(unitType))
                return false;

            if (MorphHelper.MorpSteps.ContainsKey(unitType))
                return PreMorphUnitAvailable(unitType);

            // check building is free to build
            if (TrainHelper.TrainSteps.ContainsKey(unitType))
                return TrainingBuildingAvailable(unitType);
            return true;
        }

        /// <summary>
        /// Checks if it's possible to make an upgrade
        /// </summary>
        ///  <param name="upgradeId">the upgradeType of the desired upgrade</param>
        /// <returns>true if possible to make, false otherwise</returns>
        public static bool CanMakeUpgrade(int upgradeId)
        {
            if (!CheckUpgradeTechRequirements(upgradeId))
                return false;

            if (!CanAffordUpgrade(upgradeId))
                return false;

            // check building is free to build
            if (UpgradeHelper.UpgradeSteps.ContainsKey(upgradeId))
                return TrainingBuildingAvailable(upgradeId);
            return true;
        }

        /// <summary>
        ///     Checks if there the tech requirements are met to make a unit.
        /// </summary>
        /// <param name="unitType">the unit type of the desired unit</param>
        /// <returns>true if the tech is build, false otherwise</returns>
        public static bool CheckUnitTechRequirements(uint unitType)
        {
            // this might make problems for something like spire/greaterspire
            if (VBot.Bot.Data.Units[(int)unitType].TechRequirement == 0)
                return true;

            if (GetCompletedCount(VBot.Bot.Data.Units[(int)unitType].TechRequirement) > 0)
                return true;
            return false;
        }
        
        /// <summary>
        ///     Checks if there the tech requirements are met to make an upgrade.
        /// </summary>
        /// <param name="upgradeId">the unit type of the desired unit</param>
        /// <returns>true if the tech is built or non-existant, false otherwise</returns>
        public static bool CheckUpgradeTechRequirements(int upgradeId)
        {
            if (UpgradeHelper.GetUpgradeTechBuildingReq(upgradeId) == 0 &&
                UpgradeHelper.GetUpgradeTechUpgradeReq(upgradeId) == 0)
                return true;

            if (UpgradeHelper.GetUpgradeTechUpgradeReq(upgradeId) != 0)
                if (!CheckUpgrade(UpgradeHelper.GetUpgradeTechUpgradeReq(upgradeId), onlyCompleted: true))
                    return false;
            if (UpgradeHelper.GetUpgradeTechBuildingReq(upgradeId) != 0)
                if (GetCompletedCount(UpgradeHelper.GetUpgradeTechBuildingReq(upgradeId)) == 0)
                    return false;
            return true;

        }

        /// <summary>
        ///     Checks if there is a free unit to move to morph into a the desired unit
        /// </summary>
        /// <param name="unitType">the unit type of the desired unit</param>
        /// <returns>true if there is a free unit, false otherwise</returns>
        public static bool PreMorphUnitAvailable(uint unitType)
        {
            if (MorphHelper.MorpSteps.ContainsKey(unitType))
            {
                if (null != GetAvailableAgent(MorphHelper.GetPreMorphType(unitType)))
                    return true;
                return false;
            }
            else
            {
                Logger.Info("Calling Controller.PreMorphUnitAvailable() on a unit that isnt in morph list. Unit: {0}, Type: {1}",
                    VBot.Bot.Data.Units[(int)unitType], unitType);
                return false;
            }
        }

        /// <summary>
        ///     Gets a free agent from the state manager.
        /// </summary>
        /// <param name="unitType">the unit type of the desired agent</param>
        /// <returns>a free agent if one is available, null otherwise</returns>
        public static Agent GetAvailableAgent(uint unitType)
        {
            HashSet<uint> hs = new HashSet<uint>
            {
                unitType
            };
            return VBot.Bot.StateManager.GetAvailableAgent(hs);
        }

        /// <summary>
        ///     Gets a free agent from the state manager.
        /// </summary>
        /// <param name="unitType">the unit type of the desired agent</param>
        /// <returns>a free agent if one is available, null otherwise</returns>
        public static Agent GetAvailableAgent(HashSet<uint> unitType)
        {
            return VBot.Bot.StateManager.GetAvailableAgent(unitType);
        }

        /// <summary>
        /// Get a count of how many units are made and in production
        /// </summary>
        /// <param name="unitType">the desired unit to count</param>
        /// <returns>the total count of a unit</returns>
        public static int GetTotalCount(uint unitType)
        {
            return VBot.Bot.StateManager.GetCount(unitType);
        }

        /// <summary>
        /// Get a count of only how many units are made
        /// </summary>
        /// <param name="unitType">the desired unit to count</param>
        /// <returns>the current count of a unit</returns>
        public static int GetCompletedCount(uint unitType)
        {
            return VBot.Bot.StateManager.GetCompletedCount(unitType);
        }

        /// <summary>
        /// Get a count of only how many units are made
        /// </summary>
        /// <param name="unitTypes">the desired units to be counted</param>
        /// <returns>the current count of the selected units</returns>
        public static int GetCompletedCount(HashSet<uint> unitTypes)
        {
            return VBot.Bot.StateManager.GetCompletedCount(unitTypes);
        }

        /// <summary>
        /// Gets a list of all the agents that match the unitType 
        /// </summary>
        /// <param name="unitType">type of desired unit</param>
        /// <returns>a list of all the agents that match the unitType</returns>
        public static List<Agent> GetAgents(uint unitType)
        {
            HashSet<uint> list = new HashSet<uint>
            {
                unitType
            };
            return VBot.Bot.StateManager.GetAgents(list);
        }

        /// <summary>
        /// Gets a list of all the agents that match the unitTypes 
        /// </summary>
        /// <param name="unitTypes">a hashset of the type of the desired units</param>
        /// <returns>a list of all the agents that match the unitTypes</returns>
        public static List<Agent> GetAgents(HashSet<uint> unitTypes)
        {
            return VBot.Bot.StateManager.GetAgents(unitTypes);
        }

        /// <summary>
        /// Gets desired SC2APIProtocol.Units from the Bots Observation
        /// </summary>
        /// <param name="unitTypes">a hashset of the desired unittypes</param>
        /// <returns>a list of SC2APIProtocol.Units that match the desired unitTypes</returns>
        public static List<Unit> GetUnits(HashSet<uint> unitTypes)
        {
            return VBot.Bot.Observation.Observation.RawData.Units.Where(u => unitTypes.Contains(u.UnitType)).ToList();
        }

        /// <summary>
        /// Creates a Request to send to through the API to see a build can be placed in this location
        /// </summary>
        /// <param name="unitType">type of the building you wish to build</param>
        /// <param name="targetPos">desired location</param>
        /// <returns>true if it is possible to build here, false otherwise</returns>
        public static bool CanPlace(uint unitType, Point2D targetPos)
        {
            //Note: this is a blocking call! Use it sparingly, or you will slow down your execution significantly!
            var abilityID = Units.GetAbilityId(unitType);

            RequestQueryBuildingPlacement queryBuildingPlacement = new RequestQueryBuildingPlacement
            {
                AbilityId = (int)abilityID,
                TargetPos = targetPos
            };

            Request requestQuery = new Request
            {
                Query = new RequestQuery()
            };
            requestQuery.Query.Placements.Add(queryBuildingPlacement);

            var result = Program.gc.SendQuery(requestQuery.Query);
            if (result.Result.Placements.Count() > 0)
                return (result.Result.Placements[0].Result == ActionResult.Success);
            return false;
        }

        /// <summary>
        /// Gets a worker agent with a bust status of false
        /// </summary>
        /// <returns>An available worker, null if no workers available</returns>
        public static Agent GetWorker()
        {
            return GetAvailableAgent(Units.Workers);
        }

        /// <summary>
        /// Function that executes a worker to build a structure at a location in the main base
        /// </summary>
        /// <param name="worker">An agent that has the ability to construct the building, typically a worker</param>
        /// <param name="BuildingType">The unit id of the desired building</param>
        public static void BuildStructure(Agent worker, uint BuildingType)
        {
            if (Units.GasGeysers.Contains(BuildingType))
                worker.Order(Units.GetAbilityId(BuildingType), FindGasPlacement());
            else
                worker.Order(Units.GetAbilityId(BuildingType), FindPlacement(BuildingType));
        }

        /// <summary>
        /// A function that gets a valid location for a structure of the type specified in the main base
        /// </summary>
        /// <param name="unitType">The desired building type</param>
        /// <returns>a valid location of where the building will fit, Exception if placement cannot be found</returns>
        public static Point2D FindPlacement(uint unitType)
        {
            BaseLocation startLocation = VBot.Bot.Map.StartLocation;
            Point startingSpot = startLocation.Location;
            Point2D constructionSpot;
            const int radius = 12;

            if (Units.ResourceCenters.Contains(unitType))
            {
                Point p = VBot.Bot.Map.GetExpansionLocation();
                return new Point2D { X = p.X, Y = p.Y };
            }
                
            //trying to find a valid construction spot
            List<Point> mineralFields = new List<Point>();
            foreach (MineralField mf in startLocation.MineralPatches)
                mineralFields.Add(mf.Location);

            int counter = 0;
            while (true)
            {
                constructionSpot = new Point2D();
                Random random = new Random();
                constructionSpot.X = startingSpot.X + random.Next(-radius, radius + 1);
                constructionSpot.Y = startingSpot.Y + random.Next(-radius, radius + 1);

                //avoid building in the mineral line
                if (IsInRange(constructionSpot, mineralFields, 5)) continue;

                //check if the building fits
                if (!CanPlace(unitType, constructionSpot)) continue;
                counter++;
                if (counter > 50)
                {
                    Logger.Error("unable to place Building: {0}", VBot.Bot.Data.Units[(int)unitType].Name);
                    return null;
                }
                
                //ok, we found a spot
                break;
            }
            return constructionSpot;
        }

        /// <summary>
        /// A function that gets a valid location for a structure of the type specified in the main base
        /// </summary>
        /// <param name="unitType">the type for a gas geyser</param>
        /// <returns>the tag of the unbuild geyser to build a gas on, 0 if none are found</returns>
        public static ulong FindGasPlacement()
        {
            ulong constructionTag = 0;
            foreach (var rc in GetAgents(Units.ResourceCenters))
                foreach (var gas in GetUnits(Units.GasGeysers))
                    if (gas.UnitType != Units.EXTRACTOR && DistanceBetweenSq(rc.Unit.Pos, gas.Pos) < 81)
                        constructionTag = gas.Tag;
            return constructionTag;

        }

        /// <summary>
        /// This methods gets the square of the 2D distance between two points
        /// </summary>
        /// <param name="p1">location of first point</param>
        /// <param name="p2">locatoin of second point</param>
        /// <returns>the square of the distance between two point</returns>
        public static float DistanceBetweenSq(Point2D p1, Point2D p2)
        {
            return (p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y);
        }

        /// <summary>
        /// This methods gets the square of the 2D distance between two points
        /// </summary>
        /// <param name="p1">location of first point</param>
        /// <param name="p2">locatoin of second point</param>
        /// <returns>the square of the distance between two point</returns>
        public static float DistanceBetweenSq(Point p1, Point2D p2)
        {
            return DistanceBetweenSq(new Point2D { X = p1.X, Y = p1.Y }, p2);
        }

        /// <summary>
        /// This methods gets the square of the 2D distance between two points
        /// </summary>
        /// <param name="p1">location of first point</param>
        /// <param name="p2">locatoin of second point</param>
        /// <returns>the square of the distance between two point</returns>
        public static float DistanceBetweenSq(Point2D p1, Point p2)
        {
            return DistanceBetweenSq(p2,p1);
        }

        /// <summary>
        /// This methods gets the square of the 2D distance between two points
        /// </summary>
        /// <param name="p1">location of first point</param>
        /// <param name="p2">locatoin of second point</param>
        /// <returns>the square of the distance between two point</returns>
        public static float DistanceBetweenSq(Point p1, Point p2)
        {
            return DistanceBetweenSq(new Point2D { X = p1.X, Y = p1.Y }, p2);
        }

        /// <summary>
        /// Checks if a point is within a certain radius of a collection of points 
        /// </summary>
        /// <param name="point">point to check if it is in range of a collection</param>
        /// <param name="patches">the collection of points to compare the first point to</param>
        /// <param name="distance">the radius around the point to check if in range</param>
        /// <returns>true if the point and a patch are within distance, false otherwise</returns>
        public static bool IsInRange(Point2D point, List<Point> patches, float distance)
        {
            foreach (var mp in patches)
                if (DistanceBetweenSq(mp, point) < distance * distance)
                    return true;
            return false;
        }

        /// <summary>
        /// a function to check if there is an available building to train a unit
        /// </summary>
        /// <param name="unitType">desired unit to be trained</param>
        /// <returns>true if an idle building is waiting, false otherwise</returns>
        public static bool TrainingBuildingAvailable(uint unitType)
        {
            HashSet<uint> hs = TrainHelper.GetTrainingBuildingTypes(unitType);
            if (VBot.Bot.StateManager.GetAvailableAgent(hs) != null)
                return true;
            return false;
        }

        /// <summary>
        /// a function to check if there is an available building to research an upgrade
        /// </summary>
        /// <param name="upgradeId">desired upgrade to be researched</param>
        /// <returns>true if an idle building is waiting, false otherwise</returns>
        public static bool TrainingBuildingAvailable(int upgradeId)
        {
            HashSet<uint> hs = UpgradeHelper.GetUpgradeBuildingTypes(upgradeId);

            if (VBot.Bot.StateManager.GetAvailableAgent(hs) != null)
                return true;
            return false;
        }

        /// <summary>
        /// Checks the StateManger to see if a agent still exist and returns it
        /// </summary>
        /// <param name="tag">the tag of the desired agent</param>
        /// <returns>the agent if it exist, null otherwise</returns>
        public static Agent GetAgentByTag(ulong tag)
        {
            return VBot.Bot.StateManager.GetAgentByTag(tag);
        }

        /// <summary>
        /// Get a location to place a creep tumor, heading towards the enemy base
        /// </summary>
        /// <param name="startPoint">the location of the queen or tumor that spreads the creep. MUST BE A PATHABLE TILE</param>
        /// <param name="range">how far from the startpoint it can spread to. </param>
        /// <returns>a location that is valid for a tumor to be placed. Null if none can be found</returns>
        public static Point2D GetTumorLocation(Point2D startPoint, int range)
        {
            return GetTumorLocation(startPoint, VBot.Bot.Map.EnemyStartLocations[0], range);
        }

        /// <summary>
        /// Get a location to place a creep tumor
        /// </summary>
        /// <param name="startPoint">the location of the queen or tumor that spreads the creep. MUST BE A PATHABLE TILE</param>
        /// <param name="towards">the direction of the desired creepspread</param>
        /// <param name="range">how far from the startpoint it can spread to.</param>
        /// <returns>a location that is valid for a tumor to be placed. Null if none can be found</returns>
        public static Point2D GetTumorLocation(Point2D startPoint, Point2D towards, int range)
        {
            int[,] distances;
            if (towards == VBot.Bot.Map.EnemyStartLocations[0])
                distances = VBot.Bot.Map.DistancesToEnemy;
            else
                distances = VBot.Bot.Map.GenerateDistances(towards);

            Point2D closestPoint = null;
            int minDist = 100000000;

            for (int i = -range; i <= range; i++)
            {
                for (int j = -range; j < range+1; j++)
                {
                    // check if it is creep
                    var creepMap = VBot.Bot.Observation.Observation.RawData.MapState.Creep;
                    Point2D testLoc = new Point2D { X = startPoint.X + i, Y = startPoint.Y + j };
                    if (!Sc2Util.ReadTile(creepMap, testLoc))
                        continue;

                    if (distances[(int)testLoc.X,(int)testLoc.Y] < minDist)
                    {
                        minDist = distances[(int)testLoc.X, (int)testLoc.Y];
                        closestPoint = testLoc;
                    }
                }
            }
            if (closestPoint == null)
                return null;
            return closestPoint;
                
        }

        /// <summary>
        /// Checks if an upgrade is completed or being researched
        /// </summary>
        /// <param name="upgradeId">id of the desired upgrade</param>
        /// <param name="onlyCompleted">default is false</param>
        /// <returns>true if upgrade has started, or completed in onlyCompleted = true. False if upgrade not started</returns>
        public static bool CheckUpgrade(int upgradeId, bool onlyCompleted = false)
        {
            if (onlyCompleted)
                return VBot.Bot.StateManager.CheckUpgradeFinished(upgradeId);
            else 
                return VBot.Bot.StateManager.CheckUpgradeFinished(upgradeId) && VBot.Bot.StateManager.CheckUpgradeInProgress(upgradeId);;
        }

        /// <summary>
        /// This function ensures drones are harvesting optimally across different bases
        /// </summary>
        public static void DistributeWorkers()
        {
            List<Agent> resourceCenters = GetAgents(Units.ResourceCenters);
            List<Agent> Geysers = GetAgents(Units.GasGeysers);
            List<Agent> Workers = GetAgents(Units.Workers).ToList();
            bool oversaturatedGases = false;
            // step 1 - add drones to gas
            int workersOnGas = 0;
            int maxGasPotential = 0;
            foreach (var gas in Geysers)
            {
                if (gas.Unit.AssignedHarvesters > gas.Unit.IdealHarvesters)
                    oversaturatedGases = true;

                workersOnGas += gas.Unit.AssignedHarvesters;
                maxGasPotential += gas.Unit.IdealHarvesters;
            }
            if (workersOnGas < VBot.Bot.Build.IdealGasWorkers && workersOnGas < maxGasPotential)
            {
                int idealWorkerCount = maxGasPotential < VBot.Bot.Build.IdealGasWorkers
                    ? maxGasPotential
                    : VBot.Bot.Build.IdealGasWorkers;

                int workersToTransfer = idealWorkerCount - workersOnGas;
                Queue<Agent> agentsForGas = new Queue<Agent>();
                for (int i = 0; i < workersToTransfer; i++)
                {
                    var newWorker = GetWorker();
                    if (newWorker == null)
                        continue;
                    agentsForGas.Enqueue(newWorker);
                    newWorker.Busy = true;
                }

                foreach (Agent gas in Geysers)
                {
                    int transfers = gas.Unit.IdealHarvesters - gas.Unit.AssignedHarvesters;
                    for (int i = 0; i < transfers; i++)
                        if (agentsForGas.Count > 0)
                            agentsForGas.Dequeue().Order(Abilities.SMART, gas.Unit.Tag);
                }
            }

            // step 2 - remove drones from oversaturated gas
            if (oversaturatedGases)
            {
                foreach (var gas in Geysers)
                {
                    if (gas.Unit.IdealHarvesters < gas.Unit.AssignedHarvesters)
                    {
                        List<Agent> workersToRemove = new List<Agent>();
                        for (int i = 0; i < gas.Unit.AssignedHarvesters - gas.Unit.IdealHarvesters; i++)
                        {
                            Agent worker = Workers
                                .Where(w => w.Unit.Orders.Count() > 0)
                                .Where(w => w.Unit.Orders[0].TargetUnitTag == gas.Unit.Tag).FirstOrDefault();
                            worker.Busy = false;
                            workersToRemove.Add(worker);
                        }

                        ulong mineralTag = GetUnits(Units.MineralFields)
                            .Where(m => DistanceBetweenSq(m.Pos, gas.Unit.Pos) < 100)
                            .FirstOrDefault().Tag;
                        var action = new ActionRawUnitCommand
                        {
                            AbilityId = (int)Abilities.SMART,
                            TargetUnitTag = mineralTag,
                        };
                        action.UnitTags.AddRange(workersToRemove.Select(w => w.Unit.Tag));
                        VBot.Bot.AddAction(action);
                        return;
                    }
                }

            }

            // step 3 - remove drones if over ideal drone count
            if (workersOnGas > VBot.Bot.Build.IdealGasWorkers)
            {
                int workersToRemoveCount = workersOnGas - VBot.Bot.Build.IdealGasWorkers;
                List<Agent> workersToRemove = new List<Agent>();
                ulong mineralTag = 0;
                while (workersToRemoveCount > 0)
                {
                    workersToRemoveCount--;
                    foreach (Agent gas in Geysers)
                    {
                        if (gas.Unit.AssignedHarvesters == 0)
                            continue;

                        Agent worker = Workers
                            .Where(w => w.Unit.Orders.Count() > 0)
                            .Where(w => w.Unit.Orders[0].TargetUnitTag == gas.Unit.Tag).FirstOrDefault();
                        if (worker == null)
                            return;
                        workersToRemove.Add(worker);
                        mineralTag = GetUnits(Units.MineralFields)
                            .Where(m => DistanceBetweenSq(m.Pos, gas.Unit.Pos) < 100)
                            .FirstOrDefault().Tag;
                    }
                }
                ActionRawUnitCommand action = new ActionRawUnitCommand
                {
                    AbilityId = (int)Abilities.SMART,
                    TargetUnitTag = mineralTag,
                };
                action.UnitTags.AddRange(workersToRemove.Select(w => w.Unit.Tag));
                VBot.Bot.AddAction(action);
            }

            
        }

        /// <summary>
        /// Gets a list of all the known SC2Api.Units that the enemy owns within the matching hashset
        /// </summary>
        /// <param name="unitTypes">a hashset of the desired units</param>
        /// <returns>a list of matching SC2Api units controlled by the enemy</returns>
        public static List<Unit> GetEnemyUnits(HashSet<uint> unitTypes)
        {
            return VBot.Bot.Observation.Observation.RawData.Units
                .Where(u => u.Alliance == Alliance.Enemy && unitTypes.Contains(u.UnitType))
                .ToList();
        }

        /// <summary>
        /// Gets a list of all the known SC2Api.Units that the enemy owns within the matching unitType
        /// </summary>
        /// <param name="unitType">the id of a desired unit</param>
        /// <returns>a list of matching SC2Api units controlled by the enemy</returns>
        public static List<Unit> GetEnemyUnits(uint unitType)
        {
            return GetEnemyUnits(new HashSet<uint> { unitType });
        }
        
        /// <summary>
        /// Gets a list of all the known SC2Api.Units that the enemy owns
        /// </summary>
        /// <returns>all known SC2Api units controlled by the enemy</returns>
        public static List<Unit> GetEnemyUnits()
        {
            return VBot.Bot.Observation.Observation.RawData.Units
                .Where(u => u.Alliance == Alliance.Enemy)
                .ToList();
        }

        public static int SupplyOf(HashSet<uint> units)
        {
            float sup = 0;
            foreach (var unit in units)
                sup += VBot.Bot.Data.Units[(int)unit].FoodRequired * VBot.Bot.StateManager.GetCompletedCount(unit);

            return (int)Math.Ceiling(sup);
        }

        public static bool HasVision(Point2D loc)
        {
            return Sc2Util.ReadTile(VBot.Bot.Observation.Observation.RawData.MapState.Visibility, loc);
        }

    }
}