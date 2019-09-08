using System;
using System.Collections.Generic;
using SC2APIProtocol;

namespace vBergaaaBot
{
    internal class Program
    {
        // Settings for your bot.
        private static readonly Bot bot = new VBot();
        private const Race race = Race.Zerg;

        // Settings for single player mode.
        //        private static string mapName = ;


        private static readonly Race opponentRace = Race.Random;
        private static readonly Difficulty opponentDifficulty = Difficulty.VeryHard;

        public static GameConnection gc;

        private static void Main(string[] args)
        {
            
            gc = new GameConnection();
            int gameCount = 0;
                
            if (args.Length == 0)
            {
                while (gameCount < 1) // change this to chain games automatically
                {
                    try
                    {
                        gameCount++;
                        gc.readSettings();
                        gc.RunSinglePlayer(bot, GetMapName(), race, opponentRace, opponentDifficulty).Wait();
                    }
                    catch (Exception ex)
                    {
                        Logger.Info(ex.ToString());
                    }
                }
            }
            else
                gc.RunLadder(bot, race, args).Wait();
            

            Logger.Info("Terminated.");
        }

        private static string GetMapName()
        {
            List<string> maps = new List<string>
            {
                //// old maps
                //"(2)16-BitLE.SC2Map",
                //"AbiogenesisLE.SC2Map",
                //"AbyssalReefLE.SC2Map",
                //"(2)AcidPlantLE",
                //"(2)CatalystLE",
                //"(2)DreamcatcherLE",
                //"(2)LostandFoundLE",
                //"(2)RedshiftLE",

                // 2019 Season 3
                "AcropolisLE.SC2Map",
                "DiscoBloodbathLE.SC2Map",
                "EphemeronLE.SC2Map",
                "ThunderbirdLE.SC2Map",
                "TritonLE.SC2Map",
                "WintersGateLE.SC2Map",
                "WorldofSleepersLE.SC2Map"
            };
            Random r = new Random();
            int i = r.Next(maps.Count);

            return maps[i];
        }
    }
}