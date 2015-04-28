using KatanaMUD.Models;
using System;
using System.Linq;

namespace KatanaMUD.Messages
{
    public class MoveMessage : MessageBase
    {
        public Direction? Direction { get; set; }
        public int? Portal { get; set; }

        public override void Process(Actor actor)
        {
            if (Direction != null)
            {
                var exit = actor.Room.GetExit(Direction.Value);
                if (exit == null || exit.ExitRoom == null)
                {
                    actor.SendMessage(new ActionNotAllowedMessage() { Message = "There is no exit in that direction!" });
                    actor.Room.ActiveActors.Except(actor).ForEach(x => x.SendMessage(new GenericMessage() { Class = "failed-movement", Message = actor.Name + " runs into the wall to the " + Direction.Value.ToString().ToLower() }));
                }
                else
                {
                    var action = actor.Party.CanMove(exit);

                    if (action.Allowed)
                    {
                        actor.Party.Move(exit);
                    }
                    else
                    {
                        actor.SendMessage(new ActionNotAllowedMessage() { Message = action.Reason });
                    }
                }
            }
        }
    }
}