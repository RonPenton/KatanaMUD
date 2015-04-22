using System;
using KatanaMUD.Models;

namespace KatanaMUD.Messages
{
    public class ActorInformationMessage : MessageBase
    {
        public string Name { get; set; }
        public Guid Id { get; set; }
        public bool IsYou { get; set; }


        public static ActorInformationMessage CreateFirstPerson(Actor actor)
        {
            var message = new ActorInformationMessage()
            {
                Name = actor.Name,
                Id = actor.Id,
                IsYou = true
            };

            return message;
        }
    }
}