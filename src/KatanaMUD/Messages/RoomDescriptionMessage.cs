using System;
using KatanaMUD.Models;
using System.Linq;

namespace KatanaMUD.Messages
{
    public class RoomDescriptionMessage : MessageBase
    {
        public int? RoomId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ActorDescription[] Actors { get; set; }
        public int[] VisibleItems { get; set; }
        public ExitDescription[] Exits { get; set; }
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

        internal void SetData(Room room)
        {
            var exits = room.GetExits();
            Exits = exits.Select(x =>
            {
                return new ExitDescription()
                {
                    Direction = x.Direction,
                    DestinationRoom = x.ExitRoom,
                    Name = x.Portal?.GetName(room) ?? x.Direction.ToString()
                };
            }).ToArray();

            Actors = room.Actors.Select(x =>
           {
               return new ActorDescription()
               {
                   Name = x.Name,
                   Id = x.Id.ToString()
               };
           }).ToArray();
        }
    }

    public class ExitDescription
    {
        public Direction Direction { get; set; }
        public string Name { get; set; }
        public int? DestinationRoom { get; set; }
    }

    public class ActorDescription
    {
        public string Name { get; set; }
        public string Id { get; set; }
    }
}