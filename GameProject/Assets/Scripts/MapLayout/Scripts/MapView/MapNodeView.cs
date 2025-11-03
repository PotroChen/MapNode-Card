using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class MapNodeView : MonoBehaviour
    {
        [HideInInspector]
        public MapNode data;
        public Text nameText;
        public void Refresh()
        {
            if (data == null)
                return;

            nameText.text = data.Name;
        }
    }

}