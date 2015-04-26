using System;

namespace KatanaMUD.Messages
{
    public class GenericMessage : MessageBase
    {
        public string Message { get; set; }

        public string Class { get; set; }
    }
}