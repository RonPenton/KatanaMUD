using KatanaMUD.Messages;
using KatanaMUD.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;

namespace KatanaMUD
{
    public class ConnectionManager
    {
        object syncRoot = new object();

        Dictionary<User, Connection> _connections = new Dictionary<User, Connection>();
        List<Connection> _disconnections = new List<Connection>();          // Disconnections from the connection itself
        List<Connection> _pendingDisconnections = new List<Connection>();   // Commanded disconnections from the system (kicking, etc)
        List<Connection> _newConnections = new List<Connection>();

        public bool IsLoggedIn(User user)
        {
            lock (syncRoot)
            {
                return _connections.ContainsKey(user);
            }
        }

        public IEnumerable<Connection> GetConnections()
        {
            lock (syncRoot)
            {
                return _connections.Values.ToList();
            }
        }

        internal void Disconnect(User user)
        {
            lock (syncRoot)
            {
                var connection = _connections[user];
                if (connection.Disconnected)
                    return;
                Console.WriteLine(String.Format("[{3}] User ({0}/{1}/{2}) Disconnected", connection.User.Id, connection.Actor.Name, connection.IP, DateTime.Now.ToShortTimeString()));
                _connections.Remove(user);
                _pendingDisconnections.Add(connection);
            }
        }

        internal void Disconnected(Connection connection)
        {
            if (connection.Disconnected)
                return;
            Console.WriteLine(String.Format("[{3}] User ({0}/{1}/{2}) Disconnected", connection.User.Id, connection.Actor.Name, connection.IP, DateTime.Now.ToShortTimeString()));
            connection.Actor.UnhandledDisconnection = true;
            lock (syncRoot)
            {
                _connections.Remove(connection.User);
                _disconnections.Add(connection);
            }
        }

        internal Connection Connect(WebSocket socket, User user, Actor actor, string ip)
        {
            Console.WriteLine(String.Format("[{3}] User ({0}/{1}/{2}) Connected", user.Id, actor.Name, ip, DateTime.Now.ToShortTimeString()));
            var connection = new Connection(socket, user, actor, ip);
            lock (syncRoot)
            {
                _newConnections.Add(connection);
            }
            return connection;
        }

        internal void HandleConnectsAndDisconnects()
        {
            lock (syncRoot)
            {
                foreach (var connection in _disconnections)
                {
                    connection.Actor.Connection = null;
                    connection.Actor.MessageHandler = null;
                    connection.Actor.UnhandledDisconnection = false;
                    connection.Disconnected = true;

                    // Notify the realm.
                    var message = new LoginStateMessage() { Actor = new ActorDescription(connection.Actor), Login = false };
                    _connections.Values.ForEach(x => x.Actor.SendMessage(message));
                }
                _disconnections.Clear();

                foreach (var connection in _pendingDisconnections)
                {
                    connection.Actor.Connection = null;
                    connection.Actor.MessageHandler = null;
                    connection.Disconnected = true;
                    connection.Socket.CloseAsync(WebSocketCloseStatus.PolicyViolation, "Disconnected", CancellationToken.None);

                    // Notify the realm.
                    var message = new LoginStateMessage() { Actor = new ActorDescription(connection.Actor), Login = false };
                    _connections.Values.ForEach(x => x.Actor.SendMessage(message));
                }
                _pendingDisconnections.Clear();

                foreach (var connection in _newConnections)
                {
                    // Notify the realm.
                    var message = new LoginStateMessage() { Actor = new ActorDescription(connection.Actor), Login = true };
                    _connections.Values.ForEach(x => x.Actor.SendMessage(message));

                    _connections[connection.User] = connection;

                    connection.Actor.MessageHandler = new ConnectionMessageHandler(connection.Actor);
                    connection.Actor.Connection = connection;

                    //TODO: Load Server message

                    connection.Actor.SendMessage(new ServerMessage() { Contents = "Auto-sensing... Just kidding!" });
                    connection.Actor.SendMessage(new ServerMessage() { Contents = "Welcome to KatanaMUD. A MUD on the Web. Because I'm apparently insane. Dear lord." });
                    connection.Actor.SendMessage(ActorInformationMessage.CreateFirstPerson(connection.Actor));
                    connection.Actor.SendRoomDescription(connection.Actor.Room);
                }
                _newConnections.Clear();
            }
        }
    }
}