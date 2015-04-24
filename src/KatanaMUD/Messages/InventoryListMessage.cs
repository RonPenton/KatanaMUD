using System;
using KatanaMUD.Models;
using System.Linq;
using System.Collections.Generic;

namespace KatanaMUD.Messages
{
    public class InventoryCommand : MessageBase
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


    public class GetItemCommand : MessageBase
    {
        public Guid? ItemId { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }

        public override void Process(Actor actor)
        {
            var item = FindItem(ItemId, ItemName, actor.Room.Items);

            if (item != null)
            {
                var action = actor.CanGetItem(item);

                if (action.Allowed)
                {
                    actor.GetItem(item);

                    var message = new ItemOwnershipMessage()
                    {
                        Taker = new ActorDescription(actor),
                        Item = new ItemDescription(item)
                    };
                    actor.Room.ActiveActors.ForEach(x => x.SendMessage(message));
                }
                else
                {
                    actor.SendMessage(new ActionNotAllowedMessage() { Message = action.Reason });
                }
            }
            else
            {
                actor.SendMessage(new ActionNotAllowedMessage() { Message = "Cannot find item!" });
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

    public class DropItemCommand : MessageBase
    {
        public Guid? ItemId { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }

        public override void Process(Actor actor)
        {
            var item = GetItemCommand.FindItem(ItemId, ItemName, actor.Items);

            if (item != null)
            {
                if (actor.CanDropItem(item))
                {
                    actor.DropItem(item);

                    var message = new ItemOwnershipMessage()
                    {
                        Giver = new ActorDescription(actor),
                        Item = new ItemDescription(item)
                    };
                    actor.Room.ActiveActors.ForEach(x => x.SendMessage(message));
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