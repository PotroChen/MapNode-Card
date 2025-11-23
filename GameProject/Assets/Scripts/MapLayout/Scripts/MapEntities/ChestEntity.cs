using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ChestEntity : MapEntity
    {
        [SerializeField]
        private int m_ItemID;
        public int ItemID => m_ItemID;

        [SerializeField]
        private int m_Count;
        public int Count => m_Count;
    }
}
