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

    public class DefaultRoomScript : IRoomScript
    {
        public Room ControllingRoom { get; set; }

        public void CanActorCommunicate(Actor actor, CommunicationType type, string message, Validation validation) { }
        public void ActorCommunicated(Actor actor, CommunicationType type, string message) { }
        public void ActorEmoted(Actor actor, string emoteName, string target) { }
        public void CanActorEmote(Actor actor, string emoteName, string target, Validation validation) { }
        public void CanDropCash(Currency currency, long quantity, Actor actor, Validation validation) { }
        public void CanDropItem(Item item, Actor actor, Validation validation) { }
        public void CanGetCash(Currency currency, long quantity, Actor actor, Validation validation) { }
        public void CanGetItem(Item item, Actor actor, Validation validation) { }
        public void CanHideCash(Currency currency, long quantity, Actor actor, Validation validation) { }
        public void CanHideItem(Item item, Actor actor, Validation validation) { }
        public void CanPartyEnter(Party party, Validation validation) { }
        public void CanPartyLeave(Party party, Validation validation) { }
        public void CanActorAttack(Actor attacker, IEnumerable<Actor> defenders, Validation validation) { }
        public void CashDropped(Currency currency, long quantity, Actor actor) { }
        public void CashGotten(Currency currency, long quantity, Actor actor) { }
        public void CashHidden(Currency currency, long quantity, Actor actor) { }
        public void ItemDropped(Item item, Actor actor) { }
        public void ItemGotten(Item item, Actor actor) { }
        public void ItemHidden(Item item, Actor actor) { }
        public void PartyEntered(Party party) { }
        public void PartyLeft(Party party) { }
        public void ActorAttacks(Actor attacker, IEnumerable<Actor> defenders) { }
        public void ActorDied(Actor actor) { }
        public void ActorDropped(Actor actor) { }
        public void ActorDisconnected(Actor actor) { }
        public void ActorConnected(Actor actor) { }
    }
}
