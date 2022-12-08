using System;

namespace aoc_2022_day5
{
    public class utils
    {
        public static int[] GetIndexes(StreamReader sr)
        {
            int[] indices = Enumerable.Range(1, 9).ToArray();
            bool getSetup = true;

            string? line;
            while ((line = sr.ReadLine()) != null)
            {
                // Get the starting configuration and only do it once
                if (line.Contains("1") && getSetup)
                {
                    // keep track of which position you have already filled up
                    int counter = 0;
                    for (int i = 0; i < line.Length; i++)
                    {
                        if (Int32.TryParse(line[i].ToString(), out int _))
                        {
                            indices[counter] = i;
                            counter += 1;
                        }
                    }

                    getSetup = false;
                }

                if (line.Length == 0)
                {
                    // Stop the first loop
                    return indices;
                }
            }
            Console.WriteLine("Did not manage to finish");
            throw new Exception("Error");
        }

        public static Dictionary<string, List<char>> UseIndices(StreamReader sr, int[] indices)
        {
            // initialising the start setup
            Dictionary<string, List<char>> start = new();

            for (int i = 1; i < 10; i++)
            {
                var temp = new List<char>();
                start[$"{i}"] = temp;
            }

            string? line;
            while ((line = sr.ReadLine()) != null)
            {
                if (line.Contains("1"))
                {
                    // Stop the second loop
                    Console.WriteLine("Parsing finished");
                    return start;
                }

                // Get the starting config
                for (int i = 0; i < indices.Length; i++)
                {
                    char box = line[indices[i]];
                    if (!Char.IsWhiteSpace(box))
                    {
                        start[$"{i + 1}"].Add(box);
                    }
                }
            }
            Console.WriteLine("Did not manage to finish second loop");
            throw new Exception("Error");
        }

        public static Dictionary<string, List<char>> solveQuestionOne(
            Dictionary<string, List<char>> setup,
            int numberOfBoxes,
            string fromCol,
            string toCol
            )
        {
            var temp = new List<char>();

            for (int i = 0; i < numberOfBoxes; i++)
            {
                try
                {
                    char tempValue = setup[fromCol][0];
                    setup[fromCol].RemoveAt(0);
                    setup[toCol].Insert(0, tempValue);
                }
                catch
                {
                    Console.WriteLine("No more values to get from col");
                }

            }

            return setup;
        }

        public static Dictionary<string, List<char>> solveQuestionTwo(
            Dictionary<string, List<char>> setup,
            int numberOfBoxes,
            string fromCol,
            string toCol
            )
        {
            var temp = new List<char>();

            for (int i = 0; i < numberOfBoxes; i++)
            {
                try
                {
                    char tempValue = setup[fromCol][0];
                    setup[fromCol].RemoveAt(0);
                    temp.Add(tempValue);
                }
                catch
                {
                    Console.WriteLine("No more values to get from col");
                }

            }
            var joinedList = temp.Concat(setup[toCol]).ToList();

            setup[toCol] = joinedList;

            return setup;
        }

    }
}