﻿using System.Collections.Generic;
using System.Linq;

namespace vBergaaaBot {
    internal static class Abilities {
        //you can get all these values from the stableid.json file (just search for it on your PC)
        public static Dictionary<uint, uint> CreatesUnit = new Dictionary<uint, uint>();
        public static Dictionary<uint, uint> ResearchsUpgrade = new Dictionary<uint, uint>();

        public static uint SMART = 1;
        public static uint STOP = 4;
        public static uint ATTACK = 23;
        public static uint MOVE = 16;
        public static uint PATROL = 17;
        public static uint SALVAGE_BUNKER = 32;
        public static uint HARVEST_RETURN_MULE = 167;
        public static uint CALL_DOWN_MULE = 171;
        public static uint INJECT_LARVA = 251;
        public static uint HARVEST_GATHER_SCV = 295;
        public static uint HARVEST_RETURN_SCV = 296;
        public static uint HARVEST_GATHER_PROBE = 298;
        public static uint HARVEST_RETURN_PROBE = 299;
        public static uint CANCEL_CONSTRUCTION = 314;
        public static uint REPAIR = 316;
        public static uint SIEGE_TANK = 388;
        public static uint UNSIEGE_TANK = 390;
        public static uint SCANNER_SWEEP = 399;
        public static uint YAMATO = 401;
        public static uint UNLOAD_BUNKER = 408;
        public static uint DEPOT_RAISE = 558;
        public static uint DEPOT_LOWER = 556;
        public static uint RESEARCH_INFERNAL_PREIGNITER = 761;
        public static uint RESEARCH_BANSHEE_CLOAK = 790;
        public static uint MORPH_HATCHERY = 1152;
        public static uint MORPH_EXTRACTOR = 1154;
        public static uint HARVEST_GATHER_DRONE = 1183;
        public static uint HARVEST_RETURN_DRONE = 1184;
        public static uint RESEARCH_METABOLIC_BOOST = 1253;
        public static uint MORPH_DRONE = 1342;
        public static uint SPREAD_CREEP_QUEEN = 1694;
        public static uint SPREAD_CREEP_TUMOR = 1733;
        public static uint TRANSFORM_TO_HELLION = 1978;
        public static uint TRANSFORM_TO_HELLBAT = 1998;
        public static uint THOR_SWITCH_AP = 2362;
        public static uint THOR_SWITCH_NORMAL = 2364;
        public static uint REAPER_GRENADE = 2588;
        public static uint CANCEL = 3659;
        public static uint HARVEST_GATHER = 3666;
        public static uint HARVEST_RETURN = 3667;
        public static uint CANCEL_LAST = 3671;
        public static uint RALLY = 3673;
        public static uint CLOAK = 3676;
        public static uint LAND = 3678;
        public static uint LIFT = 3679;
        public static uint RESEARCH_UPGRADE_MECH_AIR = 3699;     
        public static uint RESEARCH_UPGRADE_MECH_ARMOR = 3700;   
        public static uint RESEARCH_UPGRADE_MECH_GROUND = 3701;

        // check if gold bases have same abilities codes
        public static HashSet<uint> MiningMinerals = new HashSet<uint>
        {
            HARVEST_GATHER,
            HARVEST_GATHER_DRONE,
            HARVEST_GATHER_PROBE,
            HARVEST_GATHER_SCV,
            HARVEST_RETURN,
            HARVEST_RETURN_DRONE,
            HARVEST_RETURN_MULE,
            HARVEST_RETURN_PROBE,
            HARVEST_RETURN_SCV,
        };
    }
}