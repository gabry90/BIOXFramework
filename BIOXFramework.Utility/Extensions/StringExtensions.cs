using System;
using System.Text.RegularExpressions;

namespace BIOXFramework.Utility.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNumeric(this Type type)
        {
            return type.IsValueType && type != typeof(bool) && type != typeof(Enum) && type != typeof(DateTime);
        }

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
    }
}