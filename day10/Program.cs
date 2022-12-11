using System;
using System.IO;

namespace aoc_2022_day10
{
    internal class Program
    {
        static void Main()
        {
            StreamReader sr = new StreamReader("input.txt");

            int res_q1 = 0;

            int registerValue = 1;
            int currentCycle = 0;
            int newCycles = 0;
            int incrementRegister = 0;

            // Question 2
            int crtPosition = 0;
            List<int> sprite = new List<int> { registerValue, registerValue + 1, registerValue + 2 };
            List<string> picture = new List<string>();

            List<int> checkpoints = new List<int> { 20, 60, 100, 140, 180, 220 };

            // Generator to return signal
            IEnumerable<Tuple<int, int>> YieldSignalStrength(int registerValue, int currentCycle, int newCycles, int incrementRegister)
            {
                bool changeDuringGenerator = false;
                if (currentCycle % 2 != 0 && newCycles == 2)
                {
                    // if you come with an uneven cycle number and are executing an `addx`-command, then you increment on the second loop
                    changeDuringGenerator = true;
                }
                for (int i = 0; i < newCycles; i++)
                {
                    currentCycle += 1;
                    if (changeDuringGenerator && i == 1)
                    {
                        registerValue += incrementRegister;
                        yield return new Tuple<int, int>(currentCycle, registerValue * currentCycle);
                    }
                    else
                    {
                        yield return new Tuple<int, int>(currentCycle, registerValue * currentCycle);
                    }
                }
            }

            string? line;
            while ((line = sr.ReadLine()) != null)
            {
                string[] lineGroup = line.Split(" ");
                if (lineGroup[0] == "noop")
                {
                    newCycles = 1;
                    incrementRegister = 0;
                }
                else if (lineGroup[0] == "addx")
                {
                    newCycles = 2;
                    incrementRegister = Int32.Parse(lineGroup[1]);
                }
                else
                {
                    Console.WriteLine("Invalid command");
                    throw new Exception();
                }

                foreach (Tuple<int, int> output in YieldSignalStrength
                (
                    registerValue: registerValue,
                    currentCycle: currentCycle,
                    newCycles: newCycles,
                    incrementRegister: incrementRegister
                ))
                {
                    if (checkpoints.Contains(output.Item1))
                    {
                        Console.WriteLine($"Value is {output.Item2}");
                        res_q1 += output.Item2;
                    }
                    sprite = new List<int> { registerValue - 1, registerValue, registerValue + 1 };

                    if (sprite.Contains(crtPosition))
                    {
                        picture.Add("#");
                    }
                    else
                    {
                        picture.Add(".");
                    }

                    crtPosition += 1;
                    if (crtPosition == 40)
                    {
                        crtPosition = 0;
                    }
                }

                // Adjust inputs for next line
                currentCycle += newCycles;
                registerValue += incrementRegister;

            }

            Console.WriteLine($"Answer for q1 is {res_q1}");

            Console.WriteLine("Answering question 2");
            Console.WriteLine($"{String.Join(" ", picture.GetRange(0, 40).ToArray())}");
            Console.WriteLine($"{String.Join(" ", picture.GetRange(40, 40).ToArray())}");
            Console.WriteLine($"{String.Join(" ", picture.GetRange(80, 40).ToArray())}");
            Console.WriteLine($"{String.Join(" ", picture.GetRange(120, 40).ToArray())}");
            Console.WriteLine($"{String.Join(" ", picture.GetRange(160, 40).ToArray())}");
            Console.WriteLine($"{String.Join(" ", picture.GetRange(200, 40).ToArray())}");
        }

        public IEnumerable<int> YieldRegisterValue(int currentRegister)
        {

            yield return currentRegister;
        }
    }
}