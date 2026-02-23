using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Game.MapLayout;

namespace Game
{
    public abstract class MapEntity : ScriptableObject
    {
        public MapNode Owner { get; set; }
        public string DisplayName
        {
            get { return name; }
            set
            {
                name = value;
            }
        }

        public virtual string GetInteractionName()
        {
            return "交互";
        }

        public ItemDefine GetItemDefine(string itemKey)
        {
            return Owner.Layout.GetItemDefine(itemKey);
        }
    }
}
