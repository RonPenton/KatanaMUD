using System;
using System.Collections.Concurrent;
using System.Threading;

namespace KatanaMUD
{
    public class GameLoop
    {
        public static ConcurrentQueue<string> Messages = new ConcurrentQueue<string>();

        public static void Run()
        {
            try {
                Console.WriteLine("Server Started");
                while (true)
                {
                    string message;
                    if (Messages.TryDequeue(out message))
                    {
                        Console.WriteLine(message);
                    }

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