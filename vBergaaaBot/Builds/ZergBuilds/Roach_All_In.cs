using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SC2APIProtocol;
using vBergaaaBot.Managers;

namespace vBergaaaBot.Builds
{
    public class Roach_All_In : Build
    {
        public override List<Manager> Managers { get; set; }
        public override string Name()
        {
            return "Roach_All_In";
        }
        public override List<BuildOrderStep> GetBuildOrder()
        {
            List<BuildOrderStep> bo = new List<BuildOrderStep>();
            bo.Add(new BuildOrderStep(Units.DRONE, 13));
            bo.Add(new BuildOrderStep(Units.OVERLORD, 2));
            bo.Add(new BuildOrderStep(Units.DRONE, 17));
            bo.Add(new BuildOrderStep(Units.EXTRACTOR, 1));
            bo.Add(new BuildOrderStep(Units.DRONE, 17));
            bo.Add(new BuildOrderStep(Units.SPAWNING_POOL, 1));
            bo.Add(new BuildOrderStep(Units.DRONE, 18));
            bo.Add(new BuildOrderStep(Units.HATCHERY, 2));
            bo.Add(new BuildOrderStep(Units.DRONE, 19));
            bo.Add(new BuildOrderStep(Units.QUEEN, 1));
            bo.Add(new BuildOrderStep(Units.ZERGLING, 1));
            bo.Add(new BuildOrderStep(Upgrades.ZERGLING_MOVE_SPEED, true));
            bo.Add(new BuildOrderStep(Units.ROACH_WARREN, 1));
            bo.Add(new BuildOrderStep(Units.DRONE, 19));
            bo.Add(new BuildOrderStep(Units.OVERLORD, 4));
            bo.Add(new BuildOrderStep(Units.ROACH, 8));
            bo.Add(new BuildOrderStep(Units.ZERGLING, 6));
            bo.Add(new BuildOrderStep(Units.OVERLORD, 5));
            bo.Add(new BuildOrderStep(Units.ZERGLING, 14));
            bo.Add(new BuildOrderStep(Units.OVERLORD, 6));
            bo.Add(new BuildOrderStep(Units.ZERGLING, 22));
            bo.Add(new BuildOrderStep(Units.OVERLORD, 8));
            bo.Add(new BuildOrderStep(Units.ZERGLING, 30));
            return bo;
        }
        public override List<BuildOrderStep> MaxOutComp()
        {
            List<BuildOrderStep> bo = new List<BuildOrderStep>();
            bo.Add(new BuildOrderStep(Units.DRONE, 19));
            bo.Add(new BuildOrderStep(Units.ZERGLING, 1000));
            return bo;
        }

        public override void OnFrame(VBergaaaBot bot)
        {
            foreach (var manager in Managers)
                manager.OnFrame(bot);
        }
        
        public override void OnStart(VBergaaaBot bot)
        {
            // load up managers
            Managers = new List<Manager>();
            Managers.Add(new GeneralBaseManager());
            Managers.Add(new MacroQueenManager());
            Managers.Add(new AttackManager(16));
            Managers.Add(new ScoutManager());
            //Managers.Add(new MacroQueenManager());
        }


    }
}
