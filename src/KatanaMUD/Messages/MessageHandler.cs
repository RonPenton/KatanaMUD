using System;

namespace KatanaMUD.Messages
{
    public interface IMessageHandler
    {
        void HandleMessage(MessageBase message);
    }
}