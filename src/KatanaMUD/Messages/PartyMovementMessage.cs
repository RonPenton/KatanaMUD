using System;
using KatanaMUD.Models;

namespace KatanaMUD.Messages
{
    public class PartyMovementMessage : MessageBase
    {
        public ActorDescription Leader { get; set; }

        public ActorDescription[] Actors { get; set; }

        public Direction? Direction { get; set; }

        public bool Enter { get; set; }

        public string CustomText { get; set; }
    }
}