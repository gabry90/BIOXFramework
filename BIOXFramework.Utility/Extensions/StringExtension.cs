using System;

namespace BIOXFramework.Utility.Extensions
{
    public static class StringExtension
    {
        public static bool IsNumeric(this Type type)
        {
            return type.IsValueType && type != typeof(bool) && type != typeof(Enum) && type != typeof(DateTime);
        }
    }
}