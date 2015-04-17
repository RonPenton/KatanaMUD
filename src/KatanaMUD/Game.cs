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
using KatanaMUD.Helpers;

namespace KatanaMUD
{
	public class Game
	{
		public static ConnectionManager Connections { get; } = new ConnectionManager();

		public static GameEntities Data { get; private set; }

		public static ConcurrentSet<Actor> ActiveActors { get; } = new ConcurrentSet<Actor>();

		async public static void Run()
		{
			Data = new GameEntities("Server=localhost;Database=KatanaMUD;integrated security=True;");
			Data.LoadFromDatabase();

			DateTime lastTime = DateTime.UtcNow;
			var pingTime = lastTime;
			var saveTime = lastTime.AddMinutes(1);

			Console.WriteLine("KatanaMUD 0.1 Server Started");
			while (true)
			{
				try
				{
					var newTime = DateTime.UtcNow;

					// Grab a snapshot of all active actors (will clear the active list)
					var actors = ActiveActors.Snapshot();

					// Extract the messages from the actors, and order them by time. 
					var messages = actors.Select(x =>
					{
						bool remaining;
						var message = x.GetNextMessage(out remaining);
						return Tuple.Create(x, message, remaining);
					}).Where(x => x.Item2 != null).OrderBy(x => x.Item2.MessageTime).ToList();

					// If any actors have more messages, re-add them to the active list.
					// we only grab the top message from each actor because each action may have a time component to it,
					// so that if the action causes the user to pause, we cannot process the second message yet. 
					// We'll grab the next message on the next game loop. It's not the best system, but it's
					// workable, and it serves to effectively limit messages to 1 per loop, or about 10-20 per second
					// depending on how long the loop sleep period is. I don't believe this will be a problem in practice,
					// though I suppose there's always the possibility of a timing exploit somewhere. 
					foreach (var message in messages.Where(x => x.Item3 == true))
					{
						ActiveActors.Add(message.Item1);
					}

					foreach (var message in messages)
					{
						message.Item2.Process(message.Item1);
					}

					lastTime = newTime;

					// Save changes to the database.
					//if (Data.ForceSave || saveTime < newTime)
					//{
					//	Data.ForceSave = false;
					//	// TODO: Make the save time configurable eventually.
					//	saveTime = newTime.AddMinutes(1);
					//	Data.SaveChanges();
					//} 

					Thread.Sleep(50);
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.ToString());
				}
			}
		}
	}
}