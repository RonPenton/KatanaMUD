using System;

namespace KatanaMUD.Messages
{
    public class LoginRejected : MessageBase
    {
        public string RejectionMessage { get; set; }
	}

    public enum TestEnum
    {
        OK,
        Noo = 4,
        Test = 6
    }
}