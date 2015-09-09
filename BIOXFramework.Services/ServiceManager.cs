using System;
using System.Linq;
using System.Collections.Generic;

namespace BIOXFramework.Services
{
    public static class ServiceManager
    {
        public static event EventHandler<ServiceRegisteredEventArgs> ServiceRegistered;
        public static event EventHandler<ServiceUnregisteredEventArgs> ServiceUnregistered;

        private static Dictionary<Type, IBIOXFrameworkService> _services = new Dictionary<Type, IBIOXFrameworkService>();

        public static void Register<T>(T service) where T : IBIOXFrameworkService
        {
            if (_services.ContainsKey(typeof(T)))
                throw new ServiceException(string.Format("the service \"{0}\" is already registered!", typeof(T).FullName));

            _services.Add(typeof(T), service);
        }

        public static void Unregister<T>() where T : IBIOXFrameworkService
        {
            if (!_services.ContainsKey(typeof(T)))
                throw new ServiceException(string.Format("the service \"{0}\" is not registered!", typeof(T).FullName));

            T service = (T)_services[typeof(T)];
            service.Dispose();
            _services.Remove(typeof(T));
        }

        public static T Get<T>() where T : IBIOXFrameworkService
        {
            if (!_services.ContainsKey(typeof(T)))
                throw new ServiceException(string.Format("the service \"{0}\" is not registered!", typeof(T).FullName));

            return (T)_services[typeof(T)];
        }

        private static void ServiceRegisteredEventDispatcher(ServiceRegisteredEventArgs e)
        {
            var h = ServiceRegistered;
            if (h != null)
                h(null, e);
        }

        private static void ServiceUnregisteredEventDispatcher(ServiceUnregisteredEventArgs e)
        {
            var h = ServiceUnregistered;
            if (h != null)
                h(null, e);
        }
    }
}