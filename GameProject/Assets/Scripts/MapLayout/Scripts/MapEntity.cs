using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
}
