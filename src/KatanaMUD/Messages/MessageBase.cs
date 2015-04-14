using System;
using Newtonsoft.Json;

namespace KatanaMUD.Messages
{
    public abstract class MessageBase
    {
		public virtual string MessageName => this.GetType().Name;

		[JsonIgnore]
		public DateTime MessageTime { get; set; }

		public virtual void Process(Connection connection) { }
	}
}