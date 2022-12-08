using System;
using System.Collections.Generic;

namespace aoc_2021_day1
{
    internal class Program
    {
        static void Main()
        {
            Console.WriteLine("Starting up...");
            // Getting the input 
            string[] input = utils.ReadInput();

            // Processing the raw input
            int temp = 0;
            int winning_q1 = 0;
            int[] winning_q2 = new int[] { 0, 0, 0 };
            int total_lines = input.Count() - 1;
            for (int i = 0; i < input.Count(); i++)
            {
                try
                {
                    // if blank line, then the parsing will throw an exception
                    int extra_calories = Int32.Parse(input[i]);
                    temp += extra_calories;

                    // handling if it is the last line in the input file
                    if (i == total_lines)
                    {
                        winning_q2 = utils.CalculateTopThree(winning_q2, temp);
                    }
                }
                catch
                {
                    if (temp > winning_q1)
                    {
                        // the current elv is staged to win
                        winning_q1 = temp;
                    }
                    // second question
                    winning_q2 = utils.CalculateTopThree(winning_q2, temp);

                    // just nullying out the counter per elv
                    temp = 0;
                }
            }
            Console.WriteLine($"And the elv with the most calories has: {winning_q1}");
            // Processing the answer for question 2
            int answer_q2 = winning_q2.Sum();
            Console.WriteLine($"And the top 3 elves have {answer_q2} calories.");

        }
    }
}