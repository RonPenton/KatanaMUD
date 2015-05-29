using System;
using System.Collections.Generic;
using Spam;
using System.Linq;

namespace KatanaMUD.Models
{
    public partial class Item : IItem
    {
        public string Name => CustomName ?? ItemTemplate.Name;

        public AddingContainer Stats;

        partial void OnConstruct()
        {
            Stats = new AddingContainer(this.StatsInternal, GetStatContainers);
        }

        private IEnumerable<IDictionaryStore> GetStatContainers()
        {
            yield return this.StatsInternal;
            yield return this.ItemTemplate.Stats;
        }

        public long Weight => Stats.GetCalculatedValue<long>("Weight");

        /// <summary>
        /// A list of users who currently know of my existence. 
        /// This list is cleared when the owner of an item is changed.
        /// TODO: Clear this list at cleanup to prevent memory leaks.
        /// </summary>
        public HashSet<Actor> UsersWhoFoundMe { get; } = new HashSet<Actor>();

        partial void OnRoomChanging(Room oldValue, Room newValue)
        {
            if (oldValue != newValue)
                UsersWhoFoundMe.Clear();
        }

        public WeaponType? WeaponType => ItemTemplate.WeaponType == null ? null : (WeaponType?)ItemTemplate.WeaponType;

        public void SetOwner(Room room) => SetOwner(room, null);
        public void SetOwner(Actor actor) => SetOwner(null, actor);

        private void SetOwner(Room room, Actor actor)
        {
            if (room != null && actor != null)
                throw new InvalidOperationException("An item cannot have more than one owner");
            Room = room;
            Actor = actor;
        }
    }

    public class ItemGroup : IItem
    {
        public IEnumerable<Item> Items { get; set; }
        public string Name => Items?.FirstOrDefault()?.Name ?? null;
        public int Quantity => Items?.Count() ?? 0;
    }

    public enum EquipmentSlot
    {
        Weapon,         // x2, with rules: only 1h weapons, and only if Offhand is empty
        Offhand,

        Head,
        Chest,
        Legs,
        Hands,
        Feet,

        Face,
        Arms,
        Shoulders,
        Back,
        Pocket,
        Waist,

        Eyes,
        Ears,           // x2
        Neck,
        Wrists,         // x2
        Fingers,        // x2

        Light
    }

    // TODO: Consider making weapon types Data-Driven
    public enum WeaponType
    {
        Bow,            // 2H Sharp
        Crossbow,       // 1H Sharp
        HeavyCrossbow,  // 2H Sharp

        Dagger,         // 1H Sharp Dual

        Thrown,         // 1H Sharp
        Javelin,        // 1H Sharp

        Spear,          // 2H Sharp
        Halberd,        // 2H Sharp

        Hammer1H,       // 1H Blunt
        Hammer2H,       // 2H Blunt
        Club1H,         // 1H Blunt
        Club2H,         // 2H Blunt
        Staff,          // 2H Blunt

        Axe1H,          // 1H Sharp Dual
        Axe2H,          // 2H Sharp
        Sword1H,        // 1H Sharp Dual
        Sword2H,        // 2H Sharp
    }

    public static class WeaponTypes
    {
        public static bool IsTwoHanded(this WeaponType? weaponType)
        {
            if (weaponType == null)
                throw new InvalidOperationException();

            return weaponType.Value.In(WeaponType.Bow, WeaponType.HeavyCrossbow, WeaponType.Spear, WeaponType.Halberd, WeaponType.Hammer2H, WeaponType.Club2H, WeaponType.Staff, WeaponType.Axe2H, WeaponType.Sword2H);
        }
    }

    public interface IItem
    {
        string Name { get; }
    }
}