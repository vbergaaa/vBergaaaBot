using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SC2APIProtocol;
using VBergaaaBot.Agents;

namespace VBergaaaBot.MapInfo
{
    class MapAnalyzer
    {
        public List<MineralField> AllMineralFields { get; set; }
        public List<GasGeyser> AllGasGeysers { get; set; }
        public List<Base> Bases { get; set; }
        public Point2D StartLocation { get; set; }
        public List<Point2D> EnemyLocations { get; set; }

    
        public MapAnalyzer(VBergaaaBot bot)
        {
            // get minerals and geysers
            List<Unit> allUnits = bot.Observation.Observation.RawData.Units.ToList();
            AllMineralFields = allUnits
                .Where(u => Units.MineralFields.Contains(u.UnitType))
                .Select(u => new MineralField { Location = new Point2D { X = u.Pos.X, Y = u.Pos.Y } })
                .ToList();
            AllGasGeysers = allUnits
                .Where(u => Units.GasGeysers.Contains(u.UnitType))
                .Select(u => new GasGeyser { Location = new Point2D { X = u.Pos.X, Y = u.Pos.Y } })
                .ToList();
            StartLocation = allUnits
                .Where(u => Units.ResourceCenters.Contains(u.UnitType) && u.Owner == bot.PlayerId)
                .Select(u => new Point2D { X = u.Pos.X, Y = u.Pos.Y })
                .FirstOrDefault();
            
            // group minerals into sets
            Bases = new List<Base>();
            List<MineralField> checkedMineralFields = new List<MineralField>();
            List<GasGeyser> checkedVespeneGeysers = new List<GasGeyser>();

            // group all the minerals and geysers into bases
            foreach (MineralField mf in AllMineralFields)
            {
                if (checkedMineralFields.Contains(mf))
                    continue;
                Base tempBase = new Base();
                tempBase.MineralFields.Add(mf);
                checkedMineralFields.Add(mf);

                // add other mineral fields
                for (int i = 0; i < tempBase.MineralFields.Count; i++)
                {
                    MineralField mfLocal = tempBase.MineralFields[i];
                    foreach (MineralField mf2 in AllMineralFields)
                    {
                        if (checkedMineralFields.Contains(mf2))
                            continue;
                        if (DistanceSquared2D(mf2.Location, mfLocal.Location) > 25) // skip if not within 5
                            continue;
                        // we want to add this minefield to our mf set and checked list
                        tempBase.MineralFields.Add(mf2);
                        checkedMineralFields.Add(mf2);
                    }
                }

                // add nearby gas geysers
                foreach (MineralField mf2 in tempBase.MineralFields)
                {
                    foreach (GasGeyser g in AllGasGeysers)
                    {
                        if (checkedVespeneGeysers.Contains(g))
                            continue;
                        if (DistanceSquared2D(g.Location, mf2.Location) < 100)
                        {
                            tempBase.GasGeysers.Add(g);
                            checkedVespeneGeysers.Add(g);
                        }
                    }
                }
                Bases.Add(tempBase);

            }

            // get locations of each base
            foreach (Base b in Bases)
            {
                // get initial value
                float avgX = 0;
                float avgY = 0;
                foreach (MineralField mf in b.MineralFields)
                {
                    avgX += mf.Location.X;
                    avgY += mf.Location.Y;
                }
                foreach (GasGeyser g in b.GasGeysers)
                {
                    avgX += g.Location.X;
                    avgY += g.Location.Y;
                }

                avgX = avgX / (b.MineralFields.Count + b.GasGeysers.Count);
                avgY = avgY / (b.MineralFields.Count + b.GasGeysers.Count);
                avgX = (int)avgX + 0.5f;
                avgY = (int)avgY + 0.5f;
                Point2D avgLoc = new Point2D { X = avgX, Y = avgY };

                // check nearest tiles to ensure buildable spot
                Point2D tempLoc = null;
                float closestDist = 1000000;
                for (int i = 0; i < 20; i++)
                {
                    for (int j = 0; j <= i; j++)
                    {
                        float maxDist;
                        Point2D newPos = new Point2D { X = avgLoc.X + i - j, Y = avgLoc.Y + j };
                        maxDist = CheckPosition(newPos, b);
                        if (maxDist < closestDist)
                        {
                            tempLoc = newPos;
                            closestDist = maxDist;
                        }

                        newPos = new Point2D { X = avgLoc.X + i - j, Y = avgLoc.Y - j };
                        maxDist = CheckPosition(newPos, b);
                        if (maxDist < closestDist)
                        {
                            tempLoc = newPos;
                            closestDist = maxDist;
                        }

                        newPos = new Point2D { X = avgLoc.X - i + j, Y = avgLoc.Y + j };
                        maxDist = CheckPosition(newPos, b);
                        if (maxDist < closestDist)
                        {
                            tempLoc = newPos;
                            closestDist = maxDist;
                        }

                        newPos = new Point2D { X = avgLoc.X - i + j, Y = avgLoc.Y - j };
                        maxDist = CheckPosition(newPos, b);
                        if (maxDist < closestDist)
                        {
                            tempLoc = newPos;
                            closestDist = maxDist;
                        }
                    }
                }
                b.Location = tempLoc;
            }

            // get enemy base location(s)
            EnemyLocations = new List<Point2D>();
            foreach (Point2D location in bot.GameInfo.StartRaw.StartLocations)
                if (DistanceSquared2D(location, StartLocation) > 400)
                    EnemyLocations.Add(location);

            // order bases
            List<Base> OrderedBases = new List<Base>();
            foreach (Base b in Bases)
            {
                if (OrderedBases.Count == 0)
                {
                    OrderedBases.Add(b);
                    continue;
                }

                float distSquared = DistanceSquared2D(b.Location, StartLocation);
                    
                for (int i = 0; i < OrderedBases.Count; i++)
                {
                    if (distSquared < DistanceSquared2D(OrderedBases[i].Location,StartLocation))
                    {
                        OrderedBases.Insert(i, b);
                        break;
                    }
                }

                if (!OrderedBases.Contains(b))
                    OrderedBases.Add(b);
            }
            Bases = OrderedBases;
/*              

        // get nearest expansion and oponents bases
        NaturalLocation = null;
        float expansionDistance = 1000000;
        foreach (Entity.BaseLocation baseLocation in BaseLocations)
        {
            // natural   
            if (GetDistance2D(baseLocation.Location, StartLocation.Location) < 10)
            {
                StartLocation = baseLocation;
                continue;
            }

            if (GetDistance2D(baseLocation.Location, StartLocation.Location) < expansionDistance)
            {
                expansionDistance = GetDistance2D(baseLocation.Location, StartLocation.Location);
                NaturalLocation = baseLocation;
            }

            // enemy bases
            foreach (Point2D startLocation in Controller.gameInfo.StartRaw.StartLocations)
            {
                if (GetDistance2D(baseLocation.Location, startLocation) < 10)
                    if (GetDistance2D(baseLocation.Location, StartLocation.Location) > 30)
                        EnemyStartLocations.Add(baseLocation);
            }
        }
    }
    */
        }

