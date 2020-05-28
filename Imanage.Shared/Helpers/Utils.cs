using System;
using System.Linq;

namespace Imanage.Shared.Helpers
{
    public class Utils
    {
        public static string GenerateRandom(int length)
        {
            Random random = new Random();
            const string upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lower = "abcdefghijklmnopqrstuvwxyz";
            const string number = "0123456789";
            //const string symbol = "~!@#$%^&*?-_";
            //string tmp = "";
            string final = "";

            //if (length > MinLength && length < MaxLength)
            //    MinLength = length;

            //if (MustContainUpperCase)
            //    tmp += new string(Enumerable.Repeat(upper, MinUpperCaseChar).Select(s => s[random.Next(s.Length)]).ToArray());

            //if (MustContainLowerCase)
            //    tmp += new string(Enumerable.Repeat(lower, MinLowerCaseChar).Select(s => s[random.Next(s.Length)]).ToArray());

            //if (MustContainNumber)
            //    tmp += new string(Enumerable.Repeat(number, MinNumbers).Select(s => s[random.Next(s.Length)]).ToArray());

            //if (MustContainSpecialCharacter)
            //    tmp += new string(Enumerable.Repeat(symbol, MinSpecialChars).Select(s => s[random.Next(s.Length)]).ToArray());

            //if (tmp.Length < MinLength)
            //{
            //    tmp += new string(Enumerable.Repeat(lower, (MinLength - tmp.Length)).Select(s => s[random.Next(s.Length)]).ToArray());
            //}

            final = new string(Enumerable.Repeat(upper + lower + number, length).Select(s => s[random.Next(s.Length)]).ToArray());
            return final;
        }

        public static string GenerateRandomNumber(int length)
        {
            Random random = new Random();
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
