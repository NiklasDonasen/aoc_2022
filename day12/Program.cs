using System;
using System.IO;

namespace aoc_2022_day12
{
    internal class Program
    {
        static void Main()
        {
            string[] lines = System.IO.File.ReadAllLines("input.txt");
            int res_q1 = 0;

            // variables to be filled
            Position cursor = new Position(0, 0);
            Position destination = new Position(0, 0);

            // Initialise grid
            int maxCols = lines[0].Length;
            int maxRows = lines.Count();
            // 0,0 is in the top-left-corner
            int[,] grid = new int[maxRows, maxCols];

            // Populate grid
            for (int row = 0; row < maxRows; row++)
            {
                for (int col = 0; col < maxCols; col++)
                {
                    char currentValue = lines[row][col];
                    if (currentValue == char.Parse("S"))
                    {
                        cursor.rowPosition = row;
                        cursor.colPosition = col;
                        grid[row, col] = char.ToUpper(char.Parse("a")) - 64;
                    }
                    else if (currentValue == char.Parse("E"))
                    {
                        destination.rowPosition = row;
                        destination.colPosition = col;
                        grid[row, col] = char.ToUpper(char.Parse("z")) - 64;
                    }
                    else
                    {
                        grid[row, col] = char.ToUpper(currentValue) - 64;
                    }
                }
            }

            // Find possible moves for start
            List<Position> possibleMoves = FindPossibleMoves(cursor, grid);

            // Du vil alltid gå for samme eller neste bokstav!!!!
            // Samle alle muligheter??

            // Initialise a temp cursor
            Position tempCursor = new Position(cursor.rowPosition, cursor.colPosition);

            foreach (Position move in possibleMoves)
            {
                // variable to store positions which the cursor already has visited
                HashSet<Tuple<int, int>> visited = new HashSet<Tuple<int, int>>();
                visited.Add(new Tuple<int, int>(cursor.rowPosition, cursor.colPosition));

                // Goal is to get to a field which has a direct neighbour (U, D, R, L) which is 500
                tempCursor = move;
                visited.Add(new Tuple<int, int>(tempCursor.rowPosition, tempCursor.colPosition));
                bool lookingForSolution = true;

                while (lookingForSolution)
                {
                    possibleMoves = FindPossibleMoves(tempCursor, grid);
                    if (possibleMoves.Count == 0)
                    {
                        // Stopping up
                        lookingForSolution = false;
                        break;
                    }

                    try
                    {
                        Position BestMove = FindBestMove(possibleMoves, destination, tempCursor, visited);
                        if (BestMove.rowPosition == destination.rowPosition && BestMove.colPosition == destination.colPosition)
                        {
                            lookingForSolution = false;
                        }
                        else
                        {
                            tempCursor = BestMove;
                            visited.Add(new Tuple<int, int>(tempCursor.rowPosition, tempCursor.colPosition));
                        }
                    }
                    catch
                    {
                        // Path stopped without finding destination
                        lookingForSolution = false;

                    }
                }

                Console.WriteLine($"This round took {visited.Count} iterations");


            }




            // We have to assign the previous val to cursor.prev

            // places which are visited more than once are marked as unaccessible and then we iterate several times over the map
            // you could also keep track of the last position where you had a choice and then go there


            Console.WriteLine($"Answer for q1 is {res_q1}");
        }

        public class Position
        {
            public int rowPosition;
            public int colPosition;
            public Position(int row, int col)
            {
                rowPosition = row;
                colPosition = col;
            }
            public static Position operator -(Position x, Position y)
            {
                return new Position(x.rowPosition - y.rowPosition, x.colPosition - y.colPosition);
            }

        }

        public static Position FindBestMove(List<Position> possibleMoves, Position destination, Position tempCursor, HashSet<Tuple<int, int>> visited)
        {
            // You want to sort the moves to get the best ones first
            List<Position> sortedMoves = new List<Position>();

            foreach (Position checkMove in possibleMoves)
            {
                // Check that none of the possibleMoves gives you the destination
                if (checkMove.rowPosition == destination.rowPosition && checkMove.colPosition == destination.colPosition)
                {
                    sortedMoves.Add(checkMove);
                    break;
                }

                // If you have already been there, it is not a valid option
                if (visited.Contains(new Tuple<int, int>(checkMove.rowPosition, checkMove.colPosition)))
                {
                    continue;
                }

                // If it brings you closer to destination column-wise: go with it
                if (Math.Abs(destination.colPosition - checkMove.colPosition) < Math.Abs(destination.colPosition - tempCursor.colPosition))
                {
                    sortedMoves.Insert(0, checkMove);
                }
                // If it brings you closer to destination row-wise: go with it
                else if (Math.Abs(destination.rowPosition - checkMove.rowPosition) < Math.Abs(destination.rowPosition - tempCursor.rowPosition))
                {
                    sortedMoves.Insert(0, checkMove);
                }
                else
                {
                    // Does not bring you closer, add at the end
                    sortedMoves.Add(checkMove);
                }
            }

            return sortedMoves[0];
        }

        public static List<Position> FindPossibleMoves(Position cursor, int[,] grid)
        {
            List<Position> allowedPositions = new List<Position>();

            int cursorValue = grid[cursor.rowPosition, cursor.colPosition];

            // check value up
            try
            {
                int valueUp = grid[cursor.rowPosition + 1, cursor.colPosition];
                if (Math.Abs(valueUp - cursorValue) == 1 || Math.Abs(valueUp - cursorValue) == 0)
                {
                    allowedPositions.Add(new Position(cursor.rowPosition + 1, cursor.colPosition));
                }
            }
            catch
            {
                // index out of bound because you are the edge somewhere
            }

            // check value down
            try
            {
                int valueDown = grid[cursor.rowPosition - 1, cursor.colPosition];
                if (Math.Abs(valueDown - cursorValue) == 1 || Math.Abs(valueDown - cursorValue) == 0)
                {
                    allowedPositions.Add(new Position(cursor.rowPosition - 1, cursor.colPosition));
                }
            }
            catch
            {
                // index out of bound because you are the edge somewhere
            }

            // check value left
            try
            {
                int valueLeft = grid[cursor.rowPosition, cursor.colPosition - 1];
                if (Math.Abs(valueLeft - cursorValue) == 1 || Math.Abs(valueLeft - cursorValue) == 0)
                {
                    allowedPositions.Add(new Position(cursor.rowPosition, cursor.colPosition - 1));
                }
            }
            catch
            {
                // index out of bound because you are the edge somewhere
            }

            // check value right
            try
            {
                int valueRight = grid[cursor.rowPosition, cursor.colPosition + 1];
                if (Math.Abs(valueRight - cursorValue) == 1 || Math.Abs(valueRight - cursorValue) == 0)
                {
                    allowedPositions.Add(new Position(cursor.rowPosition, cursor.colPosition + 1));
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