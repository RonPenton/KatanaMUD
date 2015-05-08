using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KatanaMUD.Models;

namespace KatanaMUD.Messages
{
    public class EquipMessage : MessageBase
    {
        public Guid? ItemId { get; set; }
        public string ItemName { get; set; }

        public override void Process(Actor actor)
        {
            if (ItemId == null)
            {
                var items = actor.Items.FindByName(ItemName, x => x.Name, true, true);
            }

            Item item;


        }

    }
}
