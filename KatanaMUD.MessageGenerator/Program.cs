using System;
using System.Collections.Generic;
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
			var dll = Assembly.ReflectionOnlyLoadFrom(@"..\..\..\artifacts\bin\KatanaMUD.Messages\Debug\aspnet50\KatanaMUD.Messages.dll");

			var messageType = dll.GetTypes().Single(x => x.Name == "MessageBase");
			var messages = dll.GetTypes().Where(x => x.IsSubclassOf(messageType));

		}
	}
}
