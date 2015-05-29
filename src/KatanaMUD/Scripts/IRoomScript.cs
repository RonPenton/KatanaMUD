using KatanaMUD.Messages;
using KatanaMUD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KatanaMUD.Scripts
{
    public interface IRoomScript : IScript
    {
        Room ControllingRoom { get; set; }

        void CanDropItem(Item item, Actor actor, Validation validation);
        void CanHideItem(Item item, Actor actor, Validation validation);
        void CanDropCash(Currency currency, long quantity, Actor actor, Validation validation);
        void CanHideCash(Currency currency, long quantity, Actor actor, Validation validation);
        void CanGetItem(Item item, Actor actor, Validation validation);
        void CanGetCash(Currency currency, long quantity, Actor actor, Validation validation);
        void ItemDropped(Item item, Actor actor);
        void ItemHidden(Item item, Actor actor);
        void CashDropped(Currency currency, long quantity, Actor actor);
        void CashHidden(Currency currency, long quantity, Actor actor);
        void ItemGotten(Item item, Actor actor);
        void CashGotten(Currency currency, long quantity, Actor actor);

        void CanGiveItem(Item item, Actor giver, Actor receiver, Validation validation);
        void CanGiveCash(Currency currency, long quantity, Actor giver, Actor receiver, Validation validation);
        void GaveItem(Item item, Actor giver, Actor receiver);
        void GaveCash(Currency currency, long quantity, Actor giver, Actor receiver);

        void CanPartyLeave(Party party, Validation validation);
        void CanPartyEnter(Party party, Validation validation);
        void PartyEntered(Party party);
        void PartyLeft(Party party);

        void CanActorCommunicate(Actor actor, CommunicationType type, string message, Validation validation);
        void ActorCommunicated(Actor actor, CommunicationType type, string message);

        void CanActorEmote(Actor actor, string emoteName, string target, Validation validation);
        void ActorEmoted(Actor actor, string emoteName, string target);

        //TODO: Possibly include the method of attack as well.
        void CanActorAttack(Actor attacker, IEnumerable<Actor> defenders, Validation validation);
        void ActorAttacks(Actor attacker, IEnumerable<Actor> defenders);

        void ActorDropped(Actor actor);
        void ActorDied(Actor actor);
        void ActorDisconnected(Actor actor);
        void ActorConnected(Actor actor);
    }

    public class BaseRoomScript : IRoomScript
    {
        public Room ControllingRoom { get; set; }

        public virtual void CanActorCommunicate(Actor actor, CommunicationType type, string message, Validation validation) { }
        public virtual void ActorCommunicated(Actor actor, CommunicationType type, string message) { }
        public virtual void ActorEmoted(Actor actor, string emoteName, string target) { }
        public virtual void CanActorEmote(Actor actor, string emoteName, string target, Validation validation) { }
        public virtual void CanDropCash(Currency currency, long quantity, Actor actor, Validation validation) { }
        public virtual void CanDropItem(Item item, Actor actor, Validation validation) { }
        public virtual void CanGetCash(Currency currency, long quantity, Actor actor, Validation validation) { }
        public virtual void CanGetItem(Item item, Actor actor, Validation validation) { }
        public virtual void CanHideCash(Currency currency, long quantity, Actor actor, Validation validation) { }
        public virtual void CanHideItem(Item item, Actor actor, Validation validation) { }
        public virtual void CanPartyEnter(Party party, Validation validation) { }
        public virtual void CanPartyLeave(Party party, Validation validation) { }
        public virtual void CanActorAttack(Actor attacker, IEnumerable<Actor> defenders, Validation validation) { }
        public virtual void CashDropped(Currency currency, long quantity, Actor actor) { }
        public virtual void CashGotten(Currency currency, long quantity, Actor actor) { }
        public virtual void CashHidden(Currency currency, long quantity, Actor actor) { }
        public virtual void ItemDropped(Item item, Actor actor) { }
        public virtual void ItemGotten(Item item, Actor actor) { }
        public virtual void ItemHidden(Item item, Actor actor) { }
        public virtual void PartyEntered(Party party) { }
        public virtual void PartyLeft(Party party) { }
        public virtual void ActorAttacks(Actor attacker, IEnumerable<Actor> defenders) { }
        public virtual void ActorDied(Actor actor) { }
        public virtual void ActorDropped(Actor actor) { }
        public virtual void ActorDisconnected(Actor actor) { }
        public virtual void ActorConnected(Actor actor) { }
    }
}
