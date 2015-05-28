using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KatanaMUD.Models
{
    public partial class Actor
    {
        /// <summary>
        /// Determines if an actor can accept an item.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Validation CanAcceptItem(Item item)
        {
            // Make sure fixed items aren't able to be gotten
            if (item.ItemTemplate.Fixed)
                return new Validation().Fail("You cannot pick up that item!", Name + " cannot pick up that item!", "Base");

            // Check weight.
            if (Encumbrance + item.Weight > MaxEncumbrance)
                return new Validation().Fail("You cannot carry that much!", Name + " cannot carry that much!", "Base");

            //TODO: Ask item if it can be gotten. 
            //TODO: Ask buffs.
            return new Validation();
        }

        /// <summary>
        /// Determines if an actor can pick up an item from the floor.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Validation CanPickUpItem(Item item)
        {
            // Make sure item is actually in the room.
            if (item.Room != Room)
                return new Validation().Fail("You are not in the same room as that item!", null, "Base");

            return CanAcceptItem(item);
        }

        /// <summary>
        /// Determines if a user can accept the given amount of cash.
        /// </summary>
        /// <param name="currency"></param>
        /// <param name="quantity">The quantity requested, or leave empty to get it all.</param>
        /// <returns></returns>
        public Validation CanAcceptCash(Currency currency, long quantity)
        {
            // Check weight.
            if (Encumbrance + (currency.Weight * quantity) > MaxEncumbrance)
                return new Validation().Fail("You cannot carry that much!", Name + " cannot carry that much!", "Base");

            //TODO: Ask buffs.
            return new Validation();
        }

        /// <summary>
        /// Determines if a user can pick up cash from the floor.
        /// </summary>
        /// <param name="currency"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public Validation CanPickUpCash(Currency currency, long quantity)
        {
            var q = Room.GetTotalCashUserCanSee(currency, this);

            // Make sure item is actually in the room.
            if (q.Total < quantity)
                return new Validation().Fail("You do not see that here!");

            // Ask the room scripts if the get is allowed.
            var validation = Room.Scripts.Validate((x, v) => x.CanGetCash(Room, currency, quantity, this, v));
            if (!validation.Allowed)
                return validation;

            return CanAcceptCash(currency, quantity);
        }

        /// <summary>
        /// Gets an item. Beware that this performs no checks and essentially forces a get. 
        /// Notifying the room of the item transfer is up to the executor of this method.
        /// </summary>
        /// <param name="item"></param>
        public void AcceptItem(Item item)
        {
            item.Actor = this;
            item.Room = null;
            item.HiddenTime = null;
            item.EquippedSlot = null;

            //TODO: Tell item it has been gotten.
            //TODO: Tell previous owner item has been transferred.
        }

        /// <summary>
        /// Gets cash. Beware that this performs minimal checks and essentially forces a get. 
        /// Notifying the room of the item transfer is up to the executor of this method.
        /// </summary>
        /// <param name="item"></param>
        public void AcceptCash(Currency currency, long quantity)
        {
            Currency.Add(currency, Cash, quantity);

            //TODO: Tell player (buffs?) cash has been gotten.
        }

        /// <summary>
        /// Picks up cash from the floor. 
        /// </summary>
        /// <param name="currency"></param>
        /// <param name="quantity"></param>
        public void PickUpCash(Currency currency, long quantity)
        {
            var q = Room.GetTotalCashUserCanSee(currency, this);

            var visible = Math.Min(quantity, q.Visible);
            var hidden = quantity - visible;
            Currency.Add(currency, Room.Cash, -visible);
            Currency.Add(currency, Room.HiddenCash, -hidden);
            Room.ClearFoundHiddenCash(currency);

            //TODO: Tell room cash has been removed.

            AcceptCash(currency, quantity);
        }

        /// <summary>
        /// Determines if an actor can have an item removed from their inventory
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Validation CanRemoveItem(Item item)
        {
            // Make sure item is actually in the room.
            if (item.Actor != this)
                return new Validation().Fail("Item is not owned by the actor");

            // TODO: Ask item for a reason why it can't be dropped?
            if (item.ItemTemplate.NotDroppable)
                return new Validation().Fail("You cannot drop that!");

            //TODO: Ask item if it can be dropped.

            return new Validation();
        }

        /// <summary>
        /// Determines if an actor can drop an item in their current room
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Validation CanDropItem(Item item)
        {
            // TODO: Ask room if it will accept the item.
            return CanRemoveItem(item);
        }

        /// <summary>
        /// Removes an item. Beware that this performs no checks and essentially forces a drop. 
        /// </summary>
        /// <param name="item"></param>
        public void RemoveItem(Item item)
        {
            item.Actor = null;
            item.EquippedSlot = null;
        }

        /// <summary>
        /// Drops an item into the current room.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="hide"></param>
        public void DropItem(Item item, bool hide)
        {
            RemoveItem(item);

            item.Room = Room;
            if (hide)
            {
                item.HiddenTime = Game.GameTime.Ticks;
            }
            else
            {
                item.HiddenTime = null;
            }
        }

        /// <summary>
        /// Determines if cash can be removed from the actor.
        /// </summary>
        /// <param name="currency"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public Validation CanRemoveCash(Currency currency, long quantity)
        {
            //TODO: Ask buffs
            return new Validation();
        }

        /// <summary>
        /// Determines if the actor can drop cash into their room.
        /// </summary>
        /// <param name="currency"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public Validation CanDropCash(Currency currency, long quantity, bool hide)
        {
            // TODO: Ask room.
            return CanRemoveCash(currency, quantity);
        }

        /// <summary>
        /// Removes cash from the actor.
        /// </summary>
        /// <param name="currency"></param>
        /// <param name="quantity"></param>
        public long RemoveCash(Currency currency, long quantity)
        {
            var q = Currency.Get(currency, Cash);

            if (quantity > q)
                quantity = q;

            Currency.Add(currency, Cash, -quantity);
            return quantity;
        }

        /// <summary>
        /// Drops cash from the current user into their room.
        /// </summary>
        /// <param name="currency"></param>
        /// <param name="quantity"></param>
        /// <param name="hide"></param>
        public void DropCash(Currency currency, long quantity, bool hide)
        {
            var q = RemoveCash(currency, quantity);
            var container = Room.Cash;
            if (hide == true)
                container = Room.HiddenCash;
            Currency.Add(currency, container, quantity);

            if (hide == true)
                Room.ClearFoundHiddenCash(currency);
        }
    }
}
