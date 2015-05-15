using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KatanaMUD.Models;

namespace KatanaMUD.Messages
{
    public class RemoveCommand : MessageBase
    {
        public Guid? ItemId { get; set; }
        public string ItemName { get; set; }

        public override void Process(Actor actor)
        {
            Item item;
            if (ItemId == null)
            {
                item = actor.Items.Where(x => x.EquippedSlot != null).FindByName(ItemName, x => x.Name, true, true).FirstOrDefault();
            }
            else
            {
                item = actor.Items.Where(x => x.EquippedSlot != null).FirstOrDefault(x => x.Id == ItemId.Value);
            }

            if (item == null)
            {
                var name = ItemName ?? "that";
                actor.SendMessage(new ActionNotAllowedMessage() { Message = "You do not have " + name + "  equipped." });
                return;
            }

            var isvalid = actor.CanRemoveItem(item);
            if (!isvalid.Allowed)
            {
                actor.SendMessage(new ActionNotAllowedMessage() { Message = isvalid.FirstPerson });
                return;
            }

            actor.UnEquip(item);
            var message = new ItemEquippedChangedMessage() { Actor = new ActorDescription(actor), Item = new ItemDescription(item), Equipped = false };
            actor.Room.ActiveActors.ForEach(x => x.SendMessage(message));
        }
    }

    public class EquipCommand : MessageBase
    {
        public Guid? ItemId { get; set; }
        public string ItemName { get; set; }

        public override void Process(Actor actor)
        {
            Item item;
            if (ItemId == null)
            {
                item = actor.Items.Where(x => x.EquippedSlot == null).FindByName(ItemName, x => x.Name, true, true).FirstOrDefault();
            }
            else
            {
                item = actor.Items.Where(x => x.EquippedSlot == null).FirstOrDefault(x => x.Id == ItemId.Value);
            }

            if (item == null)
            {
                var name = ItemName ?? "that";
                actor.SendMessage(new ActionNotAllowedMessage() { Message = "You do not have " + name + "  left unequipped." });
                return;
            }

            var open = actor.IsSlotOpen(item);

            if (!open.IsOpen && !String.IsNullOrEmpty(open.FailureReason))
            {
                actor.SendMessage(new ActionNotAllowedMessage() { Message = open.FailureReason });
                return;
            }

            if (!open.IsOpen && open.ItemsToRemove.Any())
            {
                // Slot can be opened, if an item is removed. So try to remove all the items blocking the slot.
                foreach (var remove in open.ItemsToRemove)
                {
                    var isvalid = actor.CanRemoveItem(remove);
                    if (!isvalid.Allowed)
                    {
                        actor.SendMessage(new ActionNotAllowedMessage() { Message = isvalid.FirstPerson });
                        return;
                    }
                }

                foreach (var remove in open.ItemsToRemove)
                {
                    actor.UnEquip(remove);
                    var remMessage = new ItemEquippedChangedMessage() { Actor = new ActorDescription(actor), Item = new ItemDescription(remove), Equipped = false };
                    actor.Room.ActiveActors.ForEach(x => x.SendMessage(remMessage));
                }
            }

            var valid = actor.CanEquipItem(item);
            if (!valid.Allowed)
            {
                actor.SendMessage(new ActionNotAllowedMessage() { Message = valid.FirstPerson });
                return;
            }

            actor.EquipItem(item);
            var message = new ItemEquippedChangedMessage() { Actor = new ActorDescription(actor), Item = new ItemDescription(item), Equipped = true };
            actor.Room.ActiveActors.ForEach(x => x.SendMessage(message));
        }
    }

    public class ItemEquippedChangedMessage : MessageBase
    {
        public ActorDescription Actor { get; set; }
        public ItemDescription Item { get; set; }
        public bool Equipped { get; set; }
    }
}
