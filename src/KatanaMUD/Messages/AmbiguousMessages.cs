using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KatanaMUD.Messages
{
    public class AmbiguousItemMessage : MessageBase
    {
        public ItemDescription[] Items { get; set; }
    }

    public class AmbiguousActorMessage : MessageBase
    {
        public ActorDescription[] Actors { get; set; }
    }
}
