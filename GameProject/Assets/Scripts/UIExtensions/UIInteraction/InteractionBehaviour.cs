using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.UIKit
{
    public partial class UIInteraction
    {
        [Serializable]
        public abstract class InteractionBehaviour 
        {
            public abstract void Execute();
        }

        public abstract class InteractionBehaviourT<T>
        {
            public abstract void Execute(T args);
        }
    }

}