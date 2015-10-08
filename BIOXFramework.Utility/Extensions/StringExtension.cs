using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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