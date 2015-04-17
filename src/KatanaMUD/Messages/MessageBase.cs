using System;
using KatanaMUD.Models;
using Newtonsoft.Json;

namespace KatanaMUD.Messages
{
    public abstract class MessageBase
    {
		public virtual string MessageName => this.GetType().Name;

		[JsonIgnore]
		public DateTime MessageTime { get; } = DateTime.UtcNow;

		public virtual void Process(Actor actor) { }
	}
}