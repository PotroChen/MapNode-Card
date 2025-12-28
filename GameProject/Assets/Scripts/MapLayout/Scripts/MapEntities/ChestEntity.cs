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

        [SerializeField]
        private ItemSourceType m_ItemSource;

        public ItemSourceType ItemSource => m_ItemSource;

        [SerializeField]
        private string m_ItemKey;

        [SerializeField]
        private int m_ItemID;
        public int ItemID => m_ItemID;

        [SerializeField]
        private int m_Count;
        public int Count => m_Count;
    }
}
