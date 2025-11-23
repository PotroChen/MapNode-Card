using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class MapNode
    {
        public int RuntimeID { get; set; }

        [SerializeField]
        [HideInInspector]
        private string m_GUIDStr;

        private Guid m_GUID;
        public Guid GUID 
        { 
            get 
            {
                if (m_GUID == default)
                {
                    if (string.IsNullOrEmpty(m_GUIDStr))
                    {
                        return Guid.Empty;
                    }
                    if(!Guid.TryParse(m_GUIDStr, out m_GUID))
                    {
                        return Guid.Empty;
                    }
                }
                return m_GUID;
            }
            set
            {
                m_GUID = value;
                m_GUIDStr = m_GUID.ToString();
            }
        }

        [SerializeField]
        private Vector2Int m_Position;
        public Vector2Int Position
        {
            get { return m_Position; }
            set { m_Position = value; }
        }

        [SerializeField]
        private List<string> m_ConnectedNodes;
        public List<string> ConnectedNodes
        {
            get { return m_ConnectedNodes; }
            set { m_ConnectedNodes = value; }
        }

        [SerializeField]
        private string m_Name;
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        [SerializeField]
        private string m_Description;
        public string Description
        {
            get { return m_Description; }
            set { m_Description = value; }
        }

        [SerializeField]
        private List<MapEntity> m_Entities;
        public List<MapEntity> Entities
        {
            get { return m_Entities; }
            set { m_Entities = value; }
        }

        public bool IsStartNode { get; set; }
        public MapLayout Layout { get; set; }

        public MapNode(string name)
        {
            this.m_Name = name;
        }

        public MapNode GetConnectedNodeAt(int index)
        {
            if(ConnectedNodes == null)
                return null;
            if (index < 0 || index >= ConnectedNodes.Count)
                return null;
            var guidStr = ConnectedNodes[index];
            return Layout.GetNode(new Guid(guidStr));
        }

        public void AddConnectedNode(MapNode node)
        {
            if(ConnectedNodes == null)
                ConnectedNodes = new List<string>();
            ConnectedNodes.Add(node.GUID.ToString());

            if (node.ConnectedNodes == null)
                node.ConnectedNodes = new List<string>();
            node.ConnectedNodes.Add(GUID.ToString());
        }

        public void RemoveConnectedNode(MapNode node)
        {
            RemoveConnectedNode_Internal(node);
            node.RemoveConnectedNode_Internal(this);
        }

        private void RemoveConnectedNode_Internal(MapNode node)
        {
            if (ConnectedNodes == null)
                return;

            for (int i = ConnectedNodes.Count - 1; i >= 0; i--)
            {
                var connectedNodeGuid = ConnectedNodes[i];
                if (connectedNodeGuid == node.GUID.ToString())
                {
                    ConnectedNodes.RemoveAt(i);
                    break;
                }
            }
        }
    }

}