using System;
using System.IO;

namespace aoc_2022_day9
{
    internal class Program
    {
        static void Main()
        {
            StreamReader sr = new StreamReader("input.txt");

            HashSet<Tuple<int, int>> coveredPositions = new HashSet<Tuple<int, int>>();
            // Initialise position
            Player head = new Player(0, 0);
            Player tail = new Player(0, 0);

            // Initialise position for part 2
            Dictionary<int, Player> Positions = new Dictionary<int, Player>();
            for (int i = 0; i < 10; i++)
            {
                Positions[i] = new Player(0, 0);
            }

            HashSet<Tuple<int, int>> res_q2 = new HashSet<Tuple<int, int>> { };
            res_q2.Add(new Tuple<int, int>(0, 0));

            // Tail is at starting position
            coveredPositions.Add(Tuple.Create(0, 0));

            string? line;
            while ((line = sr.ReadLine()) != null)
            {
                // Both players start at 0,0
                string[] lineGroup = line.Split(" ");
                string direction = lineGroup[0];
                int numMoves = Int32.Parse(lineGroup[1]);

                while (numMoves > 0)
                {
                    Player prevPos = new Player(head.RowPosition, head.ColPosition);
                    head = ImplementCommand(head, direction);

                    // Question 1: head has moved, only move tail if necessary
                    if (!CheckCloseness(head, tail))
                    {

                        tail = MoveTail(head, tail);

                        coveredPositions.Add(Tuple.Create(tail.RowPosition, tail.ColPosition));
                    }

                    // Question 2
                    Positions[0] = ImplementCommand(Positions[0], direction);

                    for (int player = 1; player < Positions.Keys.Count; player++)
                    {
                        if (!CheckCloseness(Positions[player - 1], Positions[player]))
                        {
                            Positions[player] = MoveTail(Positions[player - 1], Positions[player]);
                            if (player == 9)
                            {
                                res_q2.Add(new Tuple<int, int>(Positions[player].RowPosition, Positions[player].ColPosition));
                            }
                        }
                    }

                    numMoves -= 1;
                }

            }

            Console.WriteLine($"Answer for q1 {coveredPositions.Count()}");

            Console.WriteLine($"Answer for q2 {res_q2.Count}");

        }

        public class Player
        {
            public int RowPosition = 0;
            public int ColPosition = 0;

            public Player(int row, int col)
            {
                RowPosition = row;
                ColPosition = col;
            }
        }

        public static bool CheckCloseness(Player head, Player tail)
        {
            List<int> allowedRows = new List<int> { tail.RowPosition - 1, tail.RowPosition, tail.RowPosition + 1 };
            List<int> allowedCols = new List<int> { tail.ColPosition - 1, tail.ColPosition, tail.ColPosition + 1 };

            bool row = allowedRows.IndexOf(head.RowPosition) != -1;
            bool col = allowedCols.IndexOf(head.ColPosition) != -1;

            if (row && col)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static Player ImplementCommand(Player player, string direction)
        {
            switch (direction)
            {
                case "R":
                    player.ColPosition += 1;
                    break;
                case "L":
                    player.ColPosition = player.ColPosition -= 1;
                    break;
                case "U":
                    player.RowPosition += 1;
                    break;
                case "D":
                    player.RowPosition = player.RowPosition -= 1;
                    break;
                default:
                    Console.WriteLine("Invalid direction command");
                    throw new Exception();
            }

            return player;
        }

        public static Player MoveTail(Player head, Player tail)
        {
            if (head.ColPosition == tail.ColPosition)
            {
                // move inside same row
                if (head.RowPosition > tail.RowPosition)
                {
                    tail.RowPosition += 1;
                }
                else if (head.RowPosition < tail.RowPosition)
                {
                    tail.RowPosition -= 1;
                }
            }
            else if (head.RowPosition == tail.RowPosition)
            {
                // move inside same column
                if (head.ColPosition > tail.ColPosition)
                {
                    tail.ColPosition += 1;
                }
                else if (head.ColPosition < tail.ColPosition)
                {
                    tail.ColPosition -= 1;
                }
            }
            else
            {
                // move diagonally
                if (head.RowPosition > tail.RowPosition)
                {
                    tail.RowPosition += 1;
                }
                else
                {
                    tail.RowPosition -= 1;
                }
                if (head.ColPosition > tail.ColPosition)
                {
                    tail.ColPosition += 1;
                }
                else
                {
                    tail.ColPosition -= 1;
                }
            }

            return tail;
        }
    }
}