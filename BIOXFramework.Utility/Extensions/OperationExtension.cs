using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace BIOXFramework.Utility.Extensions
{
    public static class OperationExtension
    {
        public static T CloneEx<T>(this T self)
        {
            return EqualityComparer<T>.Default.Equals(self, default(T)) ? default(T) : (self is ICloneable ? (T)((ICloneable)self).Clone() : (T)self.GetType().GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(self, null));
        }

        public static void DisposeEx<T>(this T self)
        {
            Type type = self.GetType();

            type.GetEvents(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).ToList().ForEach(x =>
            {
                FieldInfo fi = type.GetField(x.Name, BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);
                if (fi == null)
                    return;

                Delegate del = (Delegate)fi.GetValue(self);
                if (del != null)
                    del.GetInvocationList().ToList().ForEach(y => x.RemoveEventHandler(self, y));
            });

            if (self is IDisposable) ((IDisposable)self).Dispose();
        }
    }
}