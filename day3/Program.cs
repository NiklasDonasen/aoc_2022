using System;
using System.IO;

namespace aoc_2022_day3
{
    internal class Program
    {
        static void Main()
        {
            // Solving question 1
            firstQuestion();

            // Solving question 2
            secondQuestion();

        }

        static void firstQuestion()
        {
            // getting the input
            StreamReader sr = new StreamReader("input.txt");

            int points_q1 = 0;
            string? line;
            while ((line = sr.ReadLine()) != null)
            {
                string firstHalf = line.Substring(0, line.Length / 2);
                string secondHalf = line.Substring(line.Length / 2);

                // Find common letter
                char commonLetter = utils.FindCommonLetter(firstHalf, secondHalf);

                // Calculate points
                int points = utils.CalculatePoints(commonLetter);
                points_q1 += points;
            }

            Console.WriteLine($"The answer for q1: {points_q1}");
        }

        static void secondQuestion()
        {
            string[] input = File.ReadAllLines("input.txt");

            int points_q2 = 0;
            for (int i = 0; i < input.Length; i += 3)
            {
                string firstElv = input[i];
                string secondElv = input[i + 1];
                string thirdElv = input[i + 2];

                // Find common letter
                char commonLetter = utils.FindCommonLetterForGroupOfThree(firstElv, secondElv, thirdElv);

                // Calculate points
                int points = utils.CalculatePoints(commonLetter);
                points_q2 += points;
            }

            Console.WriteLine($"The answer for q2: {points_q2}");
        }
    }
}