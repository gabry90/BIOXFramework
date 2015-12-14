using System;
using System.Linq;
using System.Reflection;

namespace BIOXFramework.Utility.Extensions
{
    public static class OperationExtension
    {
        public static T CallMethod<T>(this T self, string methodName, params object[] parameters)
        {
            return (T)self.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy).Invoke(self, parameters);
        }

        public static T CloneEx<T>(this T self)
        {
            return self is ICloneable ? (T)((ICloneable)self).Clone() : (T)self.GetType().GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(self, null);
        }

        public static void DisposeEx<T>(this T self)
        {
            //if is IDisposable call standard interface member
            if (self is IDisposable)
            {
                ((IDisposable)self).Dispose();
                return;
            }

            Type type = self.GetType();

            //dispose events
            type.GetEvents(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).ToList().ForEach(x =>
            {
                FieldInfo fi = type.GetField(x.Name, BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);
                if (fi == null)
                    return;

                Delegate del = (Delegate)fi.GetValue(self);
                if (del != null)
                    del.GetInvocationList().ToList().ForEach(y => x.RemoveEventHandler(self, y));
            });
        }
    }
}