using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class MapLayoutEditView : MonoBehaviour
    {
        [NonSerialized]
        public MapEditor mapEditor;
        //[NonSerialized]
        public MapLayout Data;

        public MapNodeEditView nodeViewTemplate;
        public LineRenderer lineTemplate;

        public Transform nodeRoot;
        public Transform connectionRoot;

        private Dictionary<MapNode, IPoolObject<GameObject>> node2ViewDic = new Dictionary<MapNode, IPoolObject<GameObject>>();

        private GameObjectPool nodePool;
        private List<IPoolObject<GameObject>> nodeViewGos = new List<IPoolObject<GameObject>>();

        private void Awake()
        {
            nodePool = new GameObjectPool(nodeViewTemplate.gameObject, 100,transform);
            nodeViewTemplate.gameObject.SetActive(false);

            linePool = new GameObjectPool(lineTemplate.gameObject,100,transform);
            lineTemplate.gameObject.SetActive(false);
        }

        private void Update()
        {
            DrawConnections();
        }
        HashSet<MapNode> temp_CreatedNodes= new HashSet<MapNode>();
        public void Refresh()
        {
            if (nodeViewGos.Count > 0)
            {
                foreach (var nodeViewGo in nodeViewGos)
                {
                    nodeViewGo.Recycle();
                }
                nodeViewGos.Clear();
            }
            temp_CreatedNodes.Clear();
            node2ViewDic.Clear();
            if (Data.AllNodes == null || Data.AllNodes.Count <= 0)
                return;
            foreach (var node in Data.AllNodes)
            {
                CreateNodeView_Internal(node);
            }
        }

        public MapNodeEditView CreateNode(string name, Vector2Int position)
        {
            var nodeData = Data.CreateNode(name, position);
            if (nodeData == null)
                return null;

            var nodeView = CreateNodeView_Internal(nodeData);
            return nodeView;
        }

        private MapNodeEditView CreateNodeView_Internal(MapNode node)
        {
            var poolObject = nodePool.Get(nodeRoot);
            var nodeView = poolObject.Content.GetComponent<MapNodeEditView>();
            nodeView.LayoutView = this;
            nodeView.Data = node;
            node2ViewDic[node] = poolObject;
            nodeView.transform.position = MapMetrics.MapCoordinateToUnityWorldPos(node.Position);
            nodeView.Refresh();
            return nodeView;
        }

        public void DestroyNode(MapNodeEditView nodeView)
        {
            Data.DestroyNode(nodeView.Data);
            DestroyNodeView_Internal(nodeView);
        }

        private void DestroyNodeView_Internal(MapNodeEditView nodeView)
        {
            if (node2ViewDic.TryGetValue(nodeView.Data, out var poolObject))
            {
                poolObject.Recycle();
                node2ViewDic[nodeView.Data] = null;
            }
            
        }

        #region 绘制相关

        #region 绘制连线
        private GameObjectPool linePool;
        private List<IPoolObject<GameObject>> linePoolObjs = new List<IPoolObject<GameObject>>();

        private HashSet<(MapNode, MapNode)> tempDrawnPairs= new HashSet<(MapNode, MapNode)>();
        private void DrawConnections()
        {
            if (Data == null || Data.AllNodes == null) return;
            tempDrawnPairs.Clear();
            ClearAllLines();
            foreach (var node in Data.AllNodes)
            {
                if (node.ConnectedNodes == null) continue;

                foreach (var connectedNodeGuid in node.ConnectedNodes)
                {
                    var connectedNode = Data.GetNode(new Guid(connectedNodeGuid));
                    // 创建有序对确保A->B和B->A被视为相同
                    var pair = (node.RuntimeID < connectedNode.RuntimeID)
                        ? (node, connectedNode)
                        : (connectedNode, node);
                    if (!tempDrawnPairs.Contains(pair))
                    {
                        tempDrawnPairs.Add(pair);
                        MapNodeEditView startNodeView = GetNodeView(pair.Item1);
                        MapNodeEditView endNodeView = GetNodeView(pair.Item2);

                        if (startNodeView != null && endNodeView != null)
                        {
                            DrawConnection(startNodeView.transform.position, endNodeView.transform.position);
                        }
                    }
                }
            }
        }

        private void DrawConnection(Vector3 startPos,Vector3 endPos)
        {
            var linePoolObj = linePool.Get(connectionRoot);

            LineRenderer lineRenderer = linePoolObj.Content.GetComponent<LineRenderer>();
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, startPos);
            lineRenderer.SetPosition(1, endPos);
            linePoolObjs.Add(linePoolObj);
        }

        private void ClearAllLines()
        {
            if (linePoolObjs.Count > 0)
            {
                foreach (var lineGo in linePoolObjs)
                {
                    lineGo.Recycle();
                }
                linePoolObjs.Clear();
            }
        }

        #endregion
        #endregion
        #region 查询接口
        private MapNodeEditView GetNodeView(MapNode node)
        {
            if (node2ViewDic.TryGetValue(node, out var rtn))
                return rtn.Content.GetComponent<MapNodeEditView>();
            return null;
        }
        #endregion
    }
}
