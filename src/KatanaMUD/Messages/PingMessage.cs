using System;

namespace KatanaMUD.Messages
{
    public class PingMessage : MessageBase
    {
        public DateTime SendTime { get; set; }

		public override void Process(Connection connection)
		{
			var pong = new PongMessage() { SendTime = SendTime };
			connection.SendMessage(pong);
		}
	}

    public class PongMessage : MessageBase
    {
        public DateTime SendTime { get; set; }
    }
}