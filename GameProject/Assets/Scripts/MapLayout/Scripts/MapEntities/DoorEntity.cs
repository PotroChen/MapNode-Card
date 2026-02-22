using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    //假设此时，只需要通过1个LocalItem,且数量也只需要一个
    public class DoorEntity : MapEntity
    {
        [SerializeField]
        internal string m_ItemToUnlock;
        public string ItemToUnlock => m_ItemToUnlock;
    }

}