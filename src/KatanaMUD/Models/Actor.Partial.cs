using System;
using System.Collections.Concurrent;
using KatanaMUD.Messages;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace KatanaMUD.Models
{
	public partial class Actor
	{
		internal Connection Connection { get; set; }

		private Party _party;

		public Party Party
		{
			get
			{
				if (_party == null)
					_party = new Party(this);
				return _party;
			}
			set
			{
				if (value == _party)
					return;

				if (_party == null || _party.Members.Count() == 1)
				{
					_party = value;
					value.Add(this);
					return;
				}

				throw new InvalidOperationException("Cannot set new party without leaving existing party.");
			}
		}

		public IMessageHandler MessageHandler { get; set; }

		public void SendMessage(MessageBase message)
		{
			if (MessageHandler != null)
				MessageHandler.HandleMessage(message);
		}

		private ConcurrentQueue<MessageBase> _messages { get; } = new ConcurrentQueue<MessageBase>();

		internal void AddMessage(MessageBase message)
		{
			_messages.Enqueue(message);
			Game.ActiveActors.Add(this);
		}

		internal MessageBase GetNextMessage(out bool remaining)
		{
			MessageBase result;
			_messages.TryDequeue(out result);
			remaining = _messages.Count > 0;
			return result;
		}


		/// <summary>
		/// Call this when a user is to change rooms, independent of party. 
		/// </summary>
		/// <param name="room"></param>
		/// <param name="exitRoom"></param>
		public void ChangeRooms(Room room, int? exitRoom)
		{
			Room newRoom = Game.Data.Rooms.Single(x => x.Id == exitRoom);

			// TODO: notify old room of user exiting. 
			Room = newRoom;
			// TODO: notify new room of user entering.
		}

		public void SendRoomDescription(Room room)
		{
			var message = new RoomDescriptionMessage()
			{
				Description = room.TextBlock.Text,
				IsCurrentRoom = room == this.Room,
				Name = room.Name,
				RoomId = room.Id
				//TODO: VisibleItems = ?
				//TODO: Actors = ?
			};
			message.SetExits(room);

			SendMessage(message);
		}
	}

	public class Party
	{
		public Party(Actor leader, params Actor[] actors)
			: this(leader, actors.AsEnumerable()) { }

		public Party(Actor leader, IEnumerable<Actor> actors)
		{
			_members.AddRange(actors);
			_members.Add(leader);
			Leader = leader;
			_members.ForEach(x => x.Party = this);
		}

		private Actor _leader;

		public Actor Leader
		{
			get { return _leader; }
			set
			{
				if (!_members.Contains(value))
					throw new InvalidOperationException("Leader must exist within the party.");
				_leader = value;
			}
		}

		public IEnumerable<Actor> Members => _members.ToList().AsReadOnly();

		public void Add(Actor actor)
		{
			if (actor.Party == this)
				return;

			if (actor.Party.Members.Count() > 1)
				throw new InvalidOperationException("Cannot add actor to party, since they are already in a party.");

			actor.Party = this;
			_members.Add(actor);
		}

		public void Remove(Actor actor)
		{
			if (actor == _leader)
				throw new InvalidOperationException("Cannot Remove the leader from a party.");
			if (_members.Remove(actor))
				actor.Party = null;
		}

		public void Disband()
		{
			foreach (var member in _members)
			{
				member.Party = null;
			}
			_members.Clear();
		}

		private HashSet<Actor> _members { get; } = new HashSet<Actor>();


		public bool CanMove(Exit exit)
		{
			//TODO: check if party can move. 
			return true;
		}

		/// <summary>
		/// Moves the player through the exit. 
		/// NOTE: This assumes that CanMove has already been called and heeded. 
		/// If you ignore that, the consequences are yours to deal with.
		/// </summary>
		/// <param name="exit"></param>
		public void Move(Exit exit)
		{
			if (exit.ExitRoom != null)
			{
				var newRoom = Game.Data.Rooms.Single(x => x.Id == exit.ExitRoom.Value);
				//TODO: Construct room messages
				Move(newRoom, null, null);
			}
			else
			{
				//TODO: Perform portal movement here
			}
		}

		public void Move(Room newRoom, RoomMessage exit, RoomMessage entrance)
		{
			//TODO: send exit message to _leader.Room

			Members.ForEach(x =>
			{
				x.Room = newRoom;
				x.SendRoomDescription(newRoom);
			});

			//TODO: send entrance message to newRoom
		}
	}
}