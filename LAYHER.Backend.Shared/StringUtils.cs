using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace LAYHER.Backend.Shared
{
    public class StringUtils
    {
        public static string ObfuscateEmail(string email)
        {
            var displayCase = email;

            var partToBeObfuscated = Regex.Match(displayCase, @"[^@]*").Value;
            if (partToBeObfuscated.Length - 3 > 0)
            {
                var obfuscation = "";
                for (var i = 0; i < partToBeObfuscated.Length - 3; i++) obfuscation += "*";
                displayCase = String.Format("{0}{1}{2}{3}", displayCase[0], displayCase[1], obfuscation, displayCase.Substring(partToBeObfuscated.Length - 1));
            }
            else if (partToBeObfuscated.Length - 3 == 0)
            {
                displayCase = String.Format("{0}*{1}", displayCase[0], displayCase.Substring(2));
            }

            return displayCase;
        }

        // Generate a random string with a given size and case.   
        // If second parameter is true, the return string is lowercase  
        public static string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }

        // Generate a random password of a given length (optional)  
        public static string RandomPassword()
        {
            // Generate a random number  
            Random random = new Random();
            // Any random integer   
            int num = random.Next(100);

            StringBuilder builder = new StringBuilder();
            builder.Append(RandomString(4, true));
            builder.Append(num);
            builder.Append(RandomString(2, false));
            return builder.ToString();
        }
    }
}
