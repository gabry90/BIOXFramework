using System;
using System.Linq;
using System.Collections.Generic;
using BIOXFramework.Utility.Extensions;

namespace BIOXFramework.Utility.Helpers
{
    public static class ComparisonHelper
    {
        public static bool IsEquals(object obj1, object obj2)
        {
            //check both null value
            if (obj1 == null && obj2 == null)
                return true;

            //check one obj is null
            if (obj1 == null || obj2 == null)
                return false;

            //check nullable value
            Type t1 = Nullable.GetUnderlyingType(obj1.GetType()) ?? obj1.GetType();
            Type t2 = Nullable.GetUnderlyingType(obj2.GetType()) ?? obj2.GetType();
            obj1 = obj1 == null ? null : Convert.ChangeType(obj1, t1);
            obj2 = obj2 == null ? null : Convert.ChangeType(obj2, t2);

            //check type comparison
            if (t1 != t2)
                return false;

            //check numeric
            if (t1.IsNumeric())
                return object.Equals(obj1, obj2);

            //check enum
            if (t1 == typeof(Enum))
                return (Enum)obj1 == (Enum)obj2;

            //check string
            if (t1 == typeof(string))
                return string.Equals((string)obj1, (string)obj2);

            //check boolean
            if (t1 == typeof(bool))
                return (bool)obj1 == (bool)obj2;

            //check DateTime
            if (t1 == typeof(DateTime))
                return (DateTime)obj1 == (DateTime)obj2;

            //check generic object
            return Equals(t1, t2);
        }
    }
}