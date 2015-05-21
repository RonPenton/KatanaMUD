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
        public AddingContainer Stats;
        private ConcurrentQueue<MessageBase> _messages = new ConcurrentQueue<MessageBase>();
        private TimeSpan _nextAvailableActionTime;

        public void AddDelay(TimeSpan time)
        {
            _nextAvailableActionTime = Game.GameTime.Add(time);
        }

        partial void OnConstruct()
        {
            Stats = new AddingContainer(this.StatsInternal, GetStatContainers);
        }

        private IEnumerable<IDictionaryStore> GetStatContainers()
        {
            yield return this.StatsInternal;
            yield return this.ClassTemplate.Stats;
            yield return this.RaceTemplate.Stats;
            foreach(var item in EquippedItems)
            {
                yield return item.Stats;
            }
            //TODO: Add buffs here.
        }

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
            if(Game.GameTime < this._nextAvailableActionTime)
            {
                // Cannot process the command yet, still waiting.
                remaining = _messages.Count > 0;
                return null;
            }

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
            Room newRoom = Game.Data.Rooms[exitRoom.Value];

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

            message.Name = room.Name;
            message.RoomId = room.Id;

            var exits = room.GetExits();
            message.Exits = exits.Select(x => new ExitDescription(x, room)).ToArray();
            message.Actors = room.VisibleActors(this).Select(x => new ActorDescription(x)).ToArray();
            message.VisibleItems = room.Items.Where(x => x.HiddenTime == null).OrderBy(x => x.Name).Select(x => new ItemDescription(x)).ToArray();
            message.VisibleCash = Game.Data.AllCurrencies.Select(x => new CurrencyDescription(x, room.GetCash(x))).Where(x => x.Amount != 0).ToArray();

            message.FoundItems = room.Items.Where(x => x.HiddenTime != null && x.UsersWhoFoundMe.Contains(this)).OrderBy(x => x.Name).Select(x => new ItemDescription(x)).ToArray();
            message.FoundCash = room.GetTotalCashUserCanSee(this).Where(x => x.KnownHidden > 0).Select(x => new CurrencyDescription(x.Currency, x.KnownHidden)).ToArray();

            SendMessage(message);
        }


        /// <summary>
        /// Returns a list of all items the Actor owns which are currently equipped.
        /// </summary>
        public IEnumerable<Item> EquippedItems => Items.Where(x => x.EquippedSlot != null);

        /// <summary>
        /// Determines if an item can be removed from its equipped state.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Validation CanUnEquip(Item item)
        {
            if (item.Actor != this)
                return new Validation("You do not own that!", null);

            if (item.EquippedSlot == null)
                return new Validation("You do not have that equipped!", null);

            // TODO: Ask the item if it can be removed

            return new Validation();
        }

        /// <summary>
        /// Removes an item from the Actors equipped list.
        /// </summary>
        /// <param name="item"></param>
        public void UnEquip(Item item)
        {
            if (item.Actor != this)
                throw new InvalidOperationException("Item does not belong to Actor");

            item.EquippedSlot = null;
        }

        /// <summary>
        /// Determines if the Actor currently has an equipment slot open.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public SlotOpenResult IsSlotOpen(Item item)
        {
            if (item.ItemTemplate.EquipType == null)
                throw new InvalidOperationException("Item has no slot");

            var slot = item.ItemTemplate.EquipType.Value;

            // Offhand is a special case.
            if (slot == EquipmentSlot.Offhand)
            {
                // Already an offhand equipped
                var offhand = Items.Where(x => x.EquippedSlot == EquipmentSlot.Offhand);
                if (offhand.Any())
                    return new SlotOpenResult(offhand.ToArray());

                // no weapons equipped and no offhands equipped, which means the offhand slot is open.
                var weapon = Items.Where(x => x.EquippedSlot == EquipmentSlot.Weapon);
                if (!weapon.Any())
                    return new SlotOpenResult();

                // there's a weapon, and it's two-handed
                if (weapon.First().WeaponType.IsTwoHanded())
                    return new SlotOpenResult(weapon.ToArray());

                // Only weapons equipped are 1H, so offhand is allowed.
                return new SlotOpenResult();
            }
            else if(slot == EquipmentSlot.Weapon)
            {
                var weapon = Items.Where(x => x.EquippedSlot == EquipmentSlot.Weapon);
                var offhand = Items.Where(x => x.EquippedSlot == EquipmentSlot.Offhand);

                // Trying to equip a 2H weapon, but we have either a weapon or an offhand equipped.
                if (item.WeaponType.IsTwoHanded() && weapon.Any() && offhand.Any())
                    return new SlotOpenResult(weapon.Concat(offhand).ToArray());

                //TODO: Check for ability "DualWield".
                if (weapon.Any())
                    return new SlotOpenResult(weapon.First());

                return new SlotOpenResult();
            }

            var items = Items.Where(x => x.EquippedSlot == slot);

            // two-slot items
            if (slot.In(EquipmentSlot.Ears, EquipmentSlot.Wrists, EquipmentSlot.Fingers))
            {
                if(items.Count() >= 2)
                {
                    return new SlotOpenResult(items.First());
                }
                return new SlotOpenResult();
            }

            // everything else is 1-slot.
            if (items.Any())
                return new SlotOpenResult(items.First());
            return new SlotOpenResult();
        }

        /// <summary>
        /// Determines if the user is able to equip the given item. Please note that if there is no slot open, this will throw
        /// a validation error at you. It is assumed that you figure out which item to unequip in order to equip a new one. 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Validation CanEquipItem(Item item)
        {
            if (item.Actor != this)
                return new Validation("You do not own that item!", null);

            if (item.ItemTemplate.EquipType == null)
                return new Validation("You may not wear that item!", null);

            var open = IsSlotOpen(item);
            if(!open.IsOpen && open.ItemsToRemove.Any())
                return new Validation("You currently have an item equipped in that slot!", null);

            if (!open.IsOpen && !String.IsNullOrEmpty(open.FailureReason))
                return new Validation(open.FailureReason, null);

            // TODO: Validate Weapon Type/Armour Type
            // TODO: ask item if it can be equipped

            return new Validation();
        }

        /// <summary>
        /// Equips an item. Note: This will not unequip an item first, but will rather throw an exception. It is up to the caller
        /// To call IsSlotOpen() and CanEquipItem() first. 
        /// </summary>
        /// <param name="item"></param>
        public void EquipItem(Item item)
        {
            if (item.Actor != this)
                throw new InvalidOperationException("Item is not owned by Actor.");

            if (item.ItemTemplate.EquipType == null)
                throw new InvalidOperationException("Item cannot be worn.");

            var open = IsSlotOpen(item);
            if(!open.IsOpen && open.ItemsToRemove.Any())
                throw new InvalidOperationException("There is no slot open for the item.");

            if (!open.IsOpen && !String.IsNullOrEmpty(open.FailureReason))
                throw new InvalidOperationException(open.FailureReason);


            // Special case. Dual Wielding.
            if (item.ItemTemplate.EquipType == EquipmentSlot.Weapon)
            {
                //TODO: Check for ability "DualWield".

                // Arm the weapon offhand if they already have a weapon equipped. 
                var weapon = Items.Where(x => x.EquippedSlot == EquipmentSlot.Weapon);
                if (weapon.Any())
                    item.EquippedSlot = EquipmentSlot.Offhand;
                else
                    item.EquippedSlot = EquipmentSlot.Weapon;

                return;
            }

            item.EquippedSlot = item.ItemTemplate.EquipType;
        }
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
                return new Validation("At least one person in your party is too heavy to move!", null);
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
                var newRoom = Game.Data.Rooms[exit.ExitRoom.Value];

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

                message = new PartyMovementMessage()
                {
                    Leader = new ActorDescription(Leader),
                    Actors = partyDescription,
                    Direction = Directions.Opposite(exit.Direction),
                    Enter = true
                };
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

    public class SlotOpenResult
    {
        /// <summary>
        /// A boolean representing whether the slot is open or not.
        /// </summary>
        public bool IsOpen { get; set; }

        /// <summary>
        /// If the slot is not open, but items may be (attempted to be) removed in order to free up the slot,
        /// then this collection will contain the list of items that can be removed to free it up.
        /// </summary>
        public IEnumerable<Item> ItemsToRemove { get; set; }

        /// <summary>
        /// If the slot simply cannot be filled at the present time, then this string will contain the reason
        /// why the slot cannot be filled. 
        /// </summary>
        public string FailureReason { get; set; }

        public SlotOpenResult() { IsOpen = true; }

        public SlotOpenResult(params Item[] toRemove) { ItemsToRemove = toRemove; }

        public SlotOpenResult(string reason) { FailureReason = reason; }
    }
}