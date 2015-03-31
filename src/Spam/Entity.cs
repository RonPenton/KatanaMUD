using System;
using System.Data.SqlClient;

namespace Spam
{
    public abstract class Entity<K>
    {
        public abstract K Key { get; set; }

		internal bool IsChanged { get; set; }

		internal IChangeNotifier<K> Container { get; set; }

		protected void Changed()
		{
			this.IsChanged = true;
			if (this.Container != null)
				this.Container.SetChanged(this);
		}
    }
}