using System;
using System.Data.SqlClient;

namespace Spam
{
    public abstract class Entity<K>
    {
        public abstract K Key { get; set; }

        public abstract SqlCommand GetAllCommand();


    }
}