using System;
using System.Reflection;
using System.Linq;

namespace KatanaMUD
{
    public static class ReflectionHelper
    {
        public static bool IsSubclassOfGeneric(this Type toCheck, Type generic)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur)
                {
                    return true;
                }
                toCheck = toCheck.BaseType;
            }
            return false;
        }

        public static PropertyInfo GetProperty(this Type type, string propertyName)
        {
            return type.GetProperties().Single(x => x.Name == propertyName);
        }

        public static bool ImplementsInterface<InterfaceType>(this Type toCheck)
        {
            return typeof(InterfaceType).IsAssignableFrom(toCheck);
        }
    }
}
