using KatanaMUD.Messages;
using KatanaMUD.Models;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Collections.Concurrent;

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
    }
}