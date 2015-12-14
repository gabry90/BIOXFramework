using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BIOXFramework.Utility.Extensions
{
    public static class StringExtensions
    {
        public static string InLine(this string self, string replacedChar = " ")
        {
            if (replacedChar == null
                || string.Equals(replacedChar, Environment.NewLine)
                || string.Equals(replacedChar, "\n")
                || replacedChar.Contains(Environment.NewLine)
                || replacedChar.Contains("\n")
                || replacedChar.Contains("\r"))
                replacedChar = " ";
            return Regex.Replace(self, @"[\f\n\t\v\r]", replacedChar);
        }

        public static string Substring(this string self, string from = null, string until = null, StringComparison comparison = StringComparison.InvariantCulture)
        {
            var fromLength = (from ?? string.Empty).Length;
            var startIndex = !string.IsNullOrEmpty(from)
                ? self.IndexOf(from, comparison) + fromLength
                : 0;

            if (startIndex < fromLength)
                return null;

            var endIndex = !string.IsNullOrEmpty(until)
            ? self.IndexOf(until, startIndex, comparison)
            : self.Length;

            if (endIndex < 0)
                return null;

            var subString = self.Substring(startIndex, endIndex - startIndex);
            return subString;
        }

        public static void Clear(this StringBuilder sb)
        {
            sb.Length = 0;
        }

        public static bool IsNullOrWhiteSpace(this string str)
        {
            return str == null || str.Trim() == "";
        }

        public static bool IsNumeric(this Type type)
        {
            return type.IsValueType && type != typeof(bool) && !type.IsEnum && type != typeof(DateTime);
        }

        public static List<string> SplitByLenght(this string self, int lenght)
        {
            if (lenght <= 0 || self.Length <= lenght)
                return new List<string> { self };

            return self
                .Where((x, i) => i % lenght == 0)
                .Select(
                    (x, i) => new string(self
                        .Skip(i * lenght)
                        .Take(lenght)
                        .ToArray()))
                .ToList();
        }

        public static int CountWithCarriageReturn(this string self)
        {
            return self.Contains("\n") ? self.Length + self.Split('\n').Length - 1 : self.Length;
        }
    }
}