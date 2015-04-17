using System;
using System.Collections.Concurrent;
using KatanaMUD.Messages;
using System.Linq;

namespace KatanaMUD.Models
{
	public partial class Actor
	{
		internal Connection Connection { get; set; }

		public IMessageHandler MessageHandler { get; set; }

		public void SendMessage(MessageBase message)
		{
			if (MessageHandler != null)
				MessageHandler.HandleMessage(message);
		}

		private ConcurrentQueue<MessageBase> _messages { get; } = new ConcurrentQueue<MessageBase>();

		internal void AddMessage(MessageBase message)
		{
			_messages.Enqueue(message);
			Game.ActiveActors.Add(this);
		}

		internal MessageBase GetNextMessage(out bool remaining)
		{
			MessageBase result;
			_messages.TryDequeue(out result);
			remaining = _messages.Count > 0;
			return result;
		}


        /// <summary>
        /// Call this when a user is to change rooms, independent of party. 
        /// </summary>
        /// <param name="room"></param>
        /// <param name="exitRoom"></param>
        public void ChangeRooms(Room room, int? exitRoom)
        {
            Room newRoom = Game.Data.Rooms.Single(x => x.Id == exitRoom);

            // TODO: notify old room of user exiting. 
            Room = newRoom;
            // TODO: notify new room of user entering.
        }
    }
}