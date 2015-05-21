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
using KatanaMUD.Helpers;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis.Scripting.CSharp;
using C5;
using KatanaMUD.Events;

namespace KatanaMUD
{
    public class Game
    {
        public static ConnectionManager Connections { get; } = new ConnectionManager();

        public static GameEntities Data { get; private set; }

        public static ConcurrentSet<Actor> ActiveActors { get; } = new ConcurrentSet<Actor>();

        // TODO: figure out if we need to seed this better, or use a different algorithm altogether.
        public static Random Random { get; } = new Random();

        public static TimeSpan GameTime { get; private set; }

        private static IntervalHeap<GameEvent> _eventQueue = new IntervalHeap<GameEvent>();

        public static void AddEvent(GameEvent e) {
            IPriorityQueueHandle<GameEvent> handle = null;
            _eventQueue.Add(ref handle, e);
            e.Handle = handle;
        }

        public static void CancelEvent(GameEvent e)
        {
            _eventQueue.Delete(e.Handle);
            e.Handle = null;
        }

        public static void RescheduleEvent(GameEvent e, TimeSpan newTime)
        {
            CancelEvent(e);
            e.ExecutionTime = newTime;
            AddEvent(e);
        }

        async public static void Run()
        {
            Data = new GameEntities("Server=KATANAMUD\\SQLEXPRESS;Database=KatanaMUD;integrated security=True;");
            Data.LoadFromDatabase();

            var gameTime = Data.Settings.Find("GameTime");
            if (gameTime == null)
            {
                GameTime = new TimeSpan(0);
                gameTime = Data.Settings.New("GameTime");
                gameTime.Value = 0.ToString();
            }
            else
            {
                GameTime = new TimeSpan(long.Parse(gameTime.Value));
            }

            DateTime lastTime = DateTime.UtcNow;
            var saveTime = lastTime.AddMinutes(1);

            Console.WriteLine("KatanaMUD 0.2 Server Started");
            while (true)
            {
                try
                {
                    var newTime = DateTime.UtcNow;
                    var timeDifference = newTime.Subtract(lastTime);
                    GameTime = GameTime.Add(timeDifference);
                    gameTime.Value = GameTime.Ticks.ToString();

                    // handle connections/disconnections
                    Connections.HandleConnectsAndDisconnects();

                    // Handle all timed events.
                    while(_eventQueue.Count > 0 && _eventQueue.FindMin().ExecutionTime < GameTime)
                    {
                        var ev = _eventQueue.DeleteMin();
                        ev.Execute();
                    }

                    // Grab a snapshot of all active actors (will clear the active list)
                    var actors = ActiveActors.Snapshot();

                    // Extract the messages from the actors, and order them by time. 
                    var messages = actors.Select(x =>
                    {
                        bool remaining;
                        var message = x.GetNextMessage(out remaining);
                        return Tuple.Create(x, message, remaining);
                    }).OrderBy(x => x.Item2?.MessageTime).ToList();

                    // If any actors have more messages, re-add them to the active list.
                    // we only grab the top message from each actor because each action may have a time component to it,
                    // so that if the action causes the user to pause, we cannot process the second message yet. 
                    // We'll grab the next message on the next game loop. It's not the best system, but it's
                    // workable, and it serves to effectively limit messages to 1 per loop, or about 10-20 per second
                    // depending on how long the loop sleep period is. I don't believe this will be a problem in practice,
                    // though I suppose there's always the possibility of a timing exploit somewhere. 
                    foreach (var message in messages.Where(x => x.Item3 == true))
                    {
                        ActiveActors.Add(message.Item1);
                    }

                    foreach (var message in messages.Where(x => x.Item2 != null))
                    {
                        message.Item2.Process(message.Item1);
                    }

                    //TODO: This could really be done in parallel, or even on separate threads. 
                    // I figure this should be addressed sooner rather than later, since once combat is enabled, output messages could get 
                    // laggy if we simply perform awaits on each one instead of letting the hardware do its thing and go on to the next. 
                    foreach(var connection in Connections.GetConnections())
                    {
                        var handler = connection?.Actor?.MessageHandler as ConnectionMessageHandler;
                        if (handler != null)
                            await handler.SendMessages();
                    }


                    lastTime = newTime;

                    // Save changes to the database.
                    if (Data.ForceSave || saveTime < newTime)
                    {
                        Data.ForceSave = false;
                        // TODO: Make the save time configurable eventually.
                        saveTime = newTime.AddMinutes(1);
                        Data.SaveChanges();
                    }

                    Thread.Sleep(50);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(String.Format("[{0}] Exception: {1}", DateTime.Now.ToShortTimeString(), ex.ToString()));
                }
            }
        }
    }
}