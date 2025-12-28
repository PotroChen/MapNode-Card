using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public partial class MapLayout
    {
        [Serializable]
        public class ItemDefine
        {
            public string Key;
            public string Name;
            public string Desc;
        }
    }
    public partial class MapLayout : ScriptableObject
    {
        [SerializeField]
        private List<ItemDefine> m_LocalItems = new List<ItemDefine>();

        [SerializeField]
        private List<MapNode> m_AllNodes = new List<MapNode>();
        public List<MapNode> AllNodes { get => m_AllNodes; set => m_AllNodes = value; }

        private MapNode m_StartNode;
        public MapNode StartNode
        {
            get
            {
                if (m_StartNode != null)
                    return m_StartNode;

                if (AllNodes != null)
                {
                    m_StartNode = AllNodes.Find((MapNode node) => node != null && node.IsStartNode);
                    if (m_StartNode == null && AllNodes.Count > 0)
                    {
                        AllNodes[0].IsStartNode = true;
                        m_StartNode = AllNodes[0];
                    }
                }
                return m_StartNode;
            }
            set
            {
                if (value == null)
                {
                    Debug.LogError("Start node can not be null");
                    return;
                }
                if (StartNode != null)
                {
                    StartNode.IsStartNode = false;
                }
                value.IsStartNode = true;
                m_StartNode = value;
            }

        }

        private Dictionary<Guid, MapNode> guid2NodeDic = new Dictionary<Guid, MapNode>();
        private Dictionary<Vector2Int, MapNode> pos2NodeDic = new Dictionary<Vector2Int, MapNode>();
        private Dictionary<string, ItemDefine> key2LocalItemDic = new Dictionary<string, ItemDefine>();

        private int idGenderator = 0;
        public void Init()
        {
            idGenderator = 0;
            pos2NodeDic.Clear();
            //节点初始化
            if (AllNodes != null && AllNodes.Count > 0)
            {
                for (int i = AllNodes.Count - 1; i >= 0; i--)
                {
                    var node = AllNodes[i];
                    if (node != null)
                    {
                        node.Layout = this;
                        node.RuntimeID = ++idGenderator;
                        if (!TryRegisterNode(node))
                        {
                            AllNodes.RemoveAt(i);
                        }
                    }
                }
            }
            //LocalItem初始化
            key2LocalItemDic.Clear();
            if (m_LocalItems != null && m_LocalItems.Count > 0)
            {
                foreach (var localItem in m_LocalItems)
                {
                    if(localItem == null)
                        continue;
                    if (key2LocalItemDic.TryGetValue(localItem.Key, out var itemDefine))
                    {
                        Debug.LogError($"[MapLayout]Name:{name},存在相同的Key:{localItem.Key}");
                    }
                    else
                    {
                        key2LocalItemDic[localItem.Key] = itemDefine;
                    }
                }
            }
        }

        public MapNode CreateNode(string name, Vector2Int position)
        {
            if (pos2NodeDic.ContainsKey(position))
            {
                Debug.LogError("Can not create more than one node at same position");
                return null;
            }
            MapNode node = new MapNode(name);
            node.RuntimeID = ++idGenderator;
            node.Position = position;
            node.GUID = Guid.NewGuid();
            node.Layout = this;
            if (TryRegisterNode(node))
            {
                if (m_AllNodes == null)
                    m_AllNodes = new List<MapNode>();
                m_AllNodes.Add(node);
                return node;
            }
            return null;
        }

        public void DestroyNode(MapNode node)
        {
            if (node == StartNode)
            {
                Debug.Log("Can not destory startNode");
                return;
            }
            if (node.ConnectedNodes != null)
            {
                for (int i = node.ConnectedNodes.Count - 1; i >= 0; i--)
                {
                    var connectedNode = node.GetConnectedNodeAt(i);
                    if (connectedNode != null)
                        node.RemoveConnectedNode(connectedNode);
                }
            }
            if (m_AllNodes != null)
            {
                if (m_AllNodes.Contains(node))
                {
                    m_AllNodes.Remove(node);
                }
            }
            pos2NodeDic.Remove(node.Position);
            guid2NodeDic.Remove(node.GUID);
        }

        private bool TryRegisterNode(MapNode node)
        {
            if (pos2NodeDic.ContainsKey(node.Position))
            {
                Debug.LogError("Exist more than one node at same position,delete one");
                return false;
            }
            pos2NodeDic[node.Position] = node;
            guid2NodeDic[node.GUID] = node;
            return true;
        }

        #region 查询接口
        public MapNode GetNode(Guid guid)
        {
            guid2NodeDic.TryGetValue(guid, out MapNode node);
            return node;
        }

        public List<MapEntity> GetAllEntities()
        {
            var entities = new List<MapEntity>();
            if (m_AllNodes != null && m_AllNodes.Count > 0)
            {
                foreach (var node in m_AllNodes)
                {
                    if(node == null)
                        continue;
                    if (node.Entities != null && node.Entities.Count > 0)
                    {
                        foreach (var entity in node.Entities)
                        {
                            if(entity == null)
                                continue;
                            entities.Add(entity);
                        }
                    }
                }
            }
            return entities;
        }
        #endregion
    }
}
