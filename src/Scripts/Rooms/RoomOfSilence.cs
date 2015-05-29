using System;
using System.Collections.Generic;
using System.Linq;
using KatanaMUD;
using KatanaMUD.Messages;
using KatanaMUD.Models;
using KatanaMUD.Scripts;

public class RoomOfSilence : BaseRoomScript
{
    public override void CanActorCommunicate(Actor actor, CommunicationType type, string message, Validation validation)
    {
        if (actor.User.AccessLevel != AccessLevel.Sysop && type == CommunicationType.Say)
        {
            validation.Fail("You open your mouth to speak, but the bishop casts a stern look at you, and you think it better to close your mouth.", null, nameof(RoomOfSilence));
        }
    }
}