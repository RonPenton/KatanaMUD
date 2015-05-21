using KatanaMUD.Messages;
using KatanaMUD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KatanaMUD.Events
{
    public class MoveEvent : GameEvent
    {
        Actor actor;
        Exit exit;

        public MoveEvent(Actor actor, Exit exit, TimeSpan time)
        {
            this.actor = actor;
            this.exit = exit;
            this.ExecutionTime = time;
        }

        public override void Execute()
        {
            // Double check that the user can still move. It's probably an edge case, but someone will abuse it...
            var action = actor.Party.CanMove(exit);

            if (action.Allowed)
            {
                actor.Party.Move(exit);
            }
            else
            {
                actor.SendMessage(new ActionNotAllowedMessage() { Message = action.FirstPerson });
            }
        }
    }
}
