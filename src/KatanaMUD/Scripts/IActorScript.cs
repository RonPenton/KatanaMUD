using KatanaMUD.Messages;
using KatanaMUD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KatanaMUD.Scripts
{
    public interface IActorScript : IScript
    {
        void CanDropItem(Room room, Item item, Actor actor, Validation validation);
        void CanHideItem(Room room, Item item, Actor actor, Validation validation);
        void CanDropCash(Room room, Currency currency, long quantity, Actor actor, Validation validation);
        void CanHideCash(Room room, Currency currency, long quantity, Actor actor, Validation validation);
        void CanGetItem(Room room, Item item, Actor actor, Validation validation);
        void CanGetCash(Room room, Currency currency, long quantity, Actor actor, Validation validation);

        void ItemDropped(Room room, Item item, Actor control, Actor actor);
        void ItemHidden(Room room, Item item, Actor actor);
        void CashDropped(Room room, Currency currency, long quantity, Actor control, Actor actor);
        void CashHidden(Room room, Currency currency, long quantity, Actor actor);
        void ItemGotten(Room room, Item item, Actor control, Actor actor);
        void CashGotten(Room room, Currency currency, long quantity, Actor control, Actor actor);

        void CanActorLeave(Room room, Actor actor, Validation validation);
        void CanActorEnter(Room room, Actor actor, Validation validation);
        void PartyEntered(Room room, Party party, Actor control);
        void PartyLeft(Room room, Party party, Actor control);

        void CanActorCommunicate(Room room, Actor actor, CommunicationType type, string message, Validation validation);
        void ActorCommunicated(Room room, Actor communicator, Actor control, CommunicationType type, string message);

        void CanActorEmote(Room room, Actor actor, string emoteName, string target, Validation validation);
        void ActorEmoted(Room room, Actor emoter, Actor control, string emoteName, string target);

        void CanActorAttack(Room room, Actor control, IEnumerable<Actor> defenders, Validation validation);
        void ActorAttacks(Room room, Actor control, Actor attacker, IEnumerable<Actor> defenders);

        void ActorDropped(Room room, Actor control, Actor actor);
        void ActorDied(Room room, Actor control, Actor actor);
        void ActorDisconnected(Room room, Actor control, Actor actor);
        void ActorConnected(Room room, Actor control, Actor actor);

    }
}
