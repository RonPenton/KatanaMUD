using System;
using System.Data.SqlClient;

namespace Spam
{
    public abstract class Entity<K> : IEntity, IEntity<K>
    {
        public abstract K Key { get; set; }

        internal IEntityContainer<K> Container { get; set; }

        public bool IsChanged => Container?.IsChanged(this) ?? false;

        public bool IsDeleted => Container?.IsDeleted(this) ?? false;

        public bool IsNew => Container?.IsNew(this) ?? false;

        public void Changed()
		{
			if (this.Container != null)
				this.Container.SetChanged(this);
		}

        protected void ChangeParent<P, T>(P value, ref P field, Action<P, T> removeChild, Action<P, T> addChild) where P : class where T : Entity<K>
        {
            // Don't do anything if the value is already what is being set.
            if (field == value)
                return;

            if (field != null)
            {
                // An existing parent is set. We need to remove the child
                // from the existing parents container.
                // set to null because the Remove action will loop back around and try to set 
                // the parent again to null, which we don't want.
                var temp = field;
                field = null;
                //temp.Actors.Remove(this);
                removeChild(temp, (T)this);
            }

            field = value;
            Changed();

            if (value != null)
            {
                // Lastly, add the new value to the parents child-list if it's not null.
                //value.Actors.Add(this);
                addChild(value, (T)this);
            }
        }
    }
}