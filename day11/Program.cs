using System;
using System.IO;

namespace aoc_2022_day11
{
    internal class Program
    {
        static void Main()
        {
            string input = System.IO.File.ReadAllText("input.txt");

            Dictionary<int, List<Item>> standing = new Dictionary<int, List<Item>> { };
            Dictionary<int, Monkey> metadataPerMonkey = new Dictionary<int, Monkey> { };

            // Parsing the input
            string[] monkeyGroup = input.Split("Monkey");
            // Item currentItem = new Item();
            foreach (string monkey in monkeyGroup)
            {
                if (monkey.Length == 0)
                {
                    continue;
                }
                string[] lineGroup = monkey.Replace("\r", "").Split("\n");
                // MonkeyNumber
                int monkeyNum = Int32.Parse(lineGroup[0].Replace(":", ""));
                // WorryLevels
                List<int> tempItemList = new List<int>();
                foreach (string word in lineGroup[1].Split(" "))
                {
                    if (Int32.TryParse(word.Replace(",", ""), out int item))
                    {
                        tempItemList.Add(item);
                    };
                }
                // Operation
                string[] splittedline = lineGroup[2].Split(" ");
                Tuple<string, string> operation = new Tuple<string, string>(splittedline[6], splittedline[7]);

                // Test
                int test = Int32.Parse(lineGroup[3].Split(" ")[5]);

                // trueRecipient
                int trueRecipient = Int32.Parse(lineGroup[4].Split(" ")[9]);

                // falseRecipient
                int falseRecipient = Int32.Parse(lineGroup[5].Split(" ")[9]);

                // Store the information in standing
                List<Item> itemsForMonkey = new List<Item>();
                foreach (Int64 worryLevel in tempItemList)
                {
                    Item currentItem = new Item(worryLevel);
                    itemsForMonkey.Add(currentItem);

                }
                standing[monkeyNum] = itemsForMonkey;

                // Store information metadata
                metadataPerMonkey[monkeyNum] = new Monkey(trueRecipient, falseRecipient, operation, test);
            }

            // Now we have the current standing, let's play
            int roundsToPlay = 10000;
            int multipliedAllDividers = 1;
            foreach (int key in metadataPerMonkey.Keys)
            {
                multipliedAllDividers *= metadataPerMonkey[key].testValue;
            }
            while (roundsToPlay > 0)
            {
                foreach (int monkey in standing.Keys)
                {
                    Console.WriteLine($"Working with monkey {monkey}");
                    foreach (Item item in standing[monkey])
                    {
                        metadataPerMonkey[monkey].itemsAssessed += 1;
                        // Int64 newWorryLevel = Convert.ToInt64(Math.Floor(CalculateNewWorryLevel(item.worryLevel, metadataPerMonkey[monkey].operation) / 3.0)); // for q1
                        Int64 newWorryLevel = CalculateNewWorryLevel(item.worryLevel, metadataPerMonkey[monkey].operation) % multipliedAllDividers; // for q2
                        item.worryLevel = newWorryLevel;

                        if (item.worryLevel % metadataPerMonkey[monkey].testValue == 0)
                        {
                            standing[metadataPerMonkey[monkey].trueRecipient].Add(item);
                        }
                        else
                        {
                            standing[metadataPerMonkey[monkey].falseRecipient].Add(item);
                        }
                    }
                    // Now the list should be empty
                    standing[monkey] = new List<Item>();
                }
                roundsToPlay -= 1;
            }

            // Check which monkeys have the most
            List<Int64> resultList = new List<Int64>();
            foreach (int key in metadataPerMonkey.Keys)
            {
                resultList.Add(metadataPerMonkey[key].itemsAssessed);
            }
            resultList.Sort();
            Int64 res = resultList[resultList.Count - 1] * resultList[resultList.Count - 2];
            Console.WriteLine($"Answer is {res}");
        }

        public class Item
        {
            public Int64 worryLevel;

            public Item(Int64 worry)
            {
                worryLevel = worry;
            }
        }

        public class Monkey
        {
            public int trueRecipient;
            public int falseRecipient;
            public Tuple<string, string> operation;
            public int testValue;
            public int itemsAssessed = 0;
            public Monkey(int tr, int fr, Tuple<string, string> op, int test)
            {
                trueRecipient = tr;
                falseRecipient = fr;
                operation = op;
                testValue = test;
            }
        }

        public static Int64 CalculateNewWorryLevel(Int64 currentLevel, Tuple<string, string> operation)
        {
            bool isNumber = false;
            if (Int64.TryParse(operation.Item2, out Int64 number))
            {
                isNumber = true;
            }
            switch (operation.Item1)
            {
                case "*":
                    if (isNumber)
                    {
                        return currentLevel * number;
                    }
                    else if (operation.Item2 == "old")
                    {
                        return currentLevel * currentLevel;
                    }
                    else
                    {
                        Console.WriteLine("Invalid operation");
                        throw new Exception();
                    }
                case "+":
                    if (isNumber)
                    {
                        return currentLevel + number;
                    }
                    else
                    {
                        Console.WriteLine("Invalid operation");
                        throw new Exception();
                    }
                default:
                    Console.WriteLine("Invalid operation");
                    throw new Exception();

            }
        }
    }
}