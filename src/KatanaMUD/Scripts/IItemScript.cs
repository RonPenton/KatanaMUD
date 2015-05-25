using KatanaMUD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KatanaMUD.Scripts
{
    public interface IItemScript : IScript
    {
        void CanDropItem(Item item, Room room, Actor actor, Validation validation);
        void CanHideItem(Item item, Room room, Actor actor, Validation validation);
        void CanGetItem(Item item, Room room, Actor actor, Validation validation);
        void ItemDropped(Item item, Room room, Actor actor);
        void ItemHidden(Item item, Room room, Actor actor);
        void GetItem(Item item, Room room, Actor actor);

        void PlayerDropped(Item item, Room room, Actor actor);
        void PlayerDied(Item item, Room room, Actor actor);
    }
}
