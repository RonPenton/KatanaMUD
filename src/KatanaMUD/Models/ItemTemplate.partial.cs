using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KatanaMUD.Models
{
    public partial class ItemTemplate
    {
        public Item SpawnInstance()
        {
            var item = Game.Data.Items.New();
            item.ItemTemplate = this;
            return item;
        }

        public EquipmentSlot? EquipmentSlot => EquipType == null ? null : (EquipmentSlot?)EquipType;
    }
}
