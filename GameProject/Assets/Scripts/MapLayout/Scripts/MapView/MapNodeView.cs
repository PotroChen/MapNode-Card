using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class MapNodeView : MonoBehaviour
    {
        [HideInInspector]
        public MapNode data;
        public Image frameBg;
        public Text nameText;

        public void Refresh()
        {
            if (data == null)
                return;

            nameText.text = data.Name;
        }

        public void SetColor(Color color)
        {
            if (nameText != null)
            {
                nameText.color = color;
            }
            if (frameBg != null)
            {
                frameBg.color = color;
            }
        }

    }

}