using System;
using System.IO;

namespace aoc_2022_day12
{
    internal class Program
    {
        static void Main()
        {
            StreamReader sr = new StreamReader("input.txt");

            int minX = 50000; // horizontal
            int maxX = 0;
            int maxY = 0;

            List<List<Tuple<int, int>>> rockLines = new List<List<Tuple<int, int>>>();

            string? line;
            while ((line = sr.ReadLine()) != null)
            {
                string[] lineGroup = line.Split(" -> ");
                Tuple<int, int> prevPos = ParseTuple(lineGroup[0]);
                for (int i = 1; i < lineGroup.Count(); i++)
                {
                    List<Tuple<int, int>> pairs = new List<Tuple<int, int>>();
                    pairs.Add(prevPos);
                    Tuple<int, int> currentPos = ParseTuple(lineGroup[i]);
                    pairs.Add(currentPos);
                    rockLines.Add(pairs);
                    // Find min and max
                    minX = new[] { minX, currentPos.Item1, prevPos.Item1 }.Min();
                    maxX = new[] { maxX, currentPos.Item1, prevPos.Item1 }.Max();
                    maxY = new[] { maxY, currentPos.Item2, prevPos.Item2 }.Max();

                    prevPos = new Tuple<int, int>(currentPos.Item1, currentPos.Item2);

                }

            }

            // Question 2: we have to make the grid wider and two steps longer
            minX -= 200;
            maxX += 200;
            maxY += 2;

            // Initialise grid shifted to the left
            int[,] grid = new int[maxX - minX + 1, maxY + 1];

            foreach (List<Tuple<int, int>> pair in rockLines)
            {
                Tuple<int, int> pairOne = pair[0];
                Tuple<int, int> pairTwo = pair[1];

                // Vertical line
                if (pairOne.Item1 == pairTwo.Item1)
                {
                    int start = Math.Min(pairOne.Item2, pairTwo.Item2);
                    int stop = Math.Max(pairOne.Item2, pairTwo.Item2);
                    List<int> coordinates = Enumerable.Range(start, stop - start + 1).ToList();
                    int x = pairOne.Item1 - minX;
                    foreach (int coordinate in coordinates)
                    {
                        grid[x, coordinate] = 1;
                    }
                }
                // Horizontal line
                else
                {
                    int start = Math.Min(pairOne.Item1, pairTwo.Item1) - minX;
                    int stop = Math.Max(pairOne.Item1, pairTwo.Item1) - minX;
                    List<int> coordinates = Enumerable.Range(start, stop - start + 1).ToList();
                    int y = pairOne.Item2;
                    foreach (int coordinate in coordinates)
                    {
                        grid[coordinate, y] = 1;
                    }

                }
            }

            int sandUnits = 0;
            // Question 2: create bottom
            int xLength = grid.GetLength(0);
            for (int i = 0; i < xLength; i++)
            {
                grid[i, maxY] = 1;
            }

            // Question 2: let the sand flow            
            while (true)
            {
                FindNewPosition(grid, minX);
                sandUnits += 1;
                if (grid[500 - minX, 0] == 1)
                {
                    Console.WriteLine($"Finished after {sandUnits}x of sand.");
                    break;
                }
            }


            // Question 1: Let the sand flow
            while (true)
            {
                try
                {
                    FindNewPosition(grid, minX);
                    sandUnits += 1;
                }
                // if out of bounds, then you are flowing past
                catch
                {
                    Console.WriteLine($"Finished after {sandUnits}x of sand.");
                    break;
                }
            }


        }

        public static void FindNewPosition(int[,] grid, int minX)
        {
            bool changingPos = true;
            Tuple<int, int> tempPos = new Tuple<int, int>(500 - minX, 0);
            while (changingPos)
            {
                // going one down
                if (grid[tempPos.Item1, tempPos.Item2 + 1] != 1)
                {
                    tempPos = new Tuple<int, int>(tempPos.Item1, tempPos.Item2 + 1);
                }
                // going one diagonally left
                else if (grid[tempPos.Item1 - 1, tempPos.Item2 + 1] != 1)
                {
                    tempPos = new Tuple<int, int>(tempPos.Item1 - 1, tempPos.Item2 + 1);
                }
                // going one diagonally right
                else if (grid[tempPos.Item1 + 1, tempPos.Item2 + 1] != 1)
                {
                    tempPos = new Tuple<int, int>(tempPos.Item1 + 1, tempPos.Item2 + 1);
                }
                else
                {
                    // resting position found
                    grid[tempPos.Item1, tempPos.Item2] = 1;
                    changingPos = false;
                }
            }


        }

        public static Tuple<int, int> ParseTuple(string line)
        {
            string[] lineGroup = line.Split(",");
            int firstNumber = Int32.Parse(lineGroup[0]);
            int secondNumber = Int32.Parse(lineGroup[1]);
            return new Tuple<int, int>(firstNumber, secondNumber);
        }
    }
}