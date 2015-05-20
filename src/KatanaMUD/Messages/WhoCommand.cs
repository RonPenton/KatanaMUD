using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KatanaMUD.Models;

namespace KatanaMUD.Messages
{
    public class WhoCommand : MessageBase
    {
        public override void Process(Actor actor)
        {
            var actors = Game.Connections.GetConnections().Select(x => new ActorDescription(x.Actor));
            actor.SendMessage(new WhoMessage() { Actors = actors.ToArray() });
        }
    }

    public class WhoMessage : MessageBase
    {
        public ActorDescription[] Actors { get; set; }
    }
}
