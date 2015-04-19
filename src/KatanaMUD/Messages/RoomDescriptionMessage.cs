using System;
using KatanaMUD.Models;

namespace KatanaMUD.Messages
{
    public class RoomDescriptionMessage : MessageBase
    {
        public int? RoomId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int[] Actors { get; set; }
        public int[] VisibleItems { get; set; }
        public int?[] Exits { get; set; }
        public bool IsCurrentRoom { get; set; }
        /// <summary>
        /// Used to determine if the user is unable to see their surroundings.
        /// </summary>
        public bool CannotSee { get; set; }
        /// <summary>
        /// If the user is unable to see their surroundings, RoomId/Name/Description/Actors/VisibleItems will all be
        /// blank, and the CannotSeeMessage should be shown instead.
        /// </summary>
        public string CannotSeeMessage { get; set; }

		internal void SetExits(Room room)
		{
			Exits = new int?[10];
			foreach (var direction in Directions.Enumerate())
			{
				Exits[(int)direction] = room.GetExit(direction)?.ExitRoom;
			}
		}
	}
}