using System;
using System.IO;

namespace aoc_2022_day4
{
    internal class Program
    {
        static void Main()
        {
            // getting the input
            StreamReader sr = new StreamReader("input.txt");

            // store result
            int res_q1 = 0;
            int res_q2 = 0;

            string? line;
            while ((line = sr.ReadLine()) != null)
            {
                string firstSection = line.Split(",")[0];
                string secondSection = line.Split(",")[1];

                // check which section has a lower start, if at all
                int firstStart = Int32.Parse(firstSection.Split("-")[0]);
                int secondStart = Int32.Parse(secondSection.Split("-")[0]);

                int firstEnd = Int32.Parse(firstSection.Split("-")[1]);
                int secondEnd = Int32.Parse(secondSection.Split("-")[1]);

                // answer q1
                // second complete in first
                if (secondStart >= firstStart && secondEnd <= firstEnd)
                {
                    res_q1 += 1;
                }
                else if (firstStart >= secondStart && firstEnd <= secondEnd)
                {
                    res_q1 += 1;
                }
                // else no points

                // answer q2
                // if second within first
                if (secondStart >= firstStart && secondStart <= firstEnd)
                {
                    res_q2 += 1;
                }
                // if first within second
                else if (firstStart >= secondStart && firstStart <= secondEnd)
                {
                    res_q2 += 1;
                }

            }

            Console.WriteLine($"answer for q1 is {res_q1}");
            Console.WriteLine($"answer for q2 is {res_q2}");

        }
    }
}