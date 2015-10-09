using System;
using System.Linq;
using System.Reflection;

namespace BIOXFramework.Utility.Extensions
{
    public static class OperationExtension
    {
        public static T CloneEx<T>(this T self)
        {
            return self is ICloneable ? (T)((ICloneable)self).Clone() : (T)self.GetType().GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(self, null);
        }

        public static void DisposeEx<T>(this T self)
        {
            //if is dispose call normal interface member
            if (self is IDisposable)
            {
                ((IDisposable)self).Dispose();
                return;
            }

            /*
             * if not is IDisposable try to reset manually all members
             */

            //set to default all properties
            self.ResetProperties();

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