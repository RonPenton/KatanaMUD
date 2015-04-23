using System;
using System.Collections.Concurrent;
using KatanaMUD.Messages;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Spam;

namespace KatanaMUD.Models
{
    public partial class Actor
    {
        private Party _party;

        private ConcurrentQueue<MessageBase> _messages = new ConcurrentQueue<MessageBase>();

        /// <summary>
        /// Keeps track of whether the connection has been unexpectedly disconnected. In that case,
        /// we need to prevent the connection from being used in case of a thread context switch. 
        /// </summary>
        internal bool UnhandledDisconnection { get; set; }

        /// <summary>
        /// The connection for the actor. 
        /// </summary>
		internal Connection Connection { get; set; }

        /// <summary>
        /// Informs us whether the user is "in purgatory", ie logged off. 
        /// Meaning the actor is attached to a user, but has no connection. 
        /// </summary>
        internal bool InPurgatory => User != null && Connection == null;

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
            {
                // special case, don't send messages if we're in a disconnected state.
                if (MessageHandler is ConnectionMessageHandler && UnhandledDisconnection)
                    return;
                MessageHandler.HandleMessage(message);
            }
        }

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
            };

            //TODO: Hidden stuff
            var exits = room.GetExits();
            message.Exits = exits.Select(x => new ExitDescription(x, room)).ToArray();
            message.Actors = room.Actors.Select(x => new ActorDescription(x)).ToArray();
            message.VisibleItems = room.Items.Select(x => new ItemDescription(x)).ToArray();

            SendMessage(message);
        }

        public T GetStat<T>(string name, T baseValue, bool includePercent = true)
        {
            List<JsonContainer> containers = new List<JsonContainer>();

            containers.Add((JsonContainer)Stats);
            containers.AddRange(Items.Select(x => (JsonContainer)x.Stats));
            //TODO: Buffs here

            return JsonContainer.Calculate<T>(containers, name, baseValue, includePercent);
        }

        public long MaxEncumbrance
        {
            get
            {
                var strength = GetStat<long>("Strength", 0);
                return GetStat<long>("MaxEncumbrance", strength * 48);
            }
        }

        public long Encumbrance => Items.Sum(x => x.ItemTemplate.Weight);

        /// <summary>
        /// Determines if an actor can get an item.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool CanGetItem(Item item)
        {
            // Make sure fixed items aren't able to be gotten
            if (item.ItemTemplate.Fixed)
                return false;

            // Make sure item is actually in the room.
            if (item.Room != Room)
                return false;

            // Check weight.
            if (Encumbrance + item.ItemTemplate.Weight > MaxEncumbrance)
                return false;

            //TODO: Ask item if it can be gotten. 

            return true;
        }

        /// <summary>
        /// Gets an item. Beware that this performs no checks and essentially forces a get. 
        /// </summary>
        /// <param name="item"></param>
        public void GetItem(Item item)
        {
            item.Actor = this;
            item.Room = null;

            //TODO: Notify item it's been gotten.
        }

        /// <summary>
        /// Determines if an actor can drop an item.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool CanDropItem(Item item)
        {
            // Make sure item is actually in the room.
            if (item.Actor != this)
                return false;

            if (item.ItemTemplate.NotDroppable)
                return false;

            //TODO: Ask item if it can be dropped. 

            return true;
        }

        /// <summary>
        /// Drops an item. Beware that this performs no checks and essentially forces a drop. 
        /// </summary>
        /// <param name="item"></param>
        public void DropItem(Item item)
        {
            item.Actor = null;
            item.Room = Room;

            //TODO: Notify item it's been dropped.
        }
    }

    /// <summary>
    /// Represents a party of actors. All actors are in an implcit 1-person party at all times. 
    /// Parties are not database entities, we don't bother to persist that data to disk.
    /// </summary>
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