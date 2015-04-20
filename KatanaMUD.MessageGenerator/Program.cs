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
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            var dll = Assembly.LoadFrom(@"..\..\..\artifacts\bin\KatanaMUD\Debug\aspnet50\KatanaMUD.dll");

            var messageType = dll.GetTypes().Single(x => x.Name == "MessageBase");
            var messages = dll.GetTypes().Where(x => x.IsSubclassOf(messageType));

            HashSet<Type> enumBuilder = new HashSet<Type>();
            HashSet<Type> typeBuilder = new HashSet<Type>();
            ScanForTypes(messages, enumBuilder, typeBuilder);


            using (var script = File.Open(@"..\..\..\src\KatanaMUD\wwwroot\scripts\Messages.g.ts", FileMode.Create))
            {
                using (var writer = new StreamWriter(script))
                {
                    writer.WriteLine("module KMud {");

                    foreach (var message in messages)
                    {
                        writer.WriteLine(String.Format("    export class {0} extends MessageBase {{", message.Name));
                        writer.WriteLine(String.Format("        constructor() {{ super('{0}'); }}", message.Name));

                        var properties = message.GetProperties().Where(x => x.Name != "MessageName" && x.Name != "MessageTime");
                        foreach (var property in properties)
                        {
                            writer.WriteLine(String.Format("        public {1}: {0};", GetPropertyType(property.PropertyType), property.Name));
                        }

                        writer.WriteLine(String.Format("        public static ClassName: string = '{0}';", message.Name));
                        writer.WriteLine("    }");
                    }

                    foreach (var type in typeBuilder)
                    {
                        BuildType(type, writer);
                    }

                    foreach (var enumeration in enumBuilder)
                    {
                        BuildEnum(enumeration, writer);
                    }

                    writer.WriteLine("}");
                }
            }
        }

        private static void ScanForTypes(IEnumerable<Type> messages, HashSet<Type> enumBuilder, HashSet<Type> typeBuilder)
        {
            foreach (var type in messages)
            {
                ScanForTypes(type, false, enumBuilder, typeBuilder);
            }
        }

        private static void ScanForTypes(Type type, bool includeType, HashSet<Type> enumBuilder, HashSet<Type> typeBuilder)
        {

            if (includeType && !typeBuilder.Add(type))
                return; // trying to include the type, but it's already been included. Already scanned, short-circuit out to prevent infinite recursion.

            foreach (var property in type.GetProperties())
            {
                ScanType(property.PropertyType, enumBuilder, typeBuilder);
            }
        }

        private static void ScanType(Type type, HashSet<Type> enumBuilder, HashSet<Type> typeBuilder)
        {
            if (isEnum(type))
            {
                enumBuilder.Add(getEnum(type));
            }
            else if (isArray(type))
            {
                ScanType(getArrayType(type), enumBuilder, typeBuilder);
            }
            else if (isDictionary(type))
            {
                ScanType(getDictionaryValueType(type), enumBuilder, typeBuilder);
            }
            else if (type.IsClass && type != typeof(string))
            {
                ScanForTypes(type, true, enumBuilder, typeBuilder);
            }
        }

        private static void BuildType(Type type, StreamWriter writer)
        {
            writer.WriteLine(String.Format("    export class {0} {{", type.Name));

            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                writer.WriteLine(String.Format("        public {1}: {0};", GetPropertyType(property.PropertyType), property.Name));
            }
            writer.WriteLine("    }");
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            // ASP.NET vNEXT assemblies know nothing of the GAC or local loading. Everything is package-based. 
            // So, search for the assemblies in the artifacts folder, hoping I remembered to turn local builds on,
            // and if that fails, load from the package cache, hoping they've been restored. 
            // Everything old is new again, DLL hell, here we come!
            var name = args.Name.Split(',').First() + ".dll";

            var file = Directory.EnumerateFiles(@"..\..\..\artifacts\bin\", name, SearchOption.AllDirectories);
            if (file.Count() > 0)
            {
                return Assembly.LoadFrom(file.First());
            }

            try
            {
                file = Directory.EnumerateFiles(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".kpm", "packages"), name, SearchOption.AllDirectories);
                if (file.Count() > 0)
                {
                    return Assembly.LoadFrom(file.First());
                }
            }
            catch (Exception) { }

            try
            {
                file = Directory.EnumerateFiles(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".dnx", "packages"), name, SearchOption.AllDirectories);
                if (file.Count() > 0)
                {
                    return Assembly.LoadFrom(file.First());
                }
            }
            catch (Exception) { }

            try
            {
                file = Directory.EnumerateFiles(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".k", "packages"), name, SearchOption.AllDirectories);
                if (file.Count() > 0)
                {
                    return Assembly.LoadFrom(file.First());
                }
            }
            catch (Exception) { }

            return null;
        }

        private static bool isArray(Type type)
        {
            if (type.IsArray)
                return true;
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
                return true;
            return false;
        }

        private static Type getArrayType(Type type)
        {
            if (type.IsArray)
                return type.GetElementType();
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
                return type.GetGenericArguments().First();
            return null;
        }

        private static bool isDictionary(Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
            {
                if (type.GetGenericArguments().First() != typeof(int) && type.GetGenericArguments().First() != typeof(string))
                {
                    throw new InvalidOperationException("Cannot make dictionaries with key types other than int and string");
                }
                return true;
            }
            return false;
        }

        private static Type getDictionaryValueType(Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
            {
                if (type.GetGenericArguments().First() != typeof(int) && type.GetGenericArguments().First() != typeof(string))
                {
                    throw new InvalidOperationException("Cannot make dictionaries with key types other than int and string");
                }
                return type.GetGenericArguments().Skip(1).First();
            }
            return null;
        }

        private static bool isFlags(Type type)
        {
            return type.IsEnum && type.GetCustomAttribute<FlagsAttribute>() != null;
        }

        private static bool isEnum(Type type)
        {
            if (type.IsEnum)
                return true;

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>) && type.GetGenericArguments().First().IsEnum)
                return true;

            return false;
        }

        private static Type getEnum(Type type)
        {
            if (type.IsEnum)
                return type;

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>) && type.GetGenericArguments().First().IsEnum)
                return type.GetGenericArguments().First();

            return null;
        }


        private static string GetPropertyType(Type type)
        {
            if (isEnum(type))
            {
                return getEnum(type).Name;
            }

            if (isArray(type))
            {
                return GetPropertyType(getArrayType(type)) + "[]";
            }

            if (isDictionary(type))
            {
                return String.Format("{{ [index: {0}] : {1} }}", GetPropertyType(type.GetGenericArguments().First()),
                    GetPropertyType(type.GetGenericArguments().Last()));
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return GetPropertyType(type.GetGenericArguments().First());
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
            if (type == typeof(string))
                return "string";

            if (type.IsClass)
            {
                return type.Name;
            }

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