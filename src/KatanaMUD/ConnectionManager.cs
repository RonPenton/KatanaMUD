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
                Console.WriteLine(String.Format("User ({0}/{1}/{2}) Disconnected", connection.User.Id, connection.Actor.Name, connection.IP));
                _connections.Remove(user);
                _pendingDisconnections.Add(connection);
            }
        }

        internal void Disconnected(Connection connection)
        {
            Console.WriteLine(String.Format("User ({0}/{1}/{2}) Disconnected", connection.User.Id, connection.Actor.Name, connection.IP));
            connection.Actor.UnhandledDisconnection = true;
            lock (syncRoot)
            {
                _connections.Remove(connection.User);
                _disconnections.Add(connection);
            }
        }

        internal Connection Connect(WebSocket socket, User user, Actor actor, string ip)
        {
            Console.WriteLine(String.Format("User ({0}/{1}/{2}) Connected", user.Id, actor.Name, ip));
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

                    //TODO: Notify realm of disconnection.
                }
                _disconnections.Clear();

                foreach (var connection in _pendingDisconnections)
                {
                    connection.Actor.Connection = null;
                    connection.Actor.MessageHandler = null;
                    connection.Socket.CloseAsync(WebSocketCloseStatus.PolicyViolation, "Disconnected", CancellationToken.None);

                    //TODO: Notify realm of disconnection.
                }
                _pendingDisconnections.Clear();

                foreach (var connection in _newConnections)
                {
                    _connections[connection.User] = connection;

                    connection.Actor.MessageHandler = new ConnectionMessageHandler(connection.Actor);
                    connection.Actor.Connection = connection;

                    //TODO: Load Server message
                    connection.Actor.SendMessage(new ServerMessage() { Contents = "Welcome to KatanaMUD. A MUD on the Web. Because I'm apparently insane. Dear lord." });
                    connection.Actor.SendRoomDescription(connection.Actor.Room);
                }
                _newConnections.Clear();
            }
        }
    }
}