        public Point2D GetNextBaseLocation(VBergaaaBot bot)
        {
            List<Unit> rcs = new List<Unit>();
            foreach (Unit unit in bot.Observation.Observation.RawData.Units)
                if (Units.ResourceCenters.Contains(unit.UnitType))
                    rcs.Add(unit);
            // get base locations
            IEnumerable<Point> rcLocations = rcs.Select(r => r.Pos);
            
            // get first base that doesnt have a rc center
            foreach (var b in Bases)
            {
                int nearbyBases = rcLocations.Where(r => DistanceSquared2D(r, b.Location) < 100).Count();
                if (nearbyBases == 0)
                    return b.Location;
            }
            return Bases[Bases.Count() - 2].Location;
        }

        public void PrintBaseLocationOrder()
        {
            string values = "";
            foreach (Base b in Bases)
                values += DistanceSquared2D(b.Location, StartLocation).ToString() + ", ";
            Logger.WriteLine(values);
        }
        private float CheckPosition(Point2D loc, Base baseLoc)
        {
            foreach (MineralField mf in baseLoc.MineralFields) 
                if (Math.Abs(mf.Location.X - loc.X) <= 5.5 && Math.Abs(mf.Location.Y - loc.Y) <= 5.5)
                    return 1000000;
            foreach (GasGeyser vg in baseLoc.GasGeysers) 
                if (Math.Abs(vg.Location.X - loc.X) <= 5.5 && Math.Abs(vg.Location.Y - loc.Y) <= 6.1)
                    return 1000000;

            // check area is big enough?  !SC2Util.GetTilePlacable((int)System.Math.Round(pos.X + x), (int)System.Math.Round(pos.Y + y))

            // clear of errors, get total distance from each mf to location
            float maxDistance = 0;
            foreach (MineralField mf in baseLoc.MineralFields)
                maxDistance += DistanceSquared2D(mf.Location, loc);
            foreach (GasGeyser vg in baseLoc.GasGeysers)
                maxDistance += DistanceSquared2D(vg.Location, loc);
            return maxDistance;
        }
        private static float DistanceSquared2D(Point p1, Point2D p2)
        {
            // function that returns the distance of two given points based on there x/y coords
            float p1x = p1.X;
            float p1y = p1.Y;
            float p2x = p2.X;
            float p2y = p2.Y;

            return (float)((p1x - p2x) * (p1x - p2x) + (p1y - p2y) * (p1y - p2y));
        }
        private static float DistanceSquared2D(Point2D p1, Point2D p2)
        {
            // function that returns the distance of two given points based on there x/y coords
            float p1x = p1.X;
            float p1y = p1.Y;
            float p2x = p2.X;
            float p2y = p2.Y;

            return (float)((p1x - p2x) * (p1x - p2x) + (p1y - p2y) * (p1y - p2y));
        }
    }
}
