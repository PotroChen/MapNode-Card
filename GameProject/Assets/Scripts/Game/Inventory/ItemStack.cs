using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Inventory
{
    public enum ItemType
    {
        Unknown = 0//未知
    }

    public struct ItemStack
    {
        public ItemType Type;
        public uint Id;
        public uint Count;
    }

}