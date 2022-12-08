using System;
using System.IO;

namespace aoc_2022_day2
{
    internal class Programm
    {
        static void Main()
        {
            // getting the input
            StreamReader sr = new StreamReader("input.txt");

            int points_q1 = 0;

            int points_q2 = 0;

            string? line;
            while ((line = sr.ReadLine()) != null)
            {
                string opponent = line.Split(" ")[0];
                string me = line.Split(" ")[1];

                // Adding my own points
                points_q1 += utils.MyOwnPoints(me);

                // Adding points for competing with my opponent
                points_q1 += utils.Competition(me, opponent);

                // Deciding your own command
                string me_q2 = utils.FindMyCommand(me, opponent);

                // Adding my own points
                points_q2 += utils.MyOwnPoints(me_q2);

                // Adding points for competing with my opponent
                points_q2 += utils.Competition_q2(me_q2, opponent);


            }

            Console.WriteLine($"The correct answer for q1 is {points_q1}");
            Console.WriteLine($"The correct answer for q2 is {points_q2}");

        }
    }
}