using System;
using KatanaMUD.Models;

namespace KatanaMUD.Messages
{
    public class PingMessage : MessageBase
    {
        public DateTime SendTime { get; set; }

        public override void Process(Actor actor)
        {
            var pong = new PongMessage() { SendTime = SendTime };
            actor.SendMessage(pong);
        }
    }

    public class PongMessage : MessageBase
    {
        public DateTime SendTime { get; set; }
    }
}