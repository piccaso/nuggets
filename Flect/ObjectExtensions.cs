using System;
using System.Collections.Generic;
using System.Reflection;

namespace Flect
{
    public static class ObjectExtensions
    {
        public static void Flect<T>() where T : new() => new T().Flect();
        public static void Flect<T>(this T o) where T:new()
        {
            var t = typeof(T);
            var instanceProperties = t.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            GetAndSet(instanceProperties, o);
            t.Flect();
        }

        public static void Flect(this Type t)
        {
            object instance;
            try
            {
                instance = Activator.CreateInstance(t);
            }
            catch (MissingMethodException)
            {
                instance = null;
            }

            var flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;
            t.GetProperties(flags).GetAndSet(instance);
            t.GetFields(flags).GetAndSet(instance);
        }

        private static void GetAndSet(this IEnumerable<PropertyInfo> propertyInfos, object o)
        {
            foreach (var prop in propertyInfos)
            {
                if (!prop.CanRead) continue;
                var val = prop.GetValue(o);
                if (prop.CanWrite)
                {
                    prop.SetValue(o, val);
                }
            }
        }
        private static void GetAndSet(this IEnumerable<FieldInfo> fieldInfos, object o)
        {
            foreach (var prop in fieldInfos)
            {
                var val = prop.GetValue(o);
                prop.SetValue(o, val);
            }
        }
    }
}
