using System;

namespace aoc_2022_day3
{
    public class utils
    {
        public static char FindCommonLetter(string firstHalf, string secondHalf)
        {
            foreach (char letter in firstHalf)
            {
                int occurence = secondHalf.Count(t => t == letter);
                if (occurence >= 1)
                {
                    return letter;
                }
            }

            Console.WriteLine("Found no common letter");
            throw new ArgumentException("Common letter expected in both strings");
        }

        public static char FindCommonLetterForGroupOfThree(string firstElv, string secondElv, string thirdElv)
        {
            foreach (char letter in firstElv)
            {
                int occurence = secondElv.Count(t => t == letter);
                if (occurence >= 1)
                {
                    if (thirdElv.Contains(letter))
                    {
                        return letter;
                    }
                }
            }

            Console.WriteLine("Found no common letter");
            throw new ArgumentException("Common letter expected in both strings");
        }

        public static int CalculatePoints(char commonLetter)
        {
            if (char.IsLower(commonLetter))
            {
                int index = char.ToUpper(commonLetter) - 64;
                return index;
            }
            else
            {
                int index = char.ToUpper(commonLetter) - 64 + 26;
                return index;
            }
        }
    }
}