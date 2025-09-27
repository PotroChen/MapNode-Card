using GameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public static class MapEvents
    {
        public struct RightClickNodeView : IEvent<RightClickNodeViewArgs> { }
        public struct RightClickNodeViewArgs
        {
            public MapNodeEditView nodeView;
        }

        public struct MapNodeDataUpdate : IEvent<MapNodeDataUpdateArgs> { }
        public struct MapNodeDataUpdateArgs
        {
            public MapNode node;
        }
    }
}
