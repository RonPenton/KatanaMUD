using System;

namespace KatanaMUD.Models
{
    public partial class Item
    {
        public string Name => CustomName ?? ItemTemplate.Name;
    }
}