using System;
using System.Globalization;

namespace BIOXFramework.Utility.Extensions
{
    public static class NumericExtensions
    {
        public static int GetDecimalPart(this float number)
        {
            return Int32.Parse(number.ToString("0.0######", CultureInfo.InvariantCulture).Split('.')[1]);
        }

        public static int GetIntPart(this float number)
        {
            return Int32.Parse(number.ToString("0.0", CultureInfo.InvariantCulture).Split('.')[0]);
        }
    }
}