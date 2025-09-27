using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public abstract class MapEntity : ScriptableObject
    {
        public abstract string Name { get; }
    }
}
