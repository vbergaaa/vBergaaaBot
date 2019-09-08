using vBergaaaBot.Helpers;
using SC2APIProtocol;
using System;
using System.Collections.Generic;
using System.Linq;

namespace vBergaaaBot.Map
{
    internal class MapAnalyser
    {
        public Point2D TargetAttackLocation { get; set; }
        public List<Base> BaseLocations = new List<Base>();
        public Base StartLocation;
        public List<Point2D> EnemyStartLocations = new List<Point2D>();
        public int[,] DistancesToEnemy;


        public void Analyse(VBot bot)
        {
            //
            // get the start location
            //
            foreach (Agent unit in Controller.GetAgents(Units.ResourceCenters))
                StartLocation = new Base { Location = unit.Unit.Pos };

            //
            // get location of all the bases
            //

            // get location of each mineral field and vespene geyser
            List<MineralField> mineralFields = new List<MineralField>();
            foreach (Unit mf in Controller.GetUnits(Units.MineralFields))
                mineralFields.Add(new MineralField { Location = mf.Pos, UnitType = mf.UnitType });
            List<VespeneGeyser> vespeneGeysers = new List<VespeneGeyser>();
            foreach (Unit vg in Controller.GetUnits(Units.GasGeysers))
                vespeneGeysers.Add(new VespeneGeyser { Location = vg.Pos, UnitType = vg.UnitType });

            //group them into set of mineral fields
            List<MineralField> checkedMineralFields = new List<MineralField>();
            List<VespeneGeyser> checkedVespeneGeysers = new List<VespeneGeyser>();
            int currentSet = 0;
            foreach (MineralField mf in mineralFields)
            {
                if (checkedMineralFields.Contains(mf)) // checks if mf has been used and skips if true
                    continue;
                BaseLocations.Add(new Base());
                checkedMineralFields.Add(mf); // adds mf to checklist and current set
                BaseLocations[currentSet].MineralPatches.Add(mf);
                // gets rest of local mfs to add to set and check
                for (int i = 0; i < BaseLocations[currentSet].MineralPatches.Count; i++)
                {
                    MineralField mfLocal = BaseLocations[currentSet].MineralPatches[i];
                    foreach (MineralField mf2 in mineralFields)
                    {
                        if (checkedMineralFields.Contains(mf2)) // potiental source of error - mf2 =/= mf
                            continue;
                        if (GetDistance2D(mf2.Location, mfLocal.Location) > 5) // skip if not within 5
                            continue;
                        // we want to add this minefield to our mf set and checked list
                        BaseLocations[currentSet].MineralPatches.Add(mf2);
                        checkedMineralFields.Add(mf2);
                    }
                }
                currentSet++;
            }

            // add gas geysers
            foreach (Base baseLoc in BaseLocations)
            {
                foreach (VespeneGeyser vg in vespeneGeysers)
                {
                    if (checkedVespeneGeysers.Contains(vg))
                        continue;

                    foreach (MineralField mf in baseLoc.MineralPatches)
                    {
                        if (GetDistance2D(mf.Location, vg.Location) < 10)
                        {
                            checkedVespeneGeysers.Add(vg);
                            baseLoc.VespeneGeysers.Add(vg);
                            break;
                        }
                    }
                }
            }

            // get average position of each mineral cluster
            foreach (Base baseLocation in BaseLocations)
            {
                // find average location of cluster
                float x = 0;
                float y = 0;

                foreach (MineralField mf in baseLocation.MineralPatches)
                {
                    x += mf.Location.X;
                    y += mf.Location.Y;
                }
                foreach (VespeneGeyser vg in baseLocation.VespeneGeysers)
                {
                    x += vg.Location.X;
                    y += vg.Location.Y;
                }
                // average cluster location 
                float avgX = x / (baseLocation.MineralPatches.Count + baseLocation.VespeneGeysers.Count);
                float avgY = y / (baseLocation.MineralPatches.Count + baseLocation.VespeneGeysers.Count);
                Point averageOfCluster = new Point { X = avgX, Y = avgY };

                avgX = (int)avgX + 0.5f;
                avgY = (int)avgX + 0.5f;

                // check placement of surrounding tiles
                Point tempLoc = null;
                float closestDist = 1000000;
                for (int i = 0; i < 20; i++)
                {
                    for (int j = 0; j <= i; j++)
                    {
                        float maxDist;
                        Point newPos;
                        newPos = new Point { X = averageOfCluster.X + i - j, Y = averageOfCluster.Y + j };
                        maxDist = checkPosition(newPos, baseLocation);
                        if (maxDist < closestDist)
                        {
                            tempLoc = newPos;
                            closestDist = maxDist;
                        }

                        newPos = new Point { X = averageOfCluster.X + i - j, Y = averageOfCluster.Y - j };
                        maxDist = checkPosition(newPos, baseLocation);
                        if (maxDist < closestDist)
                        {
                            tempLoc = newPos;
                            closestDist = maxDist;
                        }

                        newPos = new Point { X = averageOfCluster.X - i + j, Y = averageOfCluster.Y + j };
                        maxDist = checkPosition(newPos, baseLocation);
                        if (maxDist < closestDist)
                        {
                            tempLoc = newPos;
                            closestDist = maxDist;
                        }

                        newPos = new Point { X = averageOfCluster.X - i + j, Y = averageOfCluster.Y - j };
                        maxDist = checkPosition(newPos, baseLocation);
                        if (maxDist < closestDist)
                        {
                            tempLoc = newPos;
                            closestDist = maxDist;
                        }
                    }
                }

                // final tile placement should be tempLoc.. add it to base locatoins
                baseLocation.Location = tempLoc;
            }

            // order bases
            List<Base> orderedLocations = BaseLocations.OrderBy(b => GetDistance2D(b.Location, StartLocation.Location)).ToList();
            BaseLocations = orderedLocations;

            StartLocation.MineralPatches = BaseLocations[0].MineralPatches;
            StartLocation.VespeneGeysers = BaseLocations[0].VespeneGeysers;

            // get enemy locations
            foreach (Point2D startLocation in VBot.Bot.GameInfo.StartRaw.StartLocations)
            {
                if (GetDistance2D(StartLocation.Location, startLocation) < 10)
                    continue;
                EnemyStartLocations.Add(startLocation);
            }
            TargetAttackLocation = EnemyStartLocations[0];
            // generate distances from enemies main base
            DistancesToEnemy = GenerateDistances(EnemyStartLocations[0]);
            
        }

