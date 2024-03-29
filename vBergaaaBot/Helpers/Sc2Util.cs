﻿using Google.Protobuf;
using SC2APIProtocol;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vBergaaaBot.Helpers
{
    public static class Sc2Util
    {
        /// <summary>
        /// Takes a map and reads if a tile contains either creep or is pathable, depending on which map it takes
        /// </summary>
        /// <param name="pathingGrid">an ImageData of a 1bit/pixel map</param>
        /// <param name="x">distances from left side starting at 0</param>
        /// <param name="y">distances from top side starting at 0</param>
        /// <returns>a bool value if the map square is pathable/creeped etc.</returns>
        public static bool ReadTile(ImageData pathingGrid, int x, int y)
        {
            if (pathingGrid.BitsPerPixel == 1)
            {
                int pixelID = x / 8 + (pathingGrid.Size.Y - 1 - y) * pathingGrid.Size.X / 8;
                byte Byte = pathingGrid.Data[pixelID];
                var bits = new BitArray(new byte[] { Byte });
                return bits[7 - x % 8];
            }
            else
            {
                int pixelID = x + (pathingGrid.Size.Y - 1 - y) * pathingGrid.Size.X;
                byte Byte = pathingGrid.Data[pixelID];
                return ((int)Byte == 2);

            }
        }
        
        /// <summary>
        /// Takes a map and reads if a tile contains either creep or is pathable, depending on which map it takes
        /// </summary>
        /// <param name="pathingGrid">an ImageData of a 1bit/pixel map</param>
        /// <param name="tile">point of request</param>
        /// <returns>a bool value if the map square is pathable/creeped etc.</returns>
        public static bool ReadTile(ImageData pathingGrid, Point2D tile)
        {
            int x = (int)tile.X;
            int y = (int)pathingGrid.Size.Y - 1 - (int)tile.Y;
            return ReadTile(pathingGrid, x, y);
        }

        /// this method is only for testing. keeping it here to test creep spread later
        public static void DisplayMap(ImageData pathingGrid, ImageData visionGrid)
        {
            string msg = "";
            for (var j = 0; j < pathingGrid.Size.Y; j++)
            {
                for (var i = 0; i < pathingGrid.Size.X; i++)
                {
                    if (ReadTile(pathingGrid, new Point2D { X = i, Y = pathingGrid.Size.Y - j - 1 }) &&
                        ReadTile(visionGrid, new Point2D { X = i, Y = pathingGrid.Size.Y - j - 1 }))
                        msg += " ";
                    else if (ReadTile(pathingGrid, new Point2D { X = i, Y = pathingGrid.Size.Y - j - 1 }))
                        msg += "-";
                    else
                        msg += "X";
                }
                msg += "\n";
            }
            Logger.Info(msg);
        }

        public static Point2D To2D(Point p)
        {
            return new Point2D { X = p.X, Y = p.Y };
        }
    }
}
