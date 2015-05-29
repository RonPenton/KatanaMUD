using KatanaMUD.Models;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis.Scripting.CSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace KatanaMUD.Scripts
{
    public static class ScriptManager
    {
        static Dictionary<string, Type> _roomScripts = new Dictionary<string, Type>(StringComparer.InvariantCultureIgnoreCase);

        public static IRoomScript GetRoomScript(string name, Room room)
        {
            var script = GetScript<IRoomScript>(name, new Lazy<IRoomScript>(() => new DefaultRoomScript()), _roomScripts);
            script.ControllingRoom = room;
            return script;
        }

        private static T GetScript<T>(string name, Lazy<T> defaultScript, Dictionary<string, Type> dictionary)
        {
            Type type;
            if (!dictionary.TryGetValue(name, out type))
                return defaultScript.Value;
            return (T)type.Assembly.CreateInstance(type.FullName);
        }

        public static void LoadScripts()
        {
            Load<IRoomScript>("Rooms", _roomScripts);
        }

        private static void Load<T>(string directoryName, Dictionary<string, Type> dictionary)
        {
            var files = Directory.EnumerateFiles(@"..\Scripts\" + directoryName, "*.cs", SearchOption.AllDirectories);
            List<Type> types = new List<Type>();
            files.ForEach(x => types.AddRange(Load<T>(File.ReadAllText(x))));
            types.ForEach(x => dictionary[x.Name] = x);
        }

        private static IEnumerable<Type> Load<T>(string code)
        {
            //TODO: Figure out if we need to load references dynamically to support a rich scripting environment.
            ScriptOptions options = ScriptOptions.Default
                .AddReferences(new System.IO.FileInfo(@"..\..\artifacts\bin\KatanaMUD\Debug\dnx451\KatanaMUD.dll").FullName)
                .AddReferences(new System.IO.FileInfo(@"..\..\artifacts\bin\KatanaMUD.Helpers\Debug\dnx451\KatanaMUD.Helpers.dll").FullName)
                .AddReferences(typeof(Enumerable).Assembly)
                .AddNamespaces("System.Linq")
                .AddNamespaces("KatanaMUD")
                .AddNamespaces("KatanaMUD.Helpers");

            var script = CSharpScript.Create(code + @"
            public class __Anchor {
                int dummy;
            }
            var anchor = new __Anchor();", options);

            var state = script.Run();
            var anchor = state.Variables["anchor"].Value;

            var t = anchor.GetType();
            var a = t.Assembly;
            var ts = a.GetTypes();

            return anchor.GetType().Assembly.GetTypes().Where(x => x.ImplementsInterface<T>());
        }
    }
}
