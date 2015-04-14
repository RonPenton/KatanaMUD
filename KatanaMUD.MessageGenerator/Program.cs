using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KatanaMUD.MessageGenerator
{
	class Program
	{
		static void Main(string[] args)
		{
			var dll = Assembly.LoadFrom(@"..\..\..\artifacts\bin\KatanaMUD.Messages\Debug\aspnet50\KatanaMUD.dll");

			var messageType = dll.GetTypes().Single(x => x.Name == "MessageBase");
			var messages = dll.GetTypes().Where(x => x.IsSubclassOf(messageType));

            HashSet<Type> enumBuilder = new HashSet<Type>();
            using (var script = File.Open(@"..\..\..\src\KatanaMUD\wwwroot\scripts\Messages.g.ts", FileMode.Create))
            {
                using (var writer = new StreamWriter(script))
                {
                    writer.WriteLine("module KMud {");

                    foreach(var message in messages)
                    {
                        writer.WriteLine(String.Format("    export class {0} extends MessageBase {{", message.Name));
                        writer.WriteLine(String.Format("        constructor() {{ super('{0}'); }}", message.Name));

                        var properties = message.GetProperties().Where(x => x.Name != "MessageName");
                        foreach(var property in properties)
                        {
                            writer.WriteLine(String.Format("        public {1}: {0};", GetPropertyType(property.PropertyType, enumBuilder), property.Name));
                        }

                        writer.WriteLine(String.Format("        public static ClassName: string = '{0}';", message.Name));
                        writer.WriteLine("    }");
                    }

                    foreach(var enumeration in enumBuilder)
                    {
                        BuildEnum(enumeration, writer);
                    }

                    writer.WriteLine("}");
                }
            }
		}

        private static bool isArray(Type type)
        {
            if (type.IsArray)
                return true;
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
                return true;
            return false;
        }

        private static bool isDictionary(Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
                return true;
            return false;
        }

        private static bool isFlags(Type type)
        {
            return type.IsEnum && type.GetCustomAttribute<FlagsAttribute>() != null;
        }


        private static string GetPropertyType(Type type, HashSet<Type> enumBuilder)
        {
            if(type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return GetPropertyType(type.GetGenericArguments().First(), enumBuilder);
            }

            if (isArray(type))
            {
                if (type.IsArray)
                    return GetPropertyType(type.GetElementType(), enumBuilder) + "[]";
                else
                    return GetPropertyType(type.GetGenericArguments().First(), enumBuilder) + "[]";
            }

            if(isDictionary(type))
            {
                return String.Format("{{ [index: {0}] : {1} }}", GetPropertyType(type.GetGenericArguments().First(), enumBuilder), 
                    GetPropertyType(type.GetGenericArguments().Last(), enumBuilder));
            }

            if(type.IsEnum)
            {
                enumBuilder.Add(type);
                return type.Name;
            }

            if (type == typeof(bool))
                return "boolean";

            if (type == typeof(int) ||
                type == typeof(decimal) ||
                type == typeof(float) ||
                type == typeof(double))
                return "number";
            if (type == typeof(Object))
                return "any";
            if (type == typeof(DateTime))
                return "Date";

            return "string";
        }

        public static void EnumerateEnumeration(Type type, Action<string, int> functor)
        {
            var values = Enum.GetValues(type);
            var names = Enum.GetNames(type);
            var members = type.GetMembers(BindingFlags.Public | BindingFlags.Static);
            for (int i = 0; i < values.Length; i++)
            {
                var member = members.Single(x => x.Name == names[i]);
                int value = Convert.ToInt32(values.GetValue(i));
                string key = names[i];
                functor(key, value);
            }
        }


        private static void BuildEnum(Type type, StreamWriter writer)
        {
            writer.WriteLine(String.Format("    export enum {0} {{", type.Name));
            EnumerateEnumeration(type, (key, value) =>
            {
                writer.WriteLine(String.Format("        {0} = {1},", key, value));
            });
            writer.WriteLine("    }");
        }

    }
}
