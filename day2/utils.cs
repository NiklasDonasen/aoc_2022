using System.IO;

namespace aoc_2022_day2
{
    public class utils
    {
        public static int MyOwnPoints(string my_command)
        {
            switch (my_command)
            {
                // rock
                case "X":
                case "A":
                    return 1;
                // paper
                case "Y":
                case "B":
                    return 2;
                // scissor
                case "Z":
                case "C":
                    return 3;
                default:
                    Console.WriteLine("Invalid command from you");
                    return 0;
            }
        }

        public static string FindMyCommand(string my_goal, string opponent)
        {
            // X loose
            // Y draw
            // Z win

            // return the same if draw
            if (string.Equals(my_goal, "Y"))
            {
                return opponent;
            }
            // if loose
            else if (string.Equals(my_goal, "X"))
            {
                switch (opponent)
                {
                    case "A":
                        return "C";
                    case "B":
                        return "A";
                    case "C":
                        return "B";
                }
            }
            // if win
            else if (string.Equals(my_goal, "Z"))
            {
                switch (opponent)
                {
                    case "A":
                        return "B";
                    case "B":
                        return "C";
                    case "C":
                        return "A";
                }
            }
            Console.WriteLine("Invalid goal for you");
            return "A";
        }

        public static int Competition(string my_command, string opponent)
        {
            // rock > scissors
            // paper > rocks
            // scissors > paper

            // We cast the opponent's commands to my commands
            string opponent_converted = "X";
            switch (opponent)
            {
                case "A":
                    opponent_converted = "X";
                    break;
                case "B":
                    opponent_converted = "Y";
                    break;
                case "C":
                    opponent_converted = "Z";
                    break;
                default:
                    Console.WriteLine("Invalid command from opponent");
                    opponent_converted = "X";
                    break;
            }

            // if both equals
            if (string.Equals(my_command, opponent_converted))
            {
                return 3;
            }
            // I have rock and opponent scissors
            else if (string.Equals(my_command, "X") && String.Equals(opponent_converted, "Z"))
            {
                return 6;
            }
            // I have paper and opponent rocks
            else if (string.Equals(my_command, "Y") && String.Equals(opponent_converted, "X"))
            {
                return 6;
            }
            // I have scissors and opponent paper
            else if (string.Equals(my_command, "Z") && String.Equals(opponent_converted, "Y"))
            {
                return 6;
            }
            // else my opponent wins
            else
            {
                return 0;
            }
        }

        public static int Competition_q2(string my_command, string opponent)
        {
            // rock > scissors
            // paper > rocks
            // scissors > paper

            // if both equals
            if (string.Equals(my_command, opponent))
            {
                return 3;
            }
            // I have rock and opponent scissors
            else if (string.Equals(my_command, "A") && String.Equals(opponent, "C"))
            {
                return 6;
            }
            // I have paper and opponent rocks
            else if (string.Equals(my_command, "B") && String.Equals(opponent, "A"))
            {
                return 6;
            }
            // I have scissors and opponent paper
            else if (string.Equals(my_command, "C") && String.Equals(opponent, "B"))
            {
                return 6;
            }
            // else my opponent wins
            else
            {
                return 0;
            }
        }
    }
}