using Game.DungeonModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    //假设此时，只需要通过1个LocalItem,且数量也只需要一个
    public class DoorEntity : MapEntity,ICanEnter
    {
        [SerializeField]
        internal string m_ItemToUnlock;
        public string ItemToUnlock => m_ItemToUnlock;

        public bool IsUnlocked { get; private set; } = false;

        public bool TryUnlock(DungeonPlayer dungeonPlayer)
        {
            var itemDefine = Owner.Layout.GetItemDefine(m_ItemToUnlock);
            if (itemDefine == null)
            {
                return false;
            }

            if (dungeonPlayer.TryCostLocalItem(m_ItemToUnlock, 1))
                return true;
            Debug.Log($"Do not contain item:{itemDefine.Name},Count:{1}");
            return false;
        }

        public bool CanEnter()
        {
            Debug.Log($"被{DisplayName}挡住");
            return IsUnlocked;
        }
    }

}