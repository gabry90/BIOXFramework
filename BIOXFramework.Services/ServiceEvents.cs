using System;

namespace BIOXFramework.Services
{
    public class ServiceRegisteredEventArgs : EventArgs
    {
        public ServiceRegisteredEventArgs(Type service) { Service = service; }
        public Type Service { get; private set; }
    }

    public class ServiceUnregisteredEventArgs : EventArgs
    {
        public ServiceUnregisteredEventArgs(Type service) { Service = service; }
        public Type Service { get; private set; }
    }
}