using System;
using System.Collections.Generic;

namespace CSharp
{
    public class Game
    {
        public const int MapWidth = 5;
        public const int MapHeight = 5;
        public static readonly HashSet<int> RulesStayAlive = new HashSet<int>(new[] { 2, 3, 5 });
        public static readonly HashSet<int> RulesBecomeAlive = new HashSet<int>(new[] { 0, 3 });

        public bool[,] Map { get; private set; }

        public Game()
        {
            Map = new bool[MapWidth, MapHeight];
        }

        public void PlayStep()
        {
            var temp = new bool[MapWidth, MapHeight];
            Array.Copy(Map, temp, Map.Length);

            for (int x = 0; x < MapWidth; x++)
            {
                for (int y = 0; y < MapHeight; y++)
                {
                    var currentCell = temp[x, y];
                    var neighborsCount = GetLivingNeighbors(temp, x, y);

                    if (currentCell && !RulesStayAlive.Contains(neighborsCount))
                    {
                        Map[x, y] = false;
                    }
                    else if (!currentCell && RulesBecomeAlive.Contains(neighborsCount))
                    {
                        Map[x, y] = true;
                    }
                }
            }
        }

        private static int GetLivingNeighbors(bool[,] map, int x, int y)
        {
            int count = 0;

            // Top-Left
            if (x > 0 && y > 0 && map[x-1, y-1])
            {
                count++;
            }

            // Top
            if (y > 0 && map[x, y - 1])
            {
                count++;
            }

            // Top-Right
            if (x < MapWidth - 1 && y > 0 && map[x + 1, y - 1])
            {
                count++;
            }

            // Left
            if (x > 0 && map[x - 1, y])
            {
                count++;
            }

            // Right
            if (x < MapWidth - 1 && map[x + 1, y])
            {
                count++;
            }

            // Bottom-Left
            if (x > 0 && y < MapHeight - 1 && map[x - 1, y + 1])
            {
                count++;
            }

            // Bottom
            if (y < MapHeight - 1 && map[x, y + 1])
            {
                count++;
            }

            // Bottom-Right
            if (x < MapWidth - 1 && y < MapHeight - 1 && map[x + 1, y + 1])
            {
                count++;
            }

            return count;
        }
    }
}
