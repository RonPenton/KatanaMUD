using KatanaMUD;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Spam
{
    public class LinkEntityContainer<E1, E2, K1, K2> : IEnumerable<Tuple<K1, K2>>, ILinkEntityContainer where E1 : Entity<K1> where E2 : Entity<K2>
    {
        HashSet<Tuple<K1, K2>> _storage = new HashSet<Tuple<K1, K2>>(new LinkEntityCompare<K1, K2>());
        HashSet<Tuple<K1, K2>> _new = new HashSet<Tuple<K1, K2>>(new LinkEntityCompare<K1, K2>());
        HashSet<Tuple<K1, K2>> _deleted = new HashSet<Tuple<K1, K2>>(new LinkEntityCompare<K1, K2>());
        string _tableName;
        string _firstKeyName;
        string _secondKeyName;

        public LinkEntityContainer(string tableName, string firstKeyName, string secondKeyName)
        {
            _tableName = tableName;
            _firstKeyName = firstKeyName;
            _secondKeyName = secondKeyName;
        }

        public int Count => _storage.Count;

        public bool IsReadOnly => false;

        public bool Contains(Tuple<K1, K2> item) => _storage.Contains(item);

        public IEnumerator<Tuple<K1, K2>> GetEnumerator() => _storage.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _storage.GetEnumerator();

        public bool IsNew(Tuple<K1, K2> entity) => this._new.Contains(entity);

        public bool IsDeleted(Tuple<K1, K2> entity) => this._deleted.Contains(entity);

        public void ClearChanges()
        {
            _new.Clear();
            _deleted.Clear();
        }

        public void Link(K1 key1, K2 key2, bool fromLoad)
        {
            var key = new Tuple<K1, K2>(key1, key2);
            if (!fromLoad)
                _new.Add(key);
            _storage.Add(key);
        }

        public void Unlink(K1 key1, K2 key2)
        {
            var key = new Tuple<K1, K2>(key1, key2);

            bool removed = _storage.Remove(key);
            if (removed && !_new.Remove(key))
            {
                _deleted.Add(key);
            }
        }

        public void Load(SqlConnection connection)
        {
            var command = new SqlCommand(String.Format("SELECT [{0}], [{1}] from {2}", _firstKeyName, _secondKeyName, _tableName), connection);
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    K1 k1 = reader.GetFieldValue<K1>(0);
                    K2 k2 = reader.GetFieldValue<K2>(1);
                    Link(k1, k2, true);
                }
            }
        }

        public void InsertEntities(SqlCommand command)
        {
            foreach (var link in this._new)
            {
                command.CommandText = String.Format("INSERT INTO [{0}] ([{1}], [{2}]) VALUES (@First, @Second)", _tableName, _firstKeyName, _secondKeyName);
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@First", link.Item1);
                command.Parameters.AddWithValue("@Second", link.Item2);
                command.ExecuteNonQuery();
            }
        }

        public void DeleteEntities(SqlCommand command)
        {
            foreach (var link in this._deleted)
            {
                command.CommandText = String.Format(@"DELETE FROM [{0}] WHERE [{1}] = @First and [{2}] = @Second", _tableName, _firstKeyName, _secondKeyName);
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@First", link.Item1);
                command.Parameters.AddWithValue("@Second", link.Item2);
                command.ExecuteNonQuery();
            }
        }
    }

    public interface ILinkEntityContainer
    {
        void Load(SqlConnection connection);
        void InsertEntities(SqlCommand command);
        void DeleteEntities(SqlCommand command);
        void ClearChanges();
    }
}