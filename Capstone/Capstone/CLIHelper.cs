using System;
using System.Collections.Generic;
using System.Text;
using Capstone.DAL;

namespace Capstone
{
    /// <summary>
    /// This class contains helper methods that should help get valid input from users.
    /// </summary>
    public static class CLIHelper
    {
        public static int GetInteger(string message)
        {
            string userInput;
            int intValue;
            int numberOfAttempts = 0;

            do
            {
                if (numberOfAttempts > 0)
                {
                    Console.WriteLine("Invalid input format. Please try again");
                }
                Console.Write(message + " ");
                userInput = Console.ReadLine();

                numberOfAttempts++;
            }
            while (!int.TryParse(userInput, out intValue));

            return intValue;
        }

        public static double GetDouble(string message)
        {
            string userInput;
            double doubleValue;
            int numberOfAttempts = 0;

            do
            {
                if (numberOfAttempts > 0)
                {
                    Console.WriteLine("Invalid input format. Please try again");
                }

                Console.Write(message + " ");
                userInput = Console.ReadLine();
                numberOfAttempts++;
            }
            while (!double.TryParse(userInput, out doubleValue));

            return doubleValue;
        }

        public static bool GetBool(string message)
        {
            string userInput;
            bool boolValue;
            int numberOfAttempts = 0;

            do
            {
                if (numberOfAttempts > 0)
                {
                    Console.WriteLine("Invalid input format. Please try again");
                }

                Console.Write(" ");
                userInput = Console.ReadLine();
                numberOfAttempts++;
            }
            while (!bool.TryParse(userInput, out boolValue));

            return boolValue;
        }

        public static string GetString(string message)
        {
            string userInput;
            int numberOfAttempts = 0;

            do
            {
                if (numberOfAttempts > 0)
                {
                    Console.WriteLine("Invalid input format. Please try again");
                }

                Console.Write(message + " ");
                userInput = Console.ReadLine();
                numberOfAttempts++;
            }
            while (string.IsNullOrEmpty(userInput));

            return userInput;
        }

        public static DateTime FirstOf(int month)
        {
            DateTime now = DateTime.Now;
            return new DateTime(now.Year, month, 1);
        }
        public static DateTime LastOf(int month)
        {
            DateTime now = DateTime.Now;
            return new DateTime(now.Year, month, DateTime.DaysInMonth(now.Year, month));
        }
    }
}
