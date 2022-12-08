using System;
using System.IO;
using System.Collections.Generic;

namespace aoc_2022_day5
{
    internal class Program
    {
        static void Main()
        {
            StreamReader sr = new StreamReader("input.txt");

            string res = "";

            // First loop for indexes
            int[] indices = utils.GetIndexes(sr);

            // initialising the start setup
            Dictionary<string, List<char>> start = new();

            for (int i = 1; i < 10; i++)
            {
                var temp = new List<char>();
                start[$"{i}"] = temp;
            }

            StreamReader sr_2 = new StreamReader("input.txt");

            // Second loop to populate the start setup
            var setup = utils.UseIndices(sr_2, indices);

            // Use the setup and commandoes
            string? line;
            while ((line = sr.ReadLine()) != null)
            {

                if (line.Contains("move"))
                {
                    var instructions = line.Split(" ");
                    int numberOfBoxes = Int32.Parse(instructions[1]);
                    string fromCol = instructions[3];
                    string toCol = instructions[5];
                    var temp = new List<char>();

                    // Question 1
                    // setup = utils.solveQuestionOne(
                    //     setup, numberOfBoxes, fromCol, toCol
                    // );

                    // // Question 2
                    setup = utils.solveQuestionTwo(
                        setup, numberOfBoxes, fromCol, toCol
                    );

                }

            }

            for (int i = 0; i < setup.Count; i++)
            {
                res += setup[$"{i + 1}"][0];
            }
            Console.WriteLine($"Answer is {res}");

        }
    }
}