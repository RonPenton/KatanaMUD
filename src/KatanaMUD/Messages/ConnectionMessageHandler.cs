using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using KatanaMUD.Models;

namespace KatanaMUD.Messages
{
    public class ConnectionMessageHandler : IMessageHandler
    {
        private Actor _actor;

        public ConnectionMessageHandler(Actor actor)
        {
            this._actor = actor;
        }

        public void HandleMessage(MessageBase message)
        {
            HandleMessage(_actor.Connection.Socket, message);
        }

        public static async void HandleMessage(WebSocket socket, MessageBase message)
        {
            var encoded = Encoding.UTF8.GetBytes(MessageSerializer.SerializeMessage(message));
            await socket.SendAsync(new ArraySegment<byte>(encoded, 0, encoded.Length), WebSocketMessageType.Text, true, CancellationToken.None);
        }

    }
}