using System;
using KatanaMUD.Models;
using System.Linq;
using KatanaMUD.Scripts;

namespace KatanaMUD.Messages
{
    public class CommunicationMessage : MessageBase
    {
        public string Message { get; set; }
        public CommunicationType Type { get; set; }
        public string Chatroom { get; set; }
        public string ActorName { get; set; }
        public Guid ActorId { get; set; }

        public override void Process(Actor actor)
        {
            if (String.IsNullOrWhiteSpace(Message))
                return;

            var validation = actor.Room.Scripts.Validate((x, v) => x.CanActorCommunicate(actor.Room, actor, Type, Message, v));
            if (!validation.Allowed)
            {
                validation.HandleFailure(actor);
                return;
            }

            //TODO: process message to remove extranaeous characters/codes.
            switch (Type)
            {
                case CommunicationType.Gossip: Gossip(actor); break;
                case CommunicationType.Say: Say(actor); break;
                case CommunicationType.Telepath: Telepath(actor); break;
            }

            actor.Room.Scripts.ForEach(x => x.ActorCommunicated(actor.Room, actor, Type, Message));
        }

        private void Telepath(Actor actor)
        {
            var playerActors = Game.Connections.GetConnections().Select(x => x.Actor);
            var targets = playerActors.FindByName(this.ActorName, x => x.Name, true, false);

            if(targets.Count() > 1)
            {
                var message = new AmbiguousActorMessage() { Actors = targets.Select(x => new ActorDescription(x)).ToArray() };
                return;
            }

            var target = targets.FirstOrDefault();
            if (target != null)
            {
                ActorName = actor.Name;
                ActorId = actor.Id;
                target.SendMessage(this);

                var message = new GenericMessage() { Class = "telepath", Message = "--- Telepath Sent to " + target.Name + " ---" };
                actor.SendMessage(message);
                return;
            }

            actor.SendMessage(new ActionNotAllowedMessage() { Message = "Cannot find user!" });

            //TODO: block list. 
        }

        private void Say(Actor actor)
        {
            var actors = actor.Room.ActiveActors;
            ActorName = actor.Name;
            ActorId = actor.Id;
            foreach (var a in actors)
            {
                a.SendMessage(this);
            }
        }

        private void Gossip(Actor actor)
        {
            //TODO: Check if user is allowed to gossip.
            ActorName = actor.Name;
            ActorId = actor.Id;
            var connections = Game.Connections.GetConnections();
            foreach (var connection in connections)
            {
                connection.Actor.SendMessage(this);
            }
        }
    }

    public enum CommunicationType
    {
        Gossip,
        Auction,
        Say,
        Yell,
        Region,
        Gangpath,
        Officerpath,
        Chatroom,
        Telepath
    }
}