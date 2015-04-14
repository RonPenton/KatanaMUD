using KatanaMUD.Messages;
using System.Threading.Tasks;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Linq;
using KatanaMUD.Models;

namespace KatanaMUD
{
	public class Game
	{
		public static ConnectionManager Connections { get; } = new ConnectionManager();

		public static GameEntities Data { get; private set; }

		async public static void Run()
		{
			Data = new GameEntities("Server=localhost;Database=KatanaMUD;integrated security=True;");
			Data.LoadFromDatabase();

			DateTime lastTime = DateTime.UtcNow;
			var pingTime = lastTime;

			try
			{
				Console.WriteLine("KatanaMUD 0.1 Server Started");
				while (true)
				{
					var newTime = DateTime.UtcNow;

					// Grab all the messages from the connections which can be processed at the moment.
					List<Tuple<Connection, MessageBase>> messages = new List<Tuple<Connection, MessageBase>>();
					foreach (var connection in Connections.GetConnections())
					{
						MessageBase m;
						if (connection.CanProcessMessage && connection.Messages.TryDequeue(out m))
						{
							messages.Add(new Tuple<Connection, MessageBase>(connection, m));
						}
					}
					messages = messages.OrderBy(x => x.Item2.MessageTime).ToList();


					foreach (var message in messages)
					{
						message.Item2.Process(message.Item1);
					}

					lastTime = newTime;
					Thread.Sleep(50);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				throw;
			}
		}
	}
}