using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using KatanaMUD.Models;

namespace KatanaMUD.Importer.Structures
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public struct ClassBuffer
    {
        public short Number;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 29)]
        char[] Name;
        byte nothing1;
        short MinHp;
        short MaxHp;
        short Exp;
        short nothing2;
        short nothing3;
        short nothing4;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        short[] AbilityA;
        short MagicType;
        short MagicLvl;
        short Weapon;
        short Armour;
        short Combat;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        short[] AbilityB;
        short nothing5;
        short nothing6;
        short nothing7;
        int TitleText;

        public ClassTemplate ToClass(ClassTemplate cls)
        {
            if (cls == null)
                cls = new ClassTemplate();

            cls.Id = Number;
            cls.Name = new string(Name).Replace("\0", "").Trim();
            cls.Stats["HpMin"] = (long)MinHp;
            cls.Stats["HpRange"] = (long)MaxHp;

            return cls;
        }
    }
}