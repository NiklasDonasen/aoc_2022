using System;
using System.IO;

namespace aoc_2022_day12
{
    internal class Program
    {
        static void Main()
        {
            string[] lines = System.IO.File.ReadAllLines("input.txt");

            // Initialise grid
            int maxCols = lines[0].Length;
            int maxRows = lines.Count();
            Tuple<int, int> destinationTruth = new Tuple<int, int>(0, 0);
            int res_q2 = 423;
            // 0,0 is in the top-left-corner
            int[,] grid = new int[maxRows, maxCols];

            // For question 2
            List<Tuple<int, int>> possibleStartPoints = new List<Tuple<int, int>>();

            // Populate grid
            for (int row = 0; row < maxRows; row++)
            {
                for (int col = 0; col < maxCols; col++)
                {
                    char currentValue = lines[row][col];
                    if (currentValue == char.Parse("S"))
                    {
                        // Tuple<int, int> cursor = new Tuple<int, int>(row, col);
                        grid[row, col] = char.ToUpper(char.Parse("a")) - 64;
                        possibleStartPoints.Add(new Tuple<int, int>(row, col));
                    }
                    else if (currentValue == char.Parse("E"))
                    {
                        destinationTruth = new Tuple<int, int>(row, col);
                        grid[row, col] = char.ToUpper(char.Parse("z")) - 64;
                    }
                    else if (currentValue == char.Parse("a"))
                    {
                        grid[row, col] = char.ToUpper(currentValue) - 64;
                        possibleStartPoints.Add(new Tuple<int, int>(row, col));
                    }
                    else
                    {
                        grid[row, col] = char.ToUpper(currentValue) - 64;
                    }
                }
            }


            foreach (Tuple<int, int> cursor in possibleStartPoints)
            {
                Tuple<int, int> destination = new Tuple<int, int>(destinationTruth.Item1, destinationTruth.Item2);
                // Walk through the possibility room
                HashSet<Tuple<int, int>> visited = new HashSet<Tuple<int, int>>();

                // Storing information how you got into each node
                Dictionary<Tuple<int, int>, Tuple<int, int>> previous = new Dictionary<Tuple<int, int>, Tuple<int, int>>();

                Queue<Tuple<int, int>> queue = new Queue<Tuple<int, int>>();
                queue.Enqueue(cursor);

                Tuple<int, int> prevPos = new Tuple<int, int>(5000, 5000);
                while (queue.Count > 0)
                {
                    Tuple<int, int> vertex = queue.Dequeue();

                    if (visited.Contains(vertex))
                    {
                        continue;
                    }
                    visited.Add(vertex);


                    List<Tuple<int, int>> possibleMoves = FindPossibleMoves(vertex, prevPos, grid);

                    foreach (Tuple<int, int> neighbor in possibleMoves)
                    {
                        if (previous.ContainsKey(neighbor))
                        {
                            continue;
                        }
                        previous[neighbor] = vertex;
                        queue.Enqueue(neighbor);
                    }
                    prevPos = new Tuple<int, int>(vertex.Item1, vertex.Item2);

                }

                List<Tuple<int, int>> path = new List<Tuple<int, int>>();
                while (!cursor.Equals(destination))
                {
                    if (path.Count > 423)
                    {
                        break;
                    }
                    path.Add(destination);
                    try
                    {
                        destination = previous[destination];
                    }
                    catch
                    {
                        continue;
                    }
                };

                path.Reverse();

                Console.WriteLine($"Start {cursor.Item1}, {cursor.Item2} took {path.Count}x steps to destination");
                if (path.Count < res_q2)
                {
                    res_q2 = path.Count;
                }

            }
            Console.WriteLine($"Answer for q2 is {res_q2}");
        }

        public static List<Tuple<int, int>> FindPossibleMoves(Tuple<int, int> cursor, Tuple<int, int> prevPos, int[,] grid)
        {
            List<Tuple<int, int>> allowedPositions = new List<Tuple<int, int>>();

            int cursorValue = grid[cursor.Item1, cursor.Item2];

            // check value up
            try
            {
                if (cursor.Item1 + 1 != prevPos.Item1 || cursor.Item2 != prevPos.Item2)
                {
                    int valueUp = grid[cursor.Item1 + 1, cursor.Item2];
                    if (valueUp - cursorValue <= 1)
                    {
                        allowedPositions.Add(new Tuple<int, int>(cursor.Item1 + 1, cursor.Item2));
                    }
                }
            }
            catch
            {
                // index out of bound because you are the edge somewhere
            }

            // check value down
            try
            {
                if (cursor.Item1 - 1 != prevPos.Item1 || cursor.Item2 != prevPos.Item2)
                {
                    int valueDown = grid[cursor.Item1 - 1, cursor.Item2];
                    if (valueDown - cursorValue <= 1)
                    {
                        allowedPositions.Add(new Tuple<int, int>(cursor.Item1 - 1, cursor.Item2));
                    }
                }
            }
            catch
            {
                // index out of bound because you are the edge somewhere
            }

            // check value left
            try
            {
                if (cursor.Item1 != prevPos.Item1 || cursor.Item2 - 1 != prevPos.Item2)
                {
                    int valueLeft = grid[cursor.Item1, cursor.Item2 - 1];
                    if (valueLeft - cursorValue <= 1)
                    {
                        allowedPositions.Add(new Tuple<int, int>(cursor.Item1, cursor.Item2 - 1));
                    }
                }
            }
            catch
            {
                // index out of bound because you are the edge somewhere
            }

            // check value right
            try
            {
                if (cursor.Item1 == prevPos.Item1 || cursor.Item2 + 1 != prevPos.Item2)
                {
                    int valueRight = grid[cursor.Item1, cursor.Item2 + 1];
                    if (valueRight - cursorValue <= 1)
                    {
                        allowedPositions.Add(new Tuple<int, int>(cursor.Item1, cursor.Item2 + 1));
                    }
                }
            }
            catch
            {
                // index out of bound because you are the edge somewhere
            }

            return allowedPositions;
        }
    }
}