using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Spam
{
    /// <summary>
    /// Entity Context represents a Simple Persistent Access Manager (or SPAM). It is meant to be a lightweight 
    /// and baremetal context designed to work with SQL to retrieve and persist objects in memory for game-like 
    /// implementations. These objects should be periodically written back out to disk as the game progresses, 
    /// to handle events like a server crash or power outage. 
    /// </summary>
    public abstract class EntityContext : IDisposable
    {
        private string _connectionString;

        private SqlConnection _connection;

        public EntityContext(string connectionString)
        {
            _connectionString = connectionString;
            _connection = new SqlConnection(_connectionString);
        }

		protected abstract void LoadAllData();

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if(_connection != null)
                    {
                        _connection.Close();
                        _connection = null;
                    }
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