        public List<Point2D> GetScoutLocations ()
        {
            List<Point2D> baseLocations = BaseLocations
                .Select(b => Sc2Util.To2D(b.Location))
                .OrderBy(b=>WalkingDistanceFromEnemy(b))
                .ToList();
            return baseLocations;
        }

        public static float GetDistance2D(Point2D p1, Point2D p2)
        {
            // function that returns the distance of two given points based on there x/y coords
            float p1x = p1.X;
            float p1y = p1.Y;
            float p2x = p2.X;
            float p2y = p2.Y;

            return (float)Math.Sqrt((p1x - p2x) * (p1x - p2x) + (p1y - p2y) * (p1y - p2y));
        }
        public static float GetDistance2D(Point p1, Point p2)
        {
            Point2D p3 = new Point2D { X = p2.X, Y = p2.Y };
            return GetDistance2D(p3, p1);
        }
        public static float GetDistance2D(Point2D p1, Point p2)
        {
            Point2D p3 = new Point2D { X = p2.X, Y = p2.Y };
            return GetDistance2D(p1, p3);
        }
        public static float GetDistance2D(Point p1, Point2D p2)
        {
            return GetDistance2D(p2, p1);
        }
        public Point GetExpansionLocation()
        {

            // get all resource centers
            List<Point> rcLocations = Controller.GetUnits(Units.ResourceCenters).Select(u => u.Pos).ToList();
            foreach (Base b in BaseLocations)
            {
                Point nearestRC = rcLocations.OrderBy(rc => GetDistance2D(rc, b.Location)).FirstOrDefault();
                if (GetDistance2D(nearestRC, b.Location) < 10)
                    continue;
                return b.Location;
            }
            return null;
        }
        private float checkPosition(Point loc, Base baseLoc)
        {
            foreach (MineralField mf in baseLoc.MineralPatches)
                if (Math.Abs(mf.Location.X - loc.X) <= 5.5 && Math.Abs(mf.Location.Y - loc.Y) <= 5.5)
                    return 1000000;
            foreach (VespeneGeyser vg in baseLoc.VespeneGeysers)
                if (Math.Abs(vg.Location.X - loc.X) <= 5.5 && Math.Abs(vg.Location.Y - loc.Y) <= 6.1)
                    return 1000000;

            // check area is big enough?  !SC2Util.GetTilePlacable((int)System.Math.Round(pos.X + x), (int)System.Math.Round(pos.Y + y))

            // clear of errors, get total distance from each mf to location
            float maxDistance = 0;
            foreach (MineralField mf in baseLoc.MineralPatches)
                maxDistance += GetDistance2D(mf.Location, loc);
            foreach (VespeneGeyser vg in baseLoc.VespeneGeysers)
                maxDistance += GetDistance2D(vg.Location, loc);
            return maxDistance;
        }

        public int [,] GenerateDistances(Point2D p)
        {
            int x = (int)p.X;
            int y = (int)p.Y;
            var mapGrid = VBot.Bot.GameInfo.StartRaw.PathingGrid;

            int[,] distances = new int[mapGrid.Size.X, mapGrid.Size.Y];
            for (int i = 0; i < mapGrid.Size.X; i++)
            {
                for (int j = 0; j < mapGrid.Size.Y; j++)
                {
                    distances[i,j] = 100000000;
                }
            }

            distances[x, y] = 0;
            Queue<Point2D> q = new Queue<Point2D>();
            q.Enqueue(p);

            while (q.Count > 0)
            {
                Point2D nextPoint = q.Dequeue();
                expandQueue(q,nextPoint, mapGrid, distances,distances[(int)nextPoint.X,(int)nextPoint.Y]+1);
            }

            return distances;
        }

        private void expandQueue(Queue<Point2D> queue, Point2D point, ImageData map, int[,] distances, int newVal)
        {
            int x = (int)point.X;
            int y = (int)point.Y;
            List<Point2D> nearbyPoints = new List<Point2D>();
            nearbyPoints.Add(new Point2D { X = x + 1, Y = y });
            nearbyPoints.Add(new Point2D { X = x - 1, Y = y });
            nearbyPoints.Add(new Point2D { X = x, Y = y + 1 });
            nearbyPoints.Add(new Point2D { X = x, Y = y - 1 });
            
            foreach (Point2D p in nearbyPoints)
            {
                if (p.X < 0 || p.X > map.Size.X - 1 || p.Y < 0 || p.Y > map.Size.Y - 1)
                    continue;
                if (Sc2Util.ReadTile(map, (int)p.X, map.Size.Y - (int)p.Y - 1) && distances[(int)p.X,(int)p.Y] == 100000000)
                {
                    queue.Enqueue(p);
                    distances[(int)p.X, (int)p.Y] = newVal;
                }

            }

        }

        public int WalkingDistanceFromEnemy(Point2D p)
        {
            return DistancesToEnemy[(int)p.X, (int)p.Y];
        }
    }
}
