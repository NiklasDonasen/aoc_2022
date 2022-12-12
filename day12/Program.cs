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
            Position cursor = new Position(0, 0, 0);
            Position destination = new Position(0, 0, 0);

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
                        // All places you have been before are 100, do not want you to backtrack
                        grid[row, col] = 100;
                    }
                    else if (currentValue == char.Parse("E"))
                    {
                        destination.rowPosition = row;
                        destination.colPosition = col;
                        // Destination will also be a "unique" number
                        grid[row, col] = 500;
                    }
                    else
                    {
                        grid[row, col] = char.ToUpper(currentValue) - 64;
                    }
                }
            }

            // Find possible moves for start
            List<Position> possibleMoves = new List<Position>();
            possibleMoves = FindPossibleMoves(cursor, destination, grid);

            // Initialise a temp cursor
            Position tempCursor = new Position(cursor.rowPosition, cursor.colPosition, 0);

            foreach (Position move in possibleMoves)
            {
                // Goal is to get to a field which has a direct neighbour (U, D, R, L) which is 500
                int counter = 0;
                tempCursor.rowPosition = move.rowPosition;
                tempCursor.colPosition = move.colPosition;
                tempCursor.prevValue = move.prevValue;
                counter += 1;

                while (tempCursor.prevValue != 500)
                {
                    possibleMoves = FindPossibleMoves(tempCursor, destination, grid);
                    // Find wanted direction
                    // Position directionCursorHasToMove = destination - cursor;
                    // Find best possible move
                    Position nextMove = FindBestNextMove(possibleMoves, tempCursor, destination);

                    // then do it
                    tempCursor.rowPosition = nextMove.rowPosition;
                    tempCursor.colPosition = nextMove.colPosition;
                    tempCursor.prevValue = nextMove.prevValue;
                    counter += 1;
                }

                Console.WriteLine($"This round took {counter} iterations");


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
            public int prevValue = 0;

            public Position(int row, int col, int prev)
            {
                rowPosition = row;
                colPosition = col;
                prevValue = prev;
            }
            public static Position operator -(Position x, Position y)
            {
                return new Position(x.rowPosition - y.rowPosition, x.colPosition - y.colPosition, 0);
            }

        }

        public static Position FindBestNextMove(List<Position> possibleMoves, Position cursor, Position destination)
        {
            foreach (Position move in possibleMoves)
            {
                if (cursor.rowPosition < move.rowPosition && move.rowPosition < destination.rowPosition)
                {
                    return move;
                }
                else if (cursor.colPosition < move.colPosition && move.colPosition < destination.colPosition)
                {
                    return move;
                }
            }
            // if moved directly, then take the first one
            return possibleMoves[0];
        }

        public static List<Position> FindPossibleMoves(Position cursor, Position destination, int[,] grid)
        {
            List<Position> allowedPositions = new List<Position>();
            List<Position> lastMove = new List<Position>();
            List<int> allowedGridValues = new List<int>();
            // if cursor has not moved yet
            if (cursor.prevValue == 0)
            {
                allowedGridValues = Enumerable.Range(1, 26).ToList();
            }
            else
            {
                allowedGridValues = new List<int> { cursor.prevValue - 1, cursor.prevValue, cursor.prevValue + 1 };
            }

            try
            {
                if (grid[cursor.rowPosition + 1, cursor.colPosition] == 500)
                {
                    Console.WriteLine("Winner winner chicken dinner");
                    lastMove.Add(new Position(
                        cursor.rowPosition + 1,
                        cursor.colPosition,
                        grid[cursor.rowPosition + 1, cursor.colPosition]
                    ));
                    return lastMove;
                }
                if (allowedGridValues.Contains(grid[cursor.rowPosition + 1, cursor.colPosition]))
                {
                    allowedPositions.Add(new Position(
                        cursor.rowPosition + 1, cursor.colPosition,
                        grid[cursor.rowPosition + 1, cursor.colPosition]
                    ));
                }
            }
            catch
            {
                // index out of bound because you are the edge somewhere
            }
            try
            {
                if (grid[cursor.rowPosition - 1, cursor.colPosition] == 500)
                {
                    Console.WriteLine("Winner winner chicken dinner");
                    lastMove.Add(new Position(
                        cursor.rowPosition - 1,
                        cursor.colPosition,
                        grid[cursor.rowPosition - 1, cursor.colPosition]
                    ));
                    return lastMove;
                }
                if (allowedGridValues.Contains(grid[cursor.rowPosition - 1, cursor.colPosition]))
                {
                    allowedPositions.Add(new Position(
                        cursor.rowPosition - 1, cursor.colPosition,
                        grid[cursor.rowPosition - 1, cursor.colPosition]
                    ));
                }
            }
            catch
            {
                // index out of bound because you are the edge somewhere
            }
            try
            {
                if (grid[cursor.rowPosition, cursor.colPosition + 1] == 500)
                {
                    Console.WriteLine("Winner winner chicken dinner");
                    lastMove.Add(new Position(
                        cursor.rowPosition,
                        cursor.colPosition + 1,
                        grid[cursor.rowPosition, cursor.colPosition + 1]
                    ));
                    return lastMove;
                }
                if (allowedGridValues.Contains(grid[cursor.rowPosition, cursor.colPosition + 1]))
                {
                    allowedPositions.Add(new Position(
                        cursor.rowPosition, cursor.colPosition + 1,
                        grid[cursor.rowPosition, cursor.colPosition + 1]
                    ));
                }
            }
            catch
            {
                // index out of bound because you are the edge somewhere
            }
            try
            {
                if (grid[cursor.rowPosition, cursor.colPosition - 1] == 500)
                {
                    Console.WriteLine("Winner winner chicken dinner");
                    lastMove.Add(new Position(
                        cursor.rowPosition,
                        cursor.colPosition - 1,
                        grid[cursor.rowPosition, cursor.colPosition - 1]
                    ));
                    return lastMove;
                }
                if (allowedGridValues.Contains(grid[cursor.rowPosition, cursor.colPosition - 1]))
                {
                    allowedPositions.Add(new Position(
                        cursor.rowPosition, cursor.colPosition - 1,
                        grid[cursor.rowPosition, cursor.colPosition - 1]
                    ));
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