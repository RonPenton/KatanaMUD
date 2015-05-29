using KatanaMUD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KatanaMUD.Scripts
{
    public interface IItemScript : IScript
    {
        Item ControllingItem { get; set; }

        void CanDropItem(Room room, Actor actor, Validation validation);
        void CanHideItem(Room room, Actor actor, Validation validation);
        void CanGetItem(Room room, Actor actor, Validation validation);
        void ItemDropped(Room room, Actor actor);
        void ItemHidden(Room room, Actor actor);
        void GetItem(Room room, Actor actor);

        void ActorDropped(Room room, Actor actor);
        void ActorDied(Room room, Actor actor);
    }
}
