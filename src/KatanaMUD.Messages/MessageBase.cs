using System;

namespace KatanaMUD.Messages
{
    public abstract class MessageBase
    {
		public virtual string MessageName => this.GetType().Name;
    }
}