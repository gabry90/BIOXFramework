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

    public class ServiceEnabledChangedEventArgs : EventArgs
    {
        public ServiceEnabledChangedEventArgs(Type service, bool enabled) 
        { 
            Service = service;
            Enabled = enabled;
        }
        public Type Service { get; private set; }
        public bool Enabled { get; private set; }
    }
}