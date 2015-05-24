using KatanaMUD.Messages;
using KatanaMUD.Models;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace KatanaMUD
{
    public class Connection
    {
        public Connection(WebSocket socket, User user, Actor actor, string ip)
        {
            Socket = socket;
            User = user;
            Actor = actor;
            IP = ip;
        }

        public WebSocket Socket { get; private set; }
        public User User { get; private set; }
        public Actor Actor { get; private set; }
        public string IP { get; private set; }
        public bool Disconnected { get; internal set; }

        internal DateTime FloodStart { get; set; }
        internal int FloodCount { get; set; }


        public async Task HandleSocketCommunication()
        {
            try
            {
                byte[] buffer = new byte[1024];
                WebSocketReceiveResult received = await Socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                while (!Socket.CloseStatus.HasValue && !Disconnected)
                {
                    var message = MessageSerializer.DeserializeMessage(Encoding.UTF8.GetString(buffer, 0, received.Count));
                    Actor.AddMessage(message);

                    var now = DateTime.UtcNow;
                    if (FloodStart < now.AddMinutes(-1))
                    {
                        FloodStart = now;
                        FloodCount = 1;
                    }
                    else
                    {
                        FloodCount++;
                    }

                    // TODO: Configurable
                    if (FloodCount > 25)
                    {
                        Game.Connections.Disconnect(this.User);
                        await Socket.CloseAsync(WebSocketCloseStatus.PolicyViolation, "Flooding violation", CancellationToken.None);
                        return;
                    }

                    received = await Socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                }

                Game.Connections.Disconnected(this);

                if (Socket.State != WebSocketState.Closed)
                {
                    await Socket.CloseAsync(Socket.CloseStatus.Value, Socket.CloseStatusDescription, CancellationToken.None);
                }
            }
            catch (Exception)
            {
                Game.Connections.Disconnected(this);
            }
        }
    }
}