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
            if(Direction != null)
            {
                var exit = actor.Room.GetExit(Direction.Value);
                if (exit == null)
                {
                    // TODO: Bump into wall
                }
                else
                {
                    //TODO: Determine if user is ambulatory
                    if (exit.ExitRoom != null)
                    {
                        // TODO: Party-based movement. 
                        actor.ChangeRooms(actor.Room, exit.ExitRoom);
                    }
                    else if (exit.Portal != null)
                    {
                        //TODO: portal-based movement
                    }
                }
            }
        }
    }
}