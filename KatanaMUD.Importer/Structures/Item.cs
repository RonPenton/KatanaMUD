using KatanaMUD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KatanaMUD.Importer.Structures
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    [Serializable]
    public struct ItemBuffer
    {
        public int Number;
        public short Ignore1;
        public short GameLimit;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public short[] Ignore2;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 157)]
        public char[] Ignore;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 29)]
        public char[] Name;
        public byte Ignore3;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
        public char[] DescriptionOne;
        public byte Ignore4;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
        public char[] DescriptionTwo;
        public byte Ignore5;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
        public char[] DescriptionThree;
        public byte Ignore6;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
        public char[] DescriptionFour;
        public byte Ignore7;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
        public char[] DescriptionFive;
        public byte Ignore8;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
        public char[] DescriptionSix;
        public byte Ignore9;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
        public char[] DescriptionSeven;
        public byte Ignore10;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
        public char[] DescriptionEight;
        public byte Ignore11;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
        public char[] DescriptionNine;
        public byte Ignore12;
        public short Ignore13;
        public short Weight;
        public short Type;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public short[] AbilityNumber;
        public short Uses;
        public short Ignore14;
        public short Cost;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public short[] Class;
        public short Ignore15;
        public short Ignore16;
        public short Ignore17;
        public short MinHit;
        public short MaxHit;
        public short AC;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public short[] Race;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public short[] Ignore18;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public short[] Negate;
        public short Weapon;
        public short Armour;
        public short WornOn;
        public short Accuracy;
        public short DamageResist;
        public byte Gettable;
        public byte Ignore19;
        public short RequiredStr;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public short[] Ignore20;
        public int OpenRunic;
        public int OpenPlatinum;
        public int OpenGold;
        public int OpenSilver;
        public int OpenCopper;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
        public short[] Ignore21;
        public short Speed;
        public short Ignore22;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public short[] AbilityValue;
        public short Ignore23;
        public int HitMsg;
        public int MissMsg;
        public int ReadTB;
        public int DestructMsg;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public short[] Ignore24;
        public byte NotDroppable;
        public byte CostType;
        public byte RetainAfterUses;
        public byte Robable;
        public byte DestroyOnDeath;
        public short Ignore25;


        public override string ToString()
        {
            return String.Format("{0} ({1})", new string(Name.TakeWhile(x => x != '\0').ToArray()), Number);
        }

        public string Description
        {
            get
            {
                Regex r = new Regex("\\s+");
                return r.Replace((Clean(DescriptionOne) +
                    Clean(DescriptionTwo) +
                    Clean(DescriptionThree) +
                    Clean(DescriptionFour) +
                    Clean(DescriptionFive) +
                    Clean(DescriptionSix) +
                    Clean(DescriptionSeven) +
                    Clean(DescriptionEight) +
                    Clean(DescriptionNine)).Trim(), " ");
            }
        }

        private string Clean(char[] input)
        {
            return new String(input.TakeWhile(x => x != '\0').ToArray()) + " ";
        }

        public ItemTemplate ToItemTemplate(ItemTemplate item)
        {
            if (item == null)
            {
                item = new ItemTemplate();
            }

            item.Id = Number;
            item.Name = new string(Name.TakeWhile(x => x != '\0').ToArray()).Trim();

            item.Stats.Set<long>("Weight", Weight);

            switch (WornOn)
            {
                case 2: item.EquipType = EquipmentSlot.Head; break;
                case 3: item.EquipType = EquipmentSlot.Hands; break;
                case 4: item.EquipType = EquipmentSlot.Fingers; break;
                case 5: item.EquipType = EquipmentSlot.Feet; break;
                case 6: item.EquipType = EquipmentSlot.Arms; break;
                case 7: item.EquipType = EquipmentSlot.Back; break;
                case 8: item.EquipType = EquipmentSlot.Neck; break;
                case 9: item.EquipType = EquipmentSlot.Legs; break;
                case 10: item.EquipType = EquipmentSlot.Waist; break;
                case 11: item.EquipType = EquipmentSlot.Chest; break;
                case 12: item.EquipType = EquipmentSlot.Offhand; break;
                case 13: item.EquipType = EquipmentSlot.Fingers; break;
                case 14: item.EquipType = EquipmentSlot.Wrists; break;
                case 15: item.EquipType = EquipmentSlot.Ears; break;
                case 16: item.EquipType = EquipmentSlot.Pocket; break;
                case 17: item.EquipType = EquipmentSlot.Wrists; break;
                case 18: item.EquipType = EquipmentSlot.Eyes; break;
                case 19: item.EquipType = EquipmentSlot.Face; break;
            }

            if (Type == 6)
            {
                item.EquipType = EquipmentSlot.Light;
            }
            else if (Type == 1)
            {
                item.EquipType = EquipmentSlot.Weapon;
            }

            item.Fixed = Gettable == 0;
            item.NotDroppable = NotDroppable != 0;
            item.NotRobable = Robable == 0;
            item.DestroyOnDeath = DestroyOnDeath != 0;

            long costMul = 1;
            switch (CostType)
            {
                case 1: costMul = 10; break;
                case 2: costMul = 100; break;
                case 3: costMul = 10000; break;
                case 4: costMul = 1000000; break;
            }
            item.Cost = Cost;
            item.Cost = item.Cost * costMul;

            if (item.EquipType == EquipmentSlot.Weapon)
            {
                switch (Weapon)
                {
                    case 0: item.WeaponType = WeaponType.Club1H; break;
                    case 1: item.WeaponType = WeaponType.Club2H; break;
                    case 2: item.WeaponType = WeaponType.Sword1H; break;
                    case 3: item.WeaponType = WeaponType.Sword2H; break;
                }
            }

            return item;
        }

        public const int RecordSize = 1072;
    }
}