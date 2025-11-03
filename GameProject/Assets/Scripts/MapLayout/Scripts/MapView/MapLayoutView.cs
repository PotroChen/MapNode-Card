using System.Collections.Generic;
using UnityEngine;
using System;
namespace Game
{
    //TODO 后面改成componentPool
    public class MapLayoutView : MonoBehaviour
    {
        public MapLayout data;

        public MapNodeView nodeViewTemplate;
        public MapNodeConnectionView connectionTemplate;
        public Transform nodeRoot;
        public Transform connectionRoot;

        private GameObjectPool nodePool;
        private List<IPoolObject<GameObject>> nodeViewGos = new List<IPoolObject<GameObject>>();

        private GameObjectPool connectionPool;
        private List<IPoolObject<GameObject>> connectionGos = new List<IPoolObject<GameObject>>();

        private Dictionary<MapNode, IPoolObject<GameObject>> node2ViewDic = new Dictionary<MapNode, IPoolObject<GameObject>>();
        private Dictionary<string, IPoolObject<GameObject>> connectionDic = new Dictionary<string, IPoolObject<GameObject>>();


        private void Awake()
        {
            nodePool = new GameObjectPool(nodeViewTemplate.gameObject, 100, transform);
            nodeViewTemplate.gameObject.SetActive(false);

            connectionPool = new GameObjectPool(connectionTemplate.gameObject, 100, transform);
            connectionTemplate.gameObject.SetActive(false);
        }

        private void ClearView()
        {
            if (nodeViewGos.Count > 0)
            {
                foreach (var nodeViewGo in nodeViewGos)
                {
                    nodeViewGo.Recycle();
                }
                nodeViewGos.Clear();
            }

            if (connectionGos.Count > 0)
            {
                foreach (var connectionGo in connectionGos)
                {
                    connectionGo.Recycle();
                }
                connectionGos.Clear();
            }

            node2ViewDic.Clear();
            connectionDic.Clear();
        }

        public void Refresh()
        {
            ClearView();
            if (data == null || data.AllNodes == null || data.AllNodes.Count <= 0)
                return;
            foreach (var node in data.AllNodes)
            {
                CreateNodeView_Internal(node);
            }

            foreach (var node in data.AllNodes)
            {
                if (node.ConnectedNodes != null && node.ConnectedNodes.Count > 0)
                {
                    foreach (var guid in node.ConnectedNodes)
                    {
                        var connectedNode = data.GetNode(new Guid(guid));
                        if (connectedNode == null)
                            continue;
                        CreateConnection_Internal(node, connectedNode);
                    }
                }
            }
        }

        private MapNodeView CreateNodeView_Internal(MapNode node)
        {
            var poolObject = nodePool.Get(nodeRoot);
            var nodeView = poolObject.Content.GetComponent<MapNodeView>();
            nodeView.data = node;
            nodeView.transform.position = MapMetrics.MapCoordinateToUnityWorldPos(node.Position);
            nodeView.gameObject.name = node.RuntimeID.ToString();
            nodeView.Refresh();
            node2ViewDic[node] = poolObject;
            nodeViewGos.Add(poolObject);
            return nodeView;
        }

        private MapNodeConnectionView CreateConnection_Internal(MapNode node1, MapNode node2)
        {
            string connectionKey = node1.RuntimeID < node2.RuntimeID ?
                $"{node1.RuntimeID}_{node2.RuntimeID}" : $"{node2.RuntimeID}_{node1.RuntimeID}";

            if (connectionDic.ContainsKey(connectionKey))
                return null;

            var poolObject = connectionPool.Get(connectionRoot);
            var connectionView = poolObject.Content.GetComponent<MapNodeConnectionView>();
            node2ViewDic.TryGetValue(node1, out var nodePo1);
            node2ViewDic.TryGetValue(node2, out var nodePo2);

            connectionView.key = connectionKey;
            connectionView.nodeView1 = nodePo1.Content.GetComponent<MapNodeView>();
            connectionView.nodeView2 = nodePo2.Content.GetComponent<MapNodeView>();
            connectionView.Refresh();

            connectionDic[connectionKey] = poolObject;
            connectionGos.Add(poolObject);
            return connectionView;
        }

#if UNITY_EDITOR
        [ContextMenu("刷新")]
        public void Refresh_Editor()
        {
            data.Init();
            Refresh();
        }
#endif
    }

}