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
                // check if we want it.
                if (step.Type == BuildOrderType.Unit)  
                    if (bot.InternalData.GetUnitCount(step.Unit) < step.Qty)
                        return step;
                if (step.Type == BuildOrderType.Building)
                    if (bot.InternalData.GetBuildingCount(step.Unit) < step.Qty)
                        return step;
                if (step.Type == BuildOrderType.Upgrade)
                    if (bot.InternalData.CheckUpgrade(step.Upgrade) != true)
                        return step;
            }
            int overlordsTraining = Controller.GetUnits(Units.OVERLORD_COCOON).Count;
            if (bot.AvailableSupply-5*overlordsTraining < 6)
            {
                return new BuildOrderStep(Units.OVERLORD);
            }
            foreach (BuildOrderStep step in maxComp)
            {
                // check if we want it.
                if (step.Type == BuildOrderType.Unit)
                    if (bot.InternalData.GetUnitCount(step.Unit) < step.Qty)
                        return step;
                if (step.Type == BuildOrderType.Building)
                    if (bot.InternalData.GetBuildingCount(step.Unit) < step.Qty)
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
                        Controller.GetUnits(morph.FromUnit)[0].Train(step.Unit);
                        bot.InternalData.AddUnits(step.Unit);
                        bot.ReservedMinerals += (int)bot.Data.Units[(int)step.Unit].MineralCost;
                        bot.ReservedGas += (int)bot.Data.Units[(int)step.Unit].VespeneCost;
                        bot.ReservedSupply += (int)bot.Data.Units[(int)step.Unit].FoodRequired;
                    }
                        
                    //get train step (queens only for z)
                    TrainStep train = TrainHelper.TrainSteps.Where(m => m.Unit == step.Unit).FirstOrDefault();
                    if (train != null)
                    {
                        Controller.GetUnits(train.FromBuildings, onlyIdle: true, onlyCompleted:true)[0].Train(step.Unit);
                        bot.InternalData.AddUnits(step.Unit);
                        bot.ReservedMinerals += (int)bot.Data.Units[(int)step.Unit].MineralCost;
                        bot.ReservedGas += (int)bot.Data.Units[(int)step.Unit].VespeneCost;
                        bot.ReservedSupply += (int)bot.Data.Units[(int)step.Unit].FoodRequired;
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
                    if (Units.ResourceCenters.Contains(step.Unit))
                    {
                        bot.ReservedMinerals += (int)bot.Data.Units[(int)step.Unit].MineralCost;
                        bot.ReservedGas += (int)bot.Data.Units[(int)step.Unit].VespeneCost;
                        bot.ReservedSupply += (int)bot.Data.Units[(int)step.Unit].FoodRequired;
                        Controller.Construct(step.Unit, bot.MapInformation.GetExpansionLocation());
                        bot.InternalData.AddBuildings(step.Unit);
                    }
                    else
                    {
                        Controller.Construct(step.Unit);
                        bot.InternalData.AddBuildings(step.Unit);
                        bot.ReservedMinerals += (int)bot.Data.Units[(int)step.Unit].MineralCost;
                        bot.ReservedGas += (int)bot.Data.Units[(int)step.Unit].VespeneCost;
                        bot.ReservedSupply += (int)bot.Data.Units[(int)step.Unit].FoodRequired;
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
                            bot.ReservedMinerals += (int)bot.Data.Units[(int)step.Unit].MineralCost;
                            bot.ReservedGas += (int)bot.Data.Units[(int)step.Unit].VespeneCost;
                            bot.ReservedSupply += (int)bot.Data.Units[(int)step.Unit].FoodRequired;
                        }
                    }
                    else
                        Logger.Warning("No upgrade info for {0}. Please add it to the UpgradeHelper Class", Controller.GetUpgradeName(step.Upgrade));
                    
                }
            }
            
            if (Controller.GetActionsCount() == 0)
            {
                bot.ReservedMinerals = 0;
                bot.ReservedSupply = 0;
                bot.ReservedGas = 0;
            }
        }
    }
}
