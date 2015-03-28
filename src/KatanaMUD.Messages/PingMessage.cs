using System;

namespace KatanaMUD.Messages
{
    public class PingMessage : MessageBase
    {
        public DateTime SendTime { get; set; }
    }

    public class PongMessage : MessageBase
    {
        public DateTime SendTime { get; set; }
    }
}