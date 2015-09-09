using System;

namespace BIOXFramework.Services
{
    public class ServiceException : Exception
    {
        public ServiceException(string message)
            : base(string.Format("[BIOXFramework.Services Exception]: {0}", message))
        {

        }
    }
}