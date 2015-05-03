using System;

namespace KatanaMUD.Messages
{
    public class ItemOwnershipMessage : MessageBase
    {
        public ItemDescription[] Items { get; set; }
        public ActorDescription Giver { get; set; }
        public ActorDescription Taker { get; set; }
        public bool Hide { get; set; }
    }

    public class CashTransferMessage : MessageBase
    {
        public CurrencyDescription Currency { get; set; }
        public long Quantity { get; set; }
        public ActorDescription Giver { get; set; }
        public ActorDescription Taker { get; set; }
        public bool Hide { get; set; }
    }
}