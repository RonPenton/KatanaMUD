using KatanaMUD.Messages;
using KatanaMUD.Models;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;

namespace KatanaMUD
{
    public class Connection
    {
        public Connection(WebSocket socket, User user, Actor actor)
        {
            if (actor.Connection != null)
                throw new InvalidOperationException("Connection already attached to actor");

            Socket = socket;
            User = user;
            Actor = actor;
            actor.Connection = this;
        }

        public void SendMessage(MessageBase message)
        {
            SendMessage(this.Socket, message);
        }

        public WebSocket Socket { get; private set; }
        public User User { get; private set; }
        public Actor Actor { get; private set; }


        public async static void SendMessage(WebSocket socket, MessageBase message)
        {
            var encoded = Encoding.UTF8.GetBytes(MessageSerializer.SerializeMessage(message));
            await socket.SendAsync(new ArraySegment<byte>(encoded, 0, encoded.Length), WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
}