using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis.Scripting.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KatanaMUD.Scripts
{
    public static class ScriptLoader
    {
        public static IEnumerable<Type> Load<T>(string code)
        {
            //TODO: Figure out if we need to load references dynamically to support a rich scripting environment.
            ScriptOptions options = ScriptOptions.Default
                .AddReferences(new System.IO.FileInfo(@"..\..\artifacts\bin\KatanaMUD\Debug\dnx451\KatanaMUD.dll").FullName)
                .AddNamespaces("KatanaMUD");

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
