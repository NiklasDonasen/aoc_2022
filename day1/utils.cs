using System;

namespace aoc_2021_day1
{
    public class utils
    {
        public static string[] ReadInput()
        {
            string[] lines = System.IO.File.ReadAllLines("input.txt");
            return lines;
        }

        public static int[] CalculateTopThree(int[] current_top_3, int challenger)
        {
            // sort in ascending order
            Array.Sort(current_top_3);
            for (int i = 0; i < current_top_3.Count(); i++)
            {
                if (challenger > current_top_3[i])
                {
                    Console.WriteLine($"Dropping {current_top_3[i]} from winning list.");
                    Console.WriteLine($"Adding {challenger} to winning_list.");
                    current_top_3[i] = challenger;
                    break;
                }
            }

            return current_top_3;
        }
    }
}