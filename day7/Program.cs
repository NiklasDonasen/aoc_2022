using System;
using System.Linq;
using System.IO;

namespace aoc_2022_day7
{
    internal class Program
    {
        static void Main()
        {
            StreamReader sr = new StreamReader("input.txt");

            int res_q1 = 0;

            string curDir = "/";

            // Try to store absolute file paths
            HashSet<string> absolutePaths = new HashSet<string>();
            List<string> tempPath = new List<string>();

            // Here we store filesizes of each directory once we have a number for it
            Dictionary<string, int> sizePerDirectory = new Dictionary<string, int>();

            string? line;
            while ((line = sr.ReadLine()) != null)
            {
                string[] lineGroup = line.Split(" ");
                if (lineGroup[0] == "$")
                {
                    // we got a command
                    if (lineGroup[1] == "ls")
                    {
                        Console.WriteLine($"About to get some file sizes for directory {curDir}");
                        continue;
                    }
                    else if (lineGroup[1] == "cd")
                    {
                        // switching directories
                        if (lineGroup[2] == "..")
                        {
                            tempPath.RemoveAt(tempPath.Count - 1);

                            curDir = tempPath.Last();
                            continue;
                        }
                        else
                        {
                            // handle the first line in input
                            if (curDir == "/" && lineGroup[2].ToString() == "/")
                            {
                                tempPath.Add(curDir);
                                continue;
                            }

                            string newDir = lineGroup[2].ToString();
                            // if `cd` is followed by a new directory name
                            tempPath.Add(newDir);

                            // Save current absolute path to HashSet
                            absolutePaths.Add(string.Join(",", tempPath));

                            // Switch directories
                            curDir = newDir;
                        }
                    }
                }
                else
                {
                    if (lineGroup[0] == "dir")
                    {
                        // we only add the path when we know its size
                        continue;
                    }
                    else
                    {
                        // now we assume that the line starts with a number
                        try
                        {
                            int fileSize = Int32.Parse(lineGroup[0]);
                            string currentPath = string.Join(",", tempPath);

                            // Add to current temp path size

                            if (sizePerDirectory.ContainsKey(currentPath))
                            {
                                int currentSize = sizePerDirectory[currentPath];
                                sizePerDirectory[currentPath] = currentSize + fileSize;
                            }
                            else
                            {
                                sizePerDirectory[currentPath] = fileSize;
                            }

                            // Only add to all directories higher up if you are not already in root folder
                            if (!(tempPath.Count == 1 && tempPath[0] == "/"))
                            {
                                // add to all directories higher up
                                int counter = tempPath.Count;

                                // create a copy
                                List<string> clonedTempPath = new List<string>(tempPath);
                                while (counter > 0)
                                {
                                    // we are going up one and one directory
                                    clonedTempPath.RemoveAt(clonedTempPath.Count - 1);
                                    if (clonedTempPath.Count > 0)
                                    {
                                        string oneUpPath = string.Join(",", clonedTempPath);

                                        // Check if the
                                        if (sizePerDirectory.ContainsKey(oneUpPath))
                                        {
                                            int currentSize = sizePerDirectory[oneUpPath];
                                            sizePerDirectory[oneUpPath] = currentSize + fileSize;
                                        }
                                        else
                                        {
                                            sizePerDirectory[oneUpPath] = fileSize;
                                        }

                                        counter -= 1;
                                    }
                                    else
                                    {
                                        break;
                                    }

                                }
                            }

                        }
                        catch
                        {
                            Console.WriteLine("Expected to get a number. Error");
                            throw new Exception();
                        }
                    }

                }
            }

            // Calculate answer to question 1
            foreach (string key in sizePerDirectory.Keys)
            {
                if (sizePerDirectory[key] <= 100000)
                {
                    res_q1 += sizePerDirectory[key];
                }
            }

            Console.WriteLine($"Answer to question 1 is {res_q1}");

            // Calculate answer to question 2
            int necessarySpace = 30000000 - (70000000 - sizePerDirectory["/"]);
            List<int> possibleAnswers = new List<int>();

            foreach (string key in sizePerDirectory.Keys)
            {
                if (sizePerDirectory[key] >= necessarySpace)
                {
                    possibleAnswers.Add(sizePerDirectory[key]);
                }
            }

            int res_q2 = possibleAnswers.Min();

            Console.WriteLine($"Answer to question 2 is {res_q2}");

        }
    }
}