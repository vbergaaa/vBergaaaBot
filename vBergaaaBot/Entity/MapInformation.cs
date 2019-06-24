using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SC2APIProtocol;
using vBergaaaBot.Entity;

namespace vBergaaaBot
{
    public class MapInformation
    {
        public List<BaseLocation> BaseLocations = new List<Entity.BaseLocation>();
        public Entity.BaseLocation StartLocation;
        public List<Point2D> EnemyStartLocations = new List<Point2D>();


        public void Analyse(VBergaaaBot vBergaaaBot)
        {
            Controller.OpenFrame();
            //
            // get the start location
            //
            foreach (Unit unit in Controller.GetUnits(Units.ResourceCenters))
                StartLocation = new Entity.BaseLocation { Location = unit.Pos };

            //
            // get location of all the bases
            //

            // get location of each mineral field and vespene geyser
            List<Entity.MineralField> mineralFields = new List<Entity.MineralField>();
            foreach (Unit mf in Controller.GetNeutralUnits(Units.MineralFields))
                mineralFields.Add(new Entity.MineralField { Location = mf.Pos, UnitType = mf.unitType });
            List<Entity.VespeneGeyser> vespeneGeysers = new List<Entity.VespeneGeyser>();
            foreach (Unit vg in Controller.GetNeutralUnits(Units.GasGeysers))
                vespeneGeysers.Add(new Entity.VespeneGeyser { Location = vg.Pos, UnitType = vg.unitType });
            Logger.Info(vespeneGeysers.Count + " vespene geysers found");

            //group them into set of mineral fields
            List<Entity.MineralField> checkedMineralFields = new List<Entity.MineralField>();
            List<Entity.VespeneGeyser> checkedVespeneGeysers = new List<Entity.VespeneGeyser>();
            int currentSet = 0;
            foreach (Entity.MineralField mf in mineralFields)
            {
                if (checkedMineralFields.Contains(mf)) // checks if mf has been used and skips if true
                    continue;
                BaseLocations.Add(new Entity.BaseLocation());
                checkedMineralFields.Add(mf); // adds mf to checklist and current set
                BaseLocations[currentSet].MineralPatches.Add(mf);
                // gets rest of local mfs to add to set and check
                for (int i = 0; i < BaseLocations[currentSet].MineralPatches.Count; i++)
                {
                    Entity.MineralField mfLocal = BaseLocations[currentSet].MineralPatches[i];
                    foreach (Entity.MineralField mf2 in mineralFields)
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
            foreach (Entity.BaseLocation baseLoc in BaseLocations)
            {
                foreach (Entity.VespeneGeyser vg in vespeneGeysers)
                {
                    if (checkedVespeneGeysers.Contains(vg))
                        continue;

                    foreach (Entity.MineralField mf in baseLoc.MineralPatches)
                    {
                        if (GetDistance2D(mf.Location,vg.Location)<10)
                        {
                            checkedVespeneGeysers.Add(vg);
                            baseLoc.VespeneGeysers.Add(vg);
                            break;
                        }
                    }
                }
            }            

            // get average position of each mineral cluster
            foreach (Entity.BaseLocation baseLocation in BaseLocations)
            {
                // find average location of cluster
                float x = 0;
                float y = 0;

                foreach (Entity.MineralField mf in baseLocation.MineralPatches)
                {
                    x += mf.Location.X;
                    y += mf.Location.Y;
                }
                foreach (Entity.VespeneGeyser vg in baseLocation.VespeneGeysers)
                {
                    x += vg.Location.X;
                    y += vg.Location.Y;
                }
                // average cluster location 
                float avgX = x / (baseLocation.MineralPatches.Count + baseLocation.VespeneGeysers.Count);
                float avgY = y / (baseLocation.MineralPatches.Count + baseLocation.VespeneGeysers.Count);
                Point2D averageOfCluster = new Point2D { X = avgX, Y = avgY };

                avgX = (int)avgX + 0.5f;
                avgY = (int)avgX + 0.5f;

                // check placement of surrounding tiles
                Point2D tempLoc = null;
                float closestDist = 1000000;
                for (int i = 0; i < 20; i++)
                {
                    for (int j = 0; j <= i; j++)
                    {
                        float maxDist;
                        Point2D newPos;
                        newPos = new Point2D { X = averageOfCluster.X + i - j, Y = averageOfCluster.Y + j };
                        maxDist = checkPosition(newPos, baseLocation);
                        if (maxDist < closestDist)
                        {
                            tempLoc = newPos;
                            closestDist = maxDist;
                        }

                        newPos = new Point2D { X = averageOfCluster.X + i - j, Y = averageOfCluster.Y - j };
                        maxDist = checkPosition(newPos, baseLocation);
                        if (maxDist < closestDist)
                        {
                            tempLoc = newPos;
                            closestDist = maxDist;
                        }

                        newPos = new Point2D { X = averageOfCluster.X - i + j, Y = averageOfCluster.Y + j };
                        maxDist = checkPosition(newPos, baseLocation);
                        if (maxDist < closestDist)
                        {
                            tempLoc = newPos;
                            closestDist = maxDist;
                        }

                        newPos = new Point2D { X = averageOfCluster.X - i + j, Y = averageOfCluster.Y - j };
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
            List<BaseLocation> orderedLocations = BaseLocations.OrderBy(b => GetDistance2D(b.Location, StartLocation.Location)).ToList();
            BaseLocations = orderedLocations;

            // get enemy locations
            foreach (Point2D startLocation in Controller.gameInfo.StartRaw.StartLocations)
            {
                if (GetDistance2D(StartLocation.Location, startLocation) < 10)
                    continue;
                EnemyStartLocations.Add(startLocation);
            }
        }

        public static float GetDistance2D(Point2D p1, Point2D p2)
        {
            // function that returns the distance of two given points based on there x/y coords
            float p1x = p1.X;
            float p1y = p1.Y;
            float p2x = p2.X;
            float p2y = p2.Y;

            return (float) Math.Sqrt((p1x - p2x) * (p1x - p2x) + (p1y - p2y) * (p1y - p2y));
        }
        public static float GetDistance2D(Point2D p1, Point p2)
        {
            Point2D p3 = new Point2D { X = p2.X, Y = p2.Y };
            return GetDistance2D(p1, p3);
        }
        public Point2D GetExpansionLocation()
        {
            
            // get all resource centers
            List<Point2D> rcLocations = Controller.GetUnits(Units.ResourceCenters).Select(u=>u.Pos).ToList();
            foreach (BaseLocation b in BaseLocations)
            {
                Point2D nearestRC = rcLocations.OrderBy(rc => GetDistance2D(rc, b.Location)).FirstOrDefault();
                if (GetDistance2D(nearestRC,b.Location) < 10 )
                    continue;
                return b.Location;
            }
            return null;
        }
        private float checkPosition(Point2D loc, Entity.BaseLocation baseLoc)
        {
            foreach (Entity.MineralField mf in baseLoc.MineralPatches) 
                if (Math.Abs(mf.Location.X - loc.X) <= 5.5 && Math.Abs(mf.Location.Y - loc.Y) <= 5.5)
                    return 1000000;
            foreach (Entity.VespeneGeyser vg in baseLoc.VespeneGeysers) 
                if (Math.Abs(vg.Location.X - loc.X) <= 5.5 && Math.Abs(vg.Location.Y - loc.Y) <= 6.1)
                    return 1000000;

            // check area is big enough?  !SC2Util.GetTilePlacable((int)System.Math.Round(pos.X + x), (int)System.Math.Round(pos.Y + y))

            // clear of errors, get total distance from each mf to location
            float maxDistance = 0;
            foreach (Entity.MineralField mf in baseLoc.MineralPatches)
                maxDistance += GetDistance2D(mf.Location, loc);
            foreach (Entity.VespeneGeyser vg in baseLoc.VespeneGeysers)
                maxDistance += GetDistance2D(vg.Location, loc);
            return maxDistance;
        }
    }
}
