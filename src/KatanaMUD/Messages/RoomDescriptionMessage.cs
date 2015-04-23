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
        public ItemDescription[] VisibleItems { get; set; }
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
    }

    public class ExitDescription
    {
        public Direction Direction { get; set; }
        public string Name { get; set; }
        public int? DestinationRoom { get; set; }

        public ExitDescription() { }

        public ExitDescription(Exit exit, Room room)
        {
            this.Direction = exit.Direction;
            this.DestinationRoom = exit.ExitRoom;
            this.Name = exit.Portal?.GetName(room) ?? exit.Direction.ToString();
        }
    }

    public class ActorDescription
    {
        public string Name { get; set; }
        public Guid Id { get; set; }

        public ActorDescription() { }

        public ActorDescription(Actor actor)
        {
            this.Name = actor.Name;
            this.Id = actor.Id;
        }
    }

    public class ItemDescription
    {
        public string Name { get; set; }
        public Guid Id { get; set; }

        public int TemplateId { get; set; }

        public ItemDescription() { }

        public ItemDescription(Item item)
        {
            this.Name = item.CustomName ?? item.ItemTemplate.Name;
            this.Id = item.Id;
            this.TemplateId = item.ItemTemplate.Id;
        }
    }
}