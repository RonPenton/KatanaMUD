using System;
using KatanaMUD.Models;

namespace KatanaMUD.Messages
{
	public class CommunicationMessage : MessageBase
	{
		public string Message { get; set; }
		public CommunicationType Type { get; set; }
		public string Chatroom { get; set; }
		public int User { get; set; }

		public override void Process(Actor actor)
		{
			//TODO: process message to remove extranaeous characters/codes.

			switch (Type)
			{
				case CommunicationType.Gossip: Gossip(); break;
			}
		}

		private void Gossip()
		{
			//TODO: Check if user is allowed to gossip.
			var connections = Game.Connections.GetConnections();
			foreach (var connection in connections)
			{
				connection.Actor.SendMessage(this);
			}
		}
	}

	public enum CommunicationType {
		Gossip,
		Auction,
		Say,
		Yell,
		Region,
		Gangpath,
		Officerpath,
		Chatroom,
		Telepath
	}
}