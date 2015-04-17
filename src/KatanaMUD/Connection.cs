using KatanaMUD.Messages;
using KatanaMUD.Models;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Collections.Concurrent;

namespace KatanaMUD
{
	public class Connection
	{
		public Connection(WebSocket socket, User user, Actor actor)
		{
			if (actor.Connection != null)
				throw new InvalidOperationException("Connection already attached to actor");

			Socket = socket;
			User = user;
			Actor = actor;
			actor.MessageHandler = new ConnectionMessageHandler(actor);
			actor.Connection = this;
		}

		public WebSocket Socket { get; private set; }
		public User User { get; private set; }
		public Actor Actor { get; private set; }
	}
}