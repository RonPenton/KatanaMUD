using KatanaMUD.Messages;
using System.Threading.Tasks;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using KatanaMUD.Models.Test;

namespace KatanaMUD
{
    public class Game
    {
        public static ConcurrentQueue<MessageBase> Messages = new ConcurrentQueue<MessageBase>();
        public static List<WebSocket> Sockets = new List<WebSocket>();

        async public static void Run()
        {
            var c = new GameEntities("Server=.\\MSSQL10.MSSQLSERVER;Database=KatanaMUD;integrated security=True");

            DateTime lastTime = DateTime.UtcNow;
            var pingTime = lastTime;

            try
            {
                Console.WriteLine("KatanaMUD 0.1 Server Started");
                while (true)
                {
                    var newTime = DateTime.UtcNow;

                    MessageBase message;
                    if (Messages.TryDequeue(out message))
                    {
                        if(message is PingMessage)
                        {
                            var ping = new PongMessage() { SendTime = ((PingMessage)message).SendTime };
                            var pingstring = MessageSerializer.SerializeMessage(ping);
                            var bytes = Encoding.UTF8.GetBytes(pingstring);
                            var segment = new ArraySegment<byte>(bytes);
                            foreach (var socket in Sockets)
                            {
                                await socket.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
                            }
                        }
                    }


                    //if(newTime - pingTime > TimeSpan.FromSeconds(3))
                    //{
                    //    var ping = new PingMessage() { SendTime = newTime };
                    //    var pingstring = MessageSerializer.SerializeMessage(ping);
                    //    var bytes = Encoding.UTF8.GetBytes(pingstring);
                    //    var segment = new ArraySegment<byte>(bytes);
                    //    foreach (var socket in Sockets)
                    //    {
                    //        await socket.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
                    //    }
                    //    pingTime = newTime;
                    //}

                    lastTime = newTime;


                    Thread.Sleep(10);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }

        }
    }
}