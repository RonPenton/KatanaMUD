using KatanaMUD.Messages;
using KatanaMUD.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace KatanaMUD
{
    public class ConnectionManager
    {
        object syncRoot = new object();

        Dictionary<User, Connection> _connections = new Dictionary<User, Connection>();

        public bool IsLoggedIn(User user)
        {
            lock(syncRoot)
            {
                return _connections.ContainsKey(user);
            }
        }

        public void Add(Connection connection)
        {
            if (_connections.ContainsKey(connection.User))
                throw new InvalidOperationException("User already connected");
            lock(syncRoot)
            {
                _connections[connection.User] = connection;
            }
        }

        public void Remove(Connection connection)
        {
            connection.Actor.MessageHandler = null;
            connection.Actor.Connection = null;
            lock(syncRoot)
            {
                _connections.Remove(connection.User);
            }
        }

		public IEnumerable<Connection> GetConnections()
		{
			lock(syncRoot)
			{
				return _connections.Values.ToList();
			}
		}
    }
}