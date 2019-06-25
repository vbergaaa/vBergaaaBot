using SC2APIProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vBergaaaBot;
using vBergaaaBot.Builds;

namespace vBergaaaBot.Helpers
{
    public static class BotHelper
    {
        public static BuildOrderStep GetNextStep(List<BuildOrderStep> buildOrder, List<BuildOrderStep> maxComp, VBergaaaBot bot)
        {
            foreach (BuildOrderStep step in buildOrder)
            {
                var qty = bot.InternalData.GetTotalUnitCount(step.Unit);
                // check if we want it.
                if (step.Type == BuildOrderType.Unit)  
                    if (qty < step.Qty)
                        return step;
                if (step.Type == BuildOrderType.Building)
                    if (qty < step.Qty)
                        return step;
                if (step.Type == BuildOrderType.Upgrade)
                    if (bot.InternalData.CheckUpgrade(step.Upgrade) != true)
                        return step;
            }
            int overlordsTraining = Controller.GetUnits(Units.OVERLORD_COCOON).Count;
            if (bot.Observation.Observation.PlayerCommon.FoodCap-bot.Observation.Observation.PlayerCommon.FoodUsed-5*overlordsTraining < 6)
            {
                return new BuildOrderStep(Units.OVERLORD);
            }
            foreach (BuildOrderStep step in maxComp)
            {
                var qty = bot.InternalData.GetTotalUnitCount(step.Unit);
                // check if we want it.
                if (step.Type == BuildOrderType.Unit)
                    if (qty < step.Qty)
                        return step;
                if (step.Type == BuildOrderType.Building)
                    if (qty < step.Qty)
                        return step;
                if (step.Type == BuildOrderType.Upgrade)
                    if (bot.InternalData.CheckUpgrade(step.Upgrade) != true)
                        return step;
            }

            return null;
        } 
        public static void ExecuteBuildOrderStep(BuildOrderStep step, VBergaaaBot bot)
        {
            
            if (step.Type == BuildOrderType.Unit)
            {
                if (Controller.CanAfford(step.Unit) && Controller.MeetsTechRequirements(step.Unit))
                {
                    // get morph step
                    MorphStep morph = MorphHelper.MorpSteps.Where(m => m.ToUnit == step.Unit).FirstOrDefault();
                    if (morph != null)
                    {
                        long trainTime = (long)(bot.Data.Units[(int)step.Unit].BuildTime);
                        Controller.GetUnits(morph.FromUnit)[0].Train(step.Unit);
                        bot.InternalData.AddPendingUnit(step.Unit, (long)bot.Observation.Observation.GameLoop + trainTime);
                    }
                        
                    //get train step (queens only for z)
                    TrainStep train = TrainHelper.TrainSteps.Where(m => m.Unit == step.Unit).FirstOrDefault();
                    if (train != null)
                    {
                        long trainTime = (long)(bot.Data.Units[(int)step.Unit].BuildTime);
                        Controller.GetUnits(train.FromBuildings, onlyIdle: true, onlyCompleted:true)[0].Train(step.Unit);
                        bot.InternalData.AddPendingUnit(step.Unit, (long)bot.Observation.Observation.GameLoop + trainTime);
                    }
                        
                    // log notice to update
                    if (train == null && morph == null)
                        Logger.Warning("No Morph/Train logic for unit {0}", Controller.GetUnitName(step.Unit));
                }
            }
            if (step.Type == BuildOrderType.Building)
            {
                if (Controller.CanAfford(step.Unit) && Controller.MeetsTechRequirements(step.Unit))
                {
                    // get drone
                    if (Units.ResourceCenters.Contains(step.Unit))
                    {
                        var droneTag = Controller.Construct(step.Unit, bot.MapInformation.GetExpansionLocation());
                        bot.InternalData.AddPendingUnit(step.Unit, droneTag);
                    }
                    else
                    {
                        var droneTag = Controller.Construct(step.Unit);
                        bot.InternalData.AddPendingUnit(step.Unit, droneTag);
                    }
                } 
            }
            if (step.Type == BuildOrderType.Upgrade)
            {
                if (Controller.CanAffordUpgrade(step.Upgrade))
                {
                    UpgradeStep upgrade = UpgradeHelper.UpgradeSteps.Where(u => u.Upgrade == step.Upgrade).FirstOrDefault();
                    if (upgrade != null)
                    {
                        // get available building
                        Unit unit = Controller.GetUnits(upgrade.FromBuilding, onlyIdle: true).FirstOrDefault();
                        if (unit!=null)
                        {
                            int abilityId = Abilities.GetResearchUpgradeId(step.Upgrade);
                            Controller.Upgrade(abilityId, unit);
                            bot.InternalData.AddUpgrade(step.Upgrade);
                        }
                    }
                    else
                        Logger.Warning("No upgrade info for {0}. Please add it to the UpgradeHelper Class", Controller.GetUpgradeName(step.Upgrade));
                    
                }
            }

        }
    }
}
