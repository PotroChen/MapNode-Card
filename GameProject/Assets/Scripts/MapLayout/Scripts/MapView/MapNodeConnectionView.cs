using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class MapNodeConnectionView : MonoBehaviour
    {
        public string key;
        public MapNodeView nodeView1;
        public MapNodeView nodeView2;

        private LineRenderer lineRenderer;
        public void Refresh()
        {
            if (lineRenderer == null)
            {
                lineRenderer = GetComponent<LineRenderer>();
            }
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, nodeView1.transform.position);
            lineRenderer.SetPosition(1, nodeView2.transform.position);
        }
    }
}
