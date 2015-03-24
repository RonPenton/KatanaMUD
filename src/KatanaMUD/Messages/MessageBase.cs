using System;

namespace KatanaMUD.Messages
{
    public abstract class MessageBase
    {
		public virtual string Name => this.GetType().Name;
    }
}