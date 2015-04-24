using System;

namespace KatanaMUD.Messages
{
    public class LoginStateMessage : MessageBase
    {
        public ActorDescription Actor { get; set; }

        public bool Login { get; set; }
    }
}