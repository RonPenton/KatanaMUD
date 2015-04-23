using System;
using KatanaMUD.Models;
using System.Linq;
using System.Collections.Generic;

namespace KatanaMUD.Messages
{
    public class InventoryMessage : MessageBase
    {
        public override void Process(Actor actor)
        {
            var response = new InventoryListMessage();
            response.Items = actor.Items.Select(x => new ItemDescription(x)).ToArray();
            response.Encumbrance = actor.Encumbrance;
            response.MaxEncumbrance = actor.MaxEncumbrance;
            actor.SendMessage(response);
        }
    }

    public class InventoryListMessage : MessageBase
    {
        public ItemDescription[] Items { get; set; }

        public long Encumbrance { get; set; }
        public long MaxEncumbrance { get; set; }
    }


    public class GetItemMessage : MessageBase
    {
        public Guid? ItemId { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }

        public override void Process(Actor actor)
        {
            var item = FindItem(ItemId, ItemName, actor.Room.Items);

            if (item != null)
            {
                if (actor.CanGetItem(item))
                {
                    actor.GetItem(item);
                }
                //TODO: failure message.
            }
            else
            {
                //TODO: failure message.
            }
        }

        public static Item FindItem(Guid? itemId, string itemName, IEnumerable<Item> items)
        {
            Item item;
            //TODO: Only items that you can see/have found.
            if (itemId != null)
            {
                item = items.SingleOrDefault(x => x.Id == itemId.Value);
            }
            else
            {
                item = items.FindByName(itemName, x => x.Name, true, true);
            }
            return item;
        }
    }

    public class DropItemMessage : MessageBase
    {
        public Guid? ItemId { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }

        public override void Process(Actor actor)
        {
            var item = GetItemMessage.FindItem(ItemId, ItemName, actor.Items);

            if (item != null)
            {
                if (actor.CanDropItem(item))
                {
                    actor.DropItem(item);
                }
                //TODO: failure message.
            }
            else
            {
                //TODO: failure message.
            }
        }
    }
}