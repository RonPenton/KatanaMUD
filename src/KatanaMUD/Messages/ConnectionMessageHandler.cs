using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using KatanaMUD.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KatanaMUD.Messages
{
    public class ConnectionMessageHandler : IMessageHandler
    {
        private Actor _actor;
        Queue<MessageBase> _outputMessages = new Queue<MessageBase>();

        public ConnectionMessageHandler(Actor actor)
        {
            this._actor = actor;
        }

        public void HandleMessage(MessageBase message)
        {
            _outputMessages.Enqueue(message);
        }

        public async Task<bool> SendMessages()
        {
            while (_outputMessages.Count > 0)
            {
                var message = _outputMessages.Dequeue();
                var success = await HandleMessage(_actor.Connection.Socket, message);
            }

            return true;
        }

        public static async Task<bool> HandleMessage(WebSocket socket, MessageBase message)
        {
            try {
                if (socket.State == WebSocketState.Open)
                {
                    var encoded = Encoding.UTF8.GetBytes(MessageSerializer.SerializeMessage(message));
                    await socket.SendAsync(new ArraySegment<byte>(encoded, 0, encoded.Length), WebSocketMessageType.Text, true, CancellationToken.None);
                    return true;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                //TODO: LOG
            }
            return false;
        }

    }
}