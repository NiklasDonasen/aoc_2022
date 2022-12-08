using System;
using System.IO;

namespace aoc_2022_day8
{
    internal class Program
    {
        static void Main()
        {
            string[] forrest = System.IO.File.ReadAllLines("input.txt");

            int totalRows = forrest.Count();
            int totalCols = forrest[0].Length;

            // We have to keep track of which trees we already marked as visible
            bool[,] visibleTrees = new bool[totalRows, totalCols];

            // for question 2 we keep an array of integers
            int[,] distanceTrees = new int[totalRows, totalCols];

            // We can consider row for row, but columns have to be saved 
            Dictionary<int, string> columns = new Dictionary<int, string>();
            foreach (int number in Enumerable.Range(0, totalCols))
            {
                string startString = "";
                columns[number] = startString;
            }

            // Go over the input
            for (int row = 0; row < totalRows; row++)
            {
                // Store information on columns for later assessment
                for (int i = 0; i < totalCols; i++)
                {
                    string currentString = columns[i];
                    columns[i] = currentString + forrest[row][i];
                }

                // Assess the row
                // Question 2
                distanceTrees = CountDistantTreesPerRow(distanceTrees, forrest[row], row);

                // Question 1
                if (row == 0 || row == totalRows - 1)
                {
                    // those are the top and bottom row. They are all visible
                    for (int i = 0; i < totalCols; i++)
                    {
                        visibleTrees[row, i] = true;
                    }
                    continue;
                }

                // Edge trees are visible
                visibleTrees[row, 0] = true;
                visibleTrees[row, totalCols - 1] = true;

                // Go over from left to right
                bool[] visibilityForOneLine = DetectVisibleTrees(forrest[row]);
                visibleTrees = SaveRow(visibleTrees, visibilityForOneLine, row);

                // To go over from right to left, you have to first reverse it and then back reverse it
                char[] charArray = forrest[row].ToCharArray();
                Array.Reverse(charArray);
                string reversedRow = new string(charArray);
                bool[] reversedVisibilityForOneLine = DetectVisibleTrees(reversedRow);
                Array.Reverse(reversedVisibilityForOneLine);
                visibleTrees = SaveRow(visibleTrees, reversedVisibilityForOneLine, row);
            }

            // assess columns
            foreach (int key in columns.Keys)
            {
                // QUestion 2
                distanceTrees = CountDistantTreesPerCol(distanceTrees, columns[key], key);

                // Question 1
                // all edges are visible
                if (key == 0 || key == totalCols - 1)
                {
                    for (int i = 0; i < totalRows; i++)
                    {
                        visibleTrees[i, key] = true;
                    }
                }

                // Top to bottom
                bool[] visibilityForOneCol = DetectVisibleTrees(columns[key]);
                visibleTrees = SaveCol(visibleTrees, visibilityForOneCol, key);
                // visibleTrees = DetectVisibleTreesCols(visibleTrees, columns[key], key, false);

                // Bottom to top
                char[] charArray = columns[key].ToCharArray();
                Array.Reverse(charArray);
                string reversedCol = new string(charArray);
                bool[] reversedVisibilityForOneCol = DetectVisibleTrees(reversedCol);
                Array.Reverse(reversedVisibilityForOneCol);
                visibleTrees = SaveCol(visibleTrees, reversedVisibilityForOneCol, key);
            }

            int res_q1 = visibleTrees.Cast<bool>().Where(c => c).Count();
            Console.WriteLine($"Answer for q1 is {res_q1}");

            int res_q2 = distanceTrees.Cast<int>().Max();
            Console.WriteLine($"Answer for q2 is {res_q2}");

        }

        public static bool[] DetectVisibleTrees(string line)
        {
            bool[] visibilityForOneLine = new bool[line.Length];
            // first and last trees are always visible since they are on the edge
            visibilityForOneLine[0] = true;
            visibilityForOneLine[line.Length - 1] = true;

            // Check out the other trees
            for (int i = 1; i < line.Length - 1; i++)
            {
                // if tree is bigger than ALL the previous, then it is visible
                int treesAwayFromEdge = i;
                bool visible = false;

                while (treesAwayFromEdge > 0)
                {
                    if (Int32.Parse(line[i].ToString()) > Int32.Parse(line[treesAwayFromEdge - 1].ToString()))
                    {
                        visible = true;
                    }
                    else
                    {
                        visible = false;
                        // if there is at least one tree hiding you, you are not visible
                        break;
                    }
                    treesAwayFromEdge -= 1;
                }

                visibilityForOneLine[i] = visible;
            }
            return visibilityForOneLine;
        }

        public static bool[,] SaveRow(bool[,] visibleTrees, bool[] visibilityForOneLine, int row)
        {
            for (int i = 0; i < visibilityForOneLine.Count(); i++)
            {
                if (visibleTrees[row, i])
                {
                    // if already marked as visible before, do not tuckle with that
                    continue;
                }
                visibleTrees[row, i] = visibilityForOneLine[i];
            }

            return visibleTrees;
        }

        public static bool[,] SaveCol(bool[,] visibleTrees, bool[] visibilityForOneLine, int col)
        {
            for (int i = 0; i < visibilityForOneLine.Count(); i++)
            {
                if (visibleTrees[i, col])
                {
                    // if already marked as visible before, do not tuckle with that
                    continue;
                }
                visibleTrees[i, col] = visibilityForOneLine[i];
            }

            return visibleTrees;
        }

        public static int[,] CountDistantTreesPerRow(int[,] distanceTrees, string line, int rowNum)
        {
            for (int i = 0; i < line.Length; i++)
            {
                int lowerTreesOneDirection = IterateOverLineOneDirection(line, i);
                int lowerTreesOtherDirection = IterateOverLineOtherDirection(line, i);

                distanceTrees[rowNum, i] = lowerTreesOtherDirection * lowerTreesOneDirection;
            }

            return distanceTrees;
        }

        public static int[,] CountDistantTreesPerCol(int[,] distanceTrees, string line, int colNum)
        {
            for (int i = 0; i < line.Length; i++)
            {
                int lowerTreesOneDirection = IterateOverLineOneDirection(line, i);
                int lowerTreesOtherDirection = IterateOverLineOtherDirection(line, i);

                distanceTrees[i, colNum] *= lowerTreesOtherDirection * lowerTreesOneDirection;
            }

            return distanceTrees;
        }

        public static int IterateOverLineOneDirection(string line, int i)
        {
            int lowerTrees = 0;
            // check one direction, e.g. left hand side
            if (i > 0)
            {
                int lhs = i - 1;
                while (lhs >= 0)
                {
                    if (line[i] > line[lhs])
                    {
                        lowerTrees += 1;
                        lhs -= 1;
                        continue;
                    }
                    else
                    {
                        lowerTrees += 1;
                        break;
                    }
                }
            }
            return lowerTrees;
        }
        public static int IterateOverLineOtherDirection(string line, int i)
        {
            int lowerTrees = 0;
            // check other direction, e.g. right hand side
            if (i < line.Length)
            {
                int rhs = i + 1;
                while (rhs < line.Length)
                {
                    if (line[i] > line[rhs])
                    {
                        lowerTrees += 1;
                        rhs += 1;
                        continue;
                    }
                    else
                    {
                        lowerTrees += 1;
                        break;
                    }
                }
            }
            return lowerTrees;
        }
    }
}