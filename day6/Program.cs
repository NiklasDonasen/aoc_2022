using System;
using System.IO;

namespace aoc_2022_day6
{
    internal class Program
    {
        static void Main()
        {
            // Configure whether you want to answer q1 (4) or q2 (14)
            int markerLength = ((int)ChooseQuestion.question2);

            string line = System.IO.File.ReadAllText("input.txt");

            HashSet<char> marker = new HashSet<char> { };

            List<char> possibleMarker = new();
            int res = 0;

            for (int i = 0; i < line.Length; i++)
            {
                // looking at a new character
                res += 1;
                Console.WriteLine($"looking at {line[i]}");
                if (possibleMarker.Count < markerLength)
                {
                    possibleMarker.Add(line[i]);
                }
                else
                {
                    possibleMarker.RemoveAt(0);
                    possibleMarker.Add(line[i]);
                }

                if (possibleMarker.Count == markerLength)
                {
                    foreach (char element in possibleMarker)
                    {
                        marker.Add(element);
                    }
                    if (marker.Count == markerLength)
                    {
                        Console.WriteLine("Found the marker");
                        break;
                    }
                    else
                    {
                        // if we do not add it then we have to clean the HashSet
                        marker.Clear();
                        Console.WriteLine("Clearing the hashset");
                        marker.Add(line[i]);
                    }
                }

            }

            Console.WriteLine($"Answer is {res}");
        }
    }
}