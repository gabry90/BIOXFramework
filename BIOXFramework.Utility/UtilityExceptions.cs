using System;

namespace BIOXFramework.Utility
{
    public class UtilityException : Exception
    {
        public UtilityException(string message)
            : base(string.Format("[BIOXFramework.Utility Exception]: {0}", message))
        {

        }
    }
}