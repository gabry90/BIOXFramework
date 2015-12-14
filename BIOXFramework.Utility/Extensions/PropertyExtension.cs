using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using BIOXFramework.Utility.Helpers;

namespace BIOXFramework.Utility.Extensions
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ExcludeFromPropertyConflict : Attribute { }

    [AttributeUsage(AttributeTargets.Property)]
    public class ExcludeFromPropertCopy : Attribute { }

    public static class PropertyExtensions
    {
        public enum PropertyCopyOptions
        {
            Included,
            Excluded
        }

        public static void CopyProperties<T>(this T self, T target, PropertyCopyOptions option, params string[] properties) where T : class
        {
            if (target == null)
                return;

            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy;

            foreach (var self_property in self.GetType().GetProperties(flags))
            {
                if (!self_property.CanRead)
                    continue;

                var target_property = target.GetType().GetProperties(flags).FirstOrDefault(x => string.Equals(x.Name, self_property.Name));
                if (target_property == null || !target_property.CanRead || !target_property.CanWrite)
                    continue;

                //skip property that not allow to copy
                if (properties != null)
                {
                    if (option == PropertyCopyOptions.Included)
                    {
                        if (!properties.Contains(target_property.Name))
                            continue;
                    }
                    else
                    {
                        if (properties.Contains(target_property.Name))
                            continue;
                    }
                }

                object self_value = self_property.GetValue(self, null);
                object target_value = target_property.GetValue(target, null);

                if (target_value != null)
                    target_value.DisposeEx();

                target_property.SetValue(target, Convert.ChangeType(self_value, target_property.PropertyType), null);
            }
        }

        public static object GetPropertyValue<T>(this T self, string propertyName, params Type[] onlyAttribute)
        {
            if (string.IsNullOrEmpty(propertyName))
                return null;

            Type type = self.GetType();

            var property = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy).FirstOrDefault(x => string.Equals(x.Name, propertyName));
            if (property == null || !property.CanRead)
                return null;

            if (onlyAttribute.Length > 0 && onlyAttribute.Count(x => Attribute.IsDefined(property, x)) != onlyAttribute.Length)
                return null;   //no attributes match

            return property.GetValue(self, null);
        }

        public static Dictionary<string, object> GetPropertiesValues<T>(this T self, params Type[] onlyAttribute)
        {
            Type type = self.GetType();

            Dictionary<string, object> properties = new Dictionary<string, object>();
            foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy))
            {
                if (!property.CanRead)
                    continue;   //the property cannot readable

                if (onlyAttribute.Length > 0 && onlyAttribute.Count(x => Attribute.IsDefined(property, x)) != onlyAttribute.Length)
                    continue;   //no attributes match

                properties.Add(property.Name, property.GetValue(self, null));
            }
            return properties;
        }

        public static Dictionary<string, object> GetPropertyConflict<T>(this T self, T original, params Type[] onlyAttribute)
        {
            if (original == null)
                return null;    //avoid null object

            Type type = self.GetType();
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy;

            if (type != original.GetType())
                return null;    //no type match

            Dictionary<string, object> properties = new Dictionary<string, object>();

            foreach (var self_prop in type.GetProperties(flags))
            {
                if (!self_prop.CanRead)
                    continue;   //the property cannot readable

                if (Attribute.IsDefined(self_prop, typeof(ExcludeFromPropertyConflict)))
                    continue;   //skip excluded element

                if (onlyAttribute.Length > 0 && onlyAttribute.Count(x => Attribute.IsDefined(self_prop, x)) != onlyAttribute.Length)
                    continue;   //no attributes match

                var original_prop = original.GetType().GetProperties(flags).FirstOrDefault(x => string.Equals(x.Name, self_prop.Name));
                if (original_prop == null)
                    continue;   //no property match

                object self_value = self_prop.GetValue(self, null);
                object original_value = original_prop.GetValue(original, null);

                //consider string null with empty
                if (self_prop.PropertyType == typeof(String))
                {
                    self_value = self_value == null ? "" : self_value;
                    original_value = original_value == null ? "" : original_value;
                }

                //check if property is equals
                if (!ComparisonHelper.IsEquals(self_value, original_value))
                    properties.Add(self_prop.Name, self_value);
            }

            return properties;
        }

        public static void SetPropertyValue<T>(this T self, string propertyName, object propertyValue)
        {
            if (string.IsNullOrEmpty(propertyName))
                return;

            Type type = self.GetType();

            var property = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy).FirstOrDefault(x => string.Equals(x.Name, propertyName));
            if (property != null && property.CanWrite)
                property.SetValue(self, Convert.ChangeType(propertyValue, property.PropertyType), null);
        }

        public static void CopyProperties<T>(this T self, object target, params Type[] onlyAttribute)
        {
            if (target == null)
                return;

            Type type = self.GetType();
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy;

            foreach (var self_prop in type.GetProperties(flags))
            {
                if (!self_prop.CanRead || !self_prop.CanWrite)
                    continue;   //the property is not accessible

                if (Attribute.IsDefined(self_prop, typeof(ExcludeFromPropertCopy)))
                    continue;   //skip excluded element

                var target_prop = target.GetType().GetProperties(flags).FirstOrDefault(x => string.Equals(x.Name, self_prop.Name));
                if (target_prop == null)
                    continue;

                if (onlyAttribute.Length > 0 && onlyAttribute.Count(x => Attribute.IsDefined(self_prop, x)) != onlyAttribute.Length)
                    continue;   //no attributes match

                object self_value = self_prop.GetValue(self, null);
                target_prop.SetValue(target, Convert.ChangeType(self_value, target_prop.PropertyType), null);
            }
        }

        public static void ResetProperties<T>(this T self)
        {
            Type type = self.GetType();

            foreach (var property in  type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy))
            {
                if (property.CanWrite)
                    property.SetValue(self, property.PropertyType.GetDefaultValue(), null);
            }
        }

        public static List<T> GetIListPropertyItemsAdded<T>(this List<T> self, List<T> original, Func<T, bool> predicate = null)
        {
            if (original == null)
                return self.CloneEx();

            List<T> properties = new List<T>();
            foreach (T self_item in self)
            {
                T original_item = predicate == null ? original.FirstOrDefault(x => ComparisonHelper.IsEquals(x, self_item)) : original.FirstOrDefault(predicate);
                if (original_item == null)
                    properties.Add(self_item);
            }

            return properties;
        }

        public static List<T> GetIListPropertyItemsRemoved<T>(this List<T> self, List<T> original, Func<T, bool> predicate = null)
        {
            if (original == null)
                return null;

            List<T> properties = new List<T>();
            foreach (T original_item in original)
            {
                T self_item = predicate == null ? self.FirstOrDefault(x => ComparisonHelper.IsEquals(x, original_item)) : self.FirstOrDefault(predicate);
                if (self_item == null)
                    properties.Add(original_item);
            }
            return properties;
        }

        public static List<T> GetIListPropertyItemsEdited<T>(this List<T> current, List<T> original, Func<T, bool> predicate = null)
        {
            if (current == null || original == null)
                return null;

            List<T> properties = new List<T>();
            foreach (T self_item in current)
            {
                T original_item = predicate == null ? original.FirstOrDefault(x => ComparisonHelper.IsEquals(x, self_item)) : original.FirstOrDefault(predicate);
                if (original_item != null)
                {
                    Dictionary<string, object> p = self_item.GetPropertyConflict(original_item);
                    if (p != null && p.Count > 0)
                        properties.Add(self_item);
                }
            }
            return properties;
        }
    }
}