using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ChestEntity : MapEntity
    {
        public enum ItemSourceType
        {
            MapLayout,
            Table
        }

        [Serializable]
        public class ChestItemInfo
        {
            [SerializeField]
            private ItemSourceType m_ItemSource;

            public ItemSourceType ItemSource => m_ItemSource;

            [SerializeField]
            private string m_ItemKey;
            public string ItemKey => m_ItemKey;

            [SerializeField]
            private int m_ItemID;
            public int ItemID => m_ItemID;

            [SerializeField]
            private uint m_Count;
            public uint Count => m_Count;
        }

        [SerializeField]
        private ChestItemInfo[] m_Items;

        public ChestItemInfo[] Items => m_Items;

        private Dictionary<int, bool> index2TakenFlagCache = new Dictionary<int, bool>();

        public bool IsItemTaken(int itemIndex)
        {
            if(index2TakenFlagCache.TryGetValue(itemIndex, out bool taken))
            {  
                return taken; 
            }
            return false;
        }

        public bool TryTakeItem(int itemIndex)
        {
            bool taken = IsItemTaken(itemIndex);
            if (taken)
            {
                Debug.LogError($"[ChestEntity]Failed!Item Index:{itemIndex},item has been taken");
                return false;
            }
            index2TakenFlagCache[itemIndex] = true;
            Debug.Log($"[ChestEntity]Succeed!Take Item,Item Index:{itemIndex}");
            return true;
        }
    }
}
