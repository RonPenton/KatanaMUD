using System;

namespace KatanaMUD.Messages
{
    public class ItemOwnershipMessage : MessageBase
    {
        public ItemDescription Item { get; set; }

        public ActorDescription Giver { get; set; }

        public ActorDescription Taker { get; set; }
    }
}