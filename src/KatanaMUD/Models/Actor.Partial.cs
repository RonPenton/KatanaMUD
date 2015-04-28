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

        public void SendRoomDescription(Room room, bool brief = false)
        {
            var message = new RoomDescriptionMessage()
            {
                IsCurrentRoom = room == this.Room
            };

            var illumination = room.Illumination + Illumination;
            message.LightLevel = LightLevels.Get(illumination);

            
            if (LightLevels.IsTooDarkToSee(message.LightLevel))
            {
                // Too dark to see. Send the light information and that's it.
                message.CannotSee = true;
                SendMessage(message);
                return;
            }

            //TODO: Check to see if the user is blind? Somehow. I have no idea yet. 

            if (!brief)
            {
                message.Description = room.TextBlock.Text;
            }

            //TODO: Hidden stuff

            message.Name = room.Name;
            message.RoomId = room.Id;

            var exits = room.GetExits();
            message.Exits = exits.Select(x => new ExitDescription(x, room)).ToArray();
            message.Actors = room.VisibleActors(this).Select(x => new ActorDescription(x)).ToArray();
            message.VisibleItems = room.Items.Where(x => x.HiddenTime == null).OrderBy(x => x.Name).Select(x => new ItemDescription(x)).ToArray();
            message.VisibleCash = Game.Data.Currencies.OrderByDescending(x => x.Value).Select(x => new CurrencyDescription(x, room.GetCash(x))).Where(x => x.Amount != 0).ToArray();

            message.FoundItems = room.Items.Where(x => x.HiddenTime != null && x.UsersWhoFoundMe.Contains(this)).OrderBy(x => x.Name).Select(x => new ItemDescription(x)).ToArray();
            message.FoundCash = Game.Data.Currencies.OrderByDescending(x => x.Value).Select(x => new CurrencyDescription(x, room.GetTotalCashUserCanSee(x, this).KnownHidden)).Where(x => x.Amount != 0).ToArray();

            SendMessage(message);
        }

        /// <summary>
        /// Determines if an actor can get an item.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Validation CanGetItem(Item item)
        {
            // Make sure fixed items aren't able to be gotten
            if (item.ItemTemplate.Fixed)
                return new Validation("You cannot pick up that item!");

            // Make sure item is actually in the room.
            if (item.Room != Room)
                return new Validation("You are not in the same room as that item!");

            // Check weight.
            if (Encumbrance + item.Weight > MaxEncumbrance)
                return new Validation("You cannot carry that much!");

            //TODO: Ask item if it can be gotten. 
            return new Validation();
        }

        public Validation CanGetCash(Currency currency, long? quantity)
        {
            var q = Room.GetTotalCashUserCanSee(currency, this);

            if (quantity == null || quantity == 0)
            {
                // No quantity specified. Set total to the amount that the user knows about. Auto-max.
                quantity = q.Total;
            }

            // Make sure item is actually in the room.
            if (q.Total < quantity)
                return new Validation("You do not see that here!");

            // Check weight.
            if (Encumbrance + (currency.Weight * quantity) > MaxEncumbrance)
                return new Validation("You cannot carry that much!");

            return new Validation();
        }

        /// <summary>
        /// Gets an item. Beware that this performs no checks and essentially forces a get. 
        /// Notifying the room of the item transfer is up to the executor of this method.
        /// </summary>
        /// <param name="item"></param>
        public void GetItem(Item item)
        {
            item.Actor = this;
            item.Room = null;
        }

        /// <summary>
        /// Gets cash. Beware that this performs minimal checks and essentially forces a get. 
        /// Notifying the room of the item transfer is up to the executor of this method.
        /// </summary>
        /// <param name="item"></param>
        public void GetCash(Currency currency, long? quantity)
        {
            var q = Room.GetTotalCashUserCanSee(currency, this);
            var total = Room.GetTotalCash(currency);

            if (quantity == null)
            {
                // No quantity specified. Set total to the amount that the user knows about. Auto-max.
                quantity = q.Total;
            }

            // Make sure we don't grab more than what's on the floor. 
            if (quantity > total.Total)
                quantity = total.Total;

            var visible = Math.Min(quantity.Value, q.Visible);
            var hidden = quantity.Value - visible;

            Currency.Add(currency, Cash, quantity.Value);
            Currency.Add(currency, Room.Cash, -visible);
            Currency.Add(currency, Room.HiddenCash, -hidden);
        }

        /// <summary>
        /// Determines if an actor can drop an item.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Validation CanDropItem(Item item)
        {
            // Make sure item is actually in the room.
            if (item.Actor != this)
                return new Validation("Item is not in the same room");

            // TODO: Ask item for a reason why it can't be dropped?
            if (item.ItemTemplate.NotDroppable)
                return new Validation("You cannot drop that!");

            //TODO: Ask item if it can be dropped.

            return new Validation();
        }

        /// <summary>
        /// Drops an item. Beware that this performs no checks and essentially forces a drop. 
        /// </summary>
        /// <param name="item"></param>
        public void DropItem(Item item)
        {
            item.Actor = null;
            item.Room = Room;
        }

        public Validation CanDropCash(Currency currency, long? quantity)
        {
            var q = Currency.Get(currency, Cash);
            if (quantity == null)
                quantity = q;

            if (quantity > q)
                return new Validation("You don't see that many " + currency.Name + " here!");

            return new Validation();
        }

        public void DropCash(Currency currency, long? quantity)
        {
            var q = Currency.Get(currency, Cash);
            if (quantity == null)
                quantity = q;

            if (quantity > q)
                quantity = q;

            Currency.Add(currency, Cash, -quantity.Value);
            Currency.Add(currency, Room.Cash, quantity.Value);
        }

        public IEnumerable<Item> EquippedItems => Items.Where(x => x.EquippedSlot != null);
    }

    /// <summary>
    /// Represents a party of actors. All actors are in an implcit 1-person party at all times. 
    /// Parties are not database entities, we don't bother to persist that data to disk.
    /// </summary>
	public class Party
    {
        public Party(Actor leader, params Actor[] actors)
            : this(leader, actors.AsEnumerable())
        { }

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

        public Validation CanMove(Exit exit)
        {
            if (Members.Any(x => x.Encumbrance > x.MaxEncumbrance))
            {
                return new Validation("At least one person in your party is too heavy to move!");
            }

            return new Validation();
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
                var oldRoom = Leader.Room;
                var newRoom = Game.Data.Rooms.Single(x => x.Id == exit.ExitRoom.Value);

                var partyDescription = this.Members.OrderBy(x => x != Leader).ThenBy(x => x.Name).Select(x => new ActorDescription(x)).ToArray();

                var message = new PartyMovementMessage()
                {
                    Leader = new ActorDescription(Leader),
                    Actors = partyDescription,
                    Direction = exit.Direction,
                    Enter = false
                };
                oldRoom.ActiveActors.ForEach(x => x.SendMessage(message));

                Move(newRoom, null, null);

                message.Direction = Directions.Opposite(exit.Direction);
                message.Enter = true;
                newRoom.ActiveActors.ForEach(x => x.SendMessage(message));

                // send "movement" messages to adjacent rooms. 
                var exits = newRoom.GetExits();
                foreach (var ex in exits)
                {
                    if (ex.ExitRoom != null)
                    {
                        if (ex.ExitRoom != oldRoom.Id)
                        {
                            var room = Game.Data.Rooms[ex.ExitRoom.Value];
                            var generic = new GenericMessage();
                            generic.Message = Directions.Format("You hear movement to the {0}.", "You hear movement {0} you.", Directions.Opposite(ex.Direction));
                            generic.Class = "hear-movement";
                            room.ActiveActors.ForEach(x => x.SendMessage(generic));
                        }
                    }
                    else
                    {
                        // TODO: Portal notification here? 
                    }
                }
            }
            else
            {
                //TODO: Perform portal movement here
            }
        }

        public void Move(Room newRoom, RoomMessage exit, RoomMessage entrance)
        {
            Members.ForEach(x =>
            {
                x.Room = newRoom;
            });

            Members.ForEach(x =>
            {
                // send description AFTER the whole party has moved, because members might have lights.
                x.SendRoomDescription(newRoom);
            });
        }
    }
}