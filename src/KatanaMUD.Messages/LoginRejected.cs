using System;

namespace KatanaMUD.Messages
{
    public class LoginRejected : MessageBase
    {
        public string RejectionMessage { get; set; }
        public bool NoCharacter { get; set; }
	}
}