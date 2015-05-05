using System;
using System.Collections.Generic;
using Spam;

namespace KatanaMUD.Models
{
    public partial class Item : IItem
    {
        public string Name => CustomName ?? ItemTemplate.Name;

        /// <summary>
        /// Gets a stat from the item. Tries to pick it off local item first, then falls back to item template.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public T GetStat<T>(string name, T defaultValue)
        {
            object value;
            if (!((JsonContainer)Stats).GetValue(name, out value))
            {
                if (!((JsonContainer)ItemTemplate.Stats).GetValue(name, out value))
                {
                    return defaultValue;
                }
            }

            return (T)Convert.ChangeType(value, typeof(T));
        }

        public long Weight => GetStat<long>("Weight", 0);

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

        public EquipmentSlot? EquipmentSlot
        {
            get { return EquippedSlot == null ? null : (EquipmentSlot?)EquippedSlot; }
            set { EquippedSlot = (int?)value; }
        }

        public WeaponType? WeaponType => ItemTemplate.WeaponType == null ? null : (WeaponType?)ItemTemplate.WeaponType;
    }

    public enum EquipmentSlot
    {
        Head,
        Face,
        Eyes,
        Ears,           // x2
        Neck,
        Shoulders,
        Chest,
        Back,
        Arms,
        Wrists,         // x2
        Hands,
        Weapon,         // x2, with rules: only 1h weapons, and only if Offhand is empty
        Offhand,
        Fingers,        // x2
        Pocket,
        Waist,
        Legs,
        Feet
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