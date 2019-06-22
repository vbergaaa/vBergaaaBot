using System;
using SC2APIProtocol;
using System.Collections.Generic;

namespace Bot {
    internal class Program {
        // Settings for your bot.
        private static readonly Bot bot = new VBergaaaBot();
        private const Race race = Race.Zerg;

        // Settings for single player mode.
        private static string mapName = RandomMap();

        private static readonly Race opponentRace = Race.Random;
        private static readonly Difficulty opponentDifficulty = Difficulty.VeryHard;

        public static GameConnection gc;

        private static void Main(string[] args) {
            try {
                gc = new GameConnection();
                if (args.Length == 0){
                    gc.readSettings();
                    gc.RunSinglePlayer(bot, mapName, race, opponentRace, opponentDifficulty).Wait();
                }
                else
                    gc.RunLadder(bot, race, args).Wait();
            }
            catch (Exception ex) {
                Logger.Info(ex.ToString());
            }

            Logger.Info("Terminated.");
        }

        private static string RandomMap()
        {
            List<string> maps = new List<string>();

            maps.Add(@"AcidPlantLE.SC2Map");
            maps.Add(@"(2)16-BitLE.SC2Map");
            maps.Add(@"CatalystLE.SC2Map");
            maps.Add(@"(2)DreamcatcherLE.SC2Map");
            maps.Add(@"(2)RedshiftLE.SC2Map");
            maps.Add(@"(2)LostAndFoundLE.SC2Map");
            maps.Add(@"AcolyteLE.SC2Map");

            Random rand = new Random();
            return maps[rand.Next(maps.Count)];
        }
    }
}