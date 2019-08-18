
using vBergaaaBot.Tasks;
using System.Collections.Generic;
using vBergaaaBot.MicroControllers;
using vBergaaaBot.Managers;

namespace vBergaaaBot.Builds
{
    public abstract class Build
    {
        public List<BuildStep> MainBuild;
        public List<BuildStep> UpgradesBuild;
        public List<BuildStep> Opener;
        public List<BuildStep> ProduceList;
        public int IdealGasWorkers;
        public bool OpeningStage = true;
        public abstract string Name { get; }

        /// <summary>
        /// This method determines what the next macro action will be and executes it
        /// </summary>
        public void ExecuteNextStep()
        {
            if (OpeningStage)
            {
                int incr = 0;
                foreach (var step in Opener)
                {
                    if (incr == 7)
                    {

                    }
                    if (!step.CheckWaitFor())
                        break;
                    if (step.CheckQty())
                    {
                        step.CreateTask();
                        break;
                    }

                    incr++;
                }
                if (Opener.Count == incr)
                    OpeningStage = false;
            }
            
            if (!OpeningStage)
            {
                foreach (var step in MainBuild)
                {
                    if (step.CheckQty())
                    {
                        if (step.CheckWaitFor())
                            step.CreateTask();
                        break;
                    }
                }
                foreach (var step in UpgradesBuild)
                {
                    if (step.CheckQty())
                    {
                        step.CreateTask();
                        break;
                    }
                }
                Produce();
            }            
        }

        /// <summary>
        /// This outlines the overall tech that must be rebuilt if it gets destroyed and sets it to the MainBuild property
        /// </summary>
        /// <returns>A List of the main structures</returns>
        public abstract List<BuildStep> GetMainBuild();

        /// <summary>
        /// This outlines the upgrades for the build
        /// </summary>
        /// <returns>A list of the required upgrades</returns>
        public abstract List<BuildStep> GetUpgrades();

        public abstract List<BuildStep> GetOpener();

        public abstract List<BuildStep> GetProduceList();

        public abstract List<MicroController> AddControllers();

        internal abstract int SetGasWorkerCount();

        public virtual void OnFrame()
        {
            ExecuteNextStep();
        }

        public virtual void OnStart()
        {
            MainBuild = GetMainBuild();
            UpgradesBuild = GetUpgrades();
            Opener = GetOpener();
            ProduceList = GetProduceList();
            IdealGasWorkers = SetGasWorkerCount();
            foreach (MicroController mc in AddControllers())
                MicroManager.AddController(mc);
        }

        internal virtual void Produce()
        {
            // check for overlords, 
            if (VBot.Bot.GetAvaibleSupplyPending()<6 * Controller.GetCompletedCount(Units.ResourceCenters))
            {
                if (VBot.Bot.GameInfo.PlayerInfo[VBot.Bot.PlayerId].RaceActual == SC2APIProtocol.Race.Zerg)
                    MacroTask.MakeUnit(Units.OVERLORD);
                else
                    throw new System.Exception("impliment supply for t or p");
            }
            else 
                foreach (var step in ProduceList)
                    if (step.CheckQty())
                        step.CreateTask();
        }
    }
}
