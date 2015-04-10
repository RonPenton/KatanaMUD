using KatanaMUD.Messages;
using System.Threading.Tasks;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Linq;
using KatanaMUD.Models;

namespace KatanaMUD
{
    public class Game
    {
        public static ConnectionManager Connections { get; } = new ConnectionManager();
        public static ConcurrentQueue<Tuple<Connection, MessageBase>> MessageQueue { get; } = new ConcurrentQueue<Tuple<Connection, MessageBase>>();

        public static GameEntities Data { get; private set; }

        async public static void Run()
        {
            Data = new GameEntities("Server=localhost;Database=KatanaMUD;integrated security=True;");
            Data.LoadFromDatabase();

            DateTime lastTime = DateTime.UtcNow;
            var pingTime = lastTime;

            try
            {
                Console.WriteLine("KatanaMUD 0.1 Server Started");
                while (true)
                {
                    var newTime = DateTime.UtcNow;

                    Tuple<Connection, MessageBase> message;
                    if (MessageQueue.TryDequeue(out message))
                    {
                        if(message.Item2 is PingMessage)
                        {
                            var pong = new PongMessage() { SendTime = ((PingMessage)message.Item2).SendTime };
                            message.Item1.SendMessage(pong);
                        }
                    }

                    lastTime = newTime;
                    Thread.Sleep(100);
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