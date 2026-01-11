using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.DungeonModule
{
    public class DungeonView
    {
        #region Static Function
        public static DungeonView Create(Dungeon dungeon)
        {
            var view = new DungeonView(dungeon);
            view.OnCreate();
            return view;
        }

        public static void Destory(DungeonView dungeonView)
        {
            dungeonView.OnDestory();
        }
        #endregion

        public static Color NormalNodeColor = Color.white;
        public static Color PlayerNodeColor = Color.green;

        private Dungeon dungeon;
        private MapLayoutView mapView;
        public DungeonView(Dungeon dungeon) 
        {
            this.dungeon = dungeon;
        }

        private void OnCreate()
        {
            dungeon.OnEntered -= OnEntered;
            dungeon.OnEntered += OnEntered;
            dungeon.OnExited -= OnExited;
            dungeon.OnExited += OnExited;
            dungeon.OnPlayerMoved -= OnPlayerMoved;
            dungeon.OnPlayerMoved += OnPlayerMoved;
            dungeon.OnPlayerMoveFailed -= OnPlayerMoveFailed;
            dungeon.OnPlayerMoveFailed += OnPlayerMoveFailed;
        }
        private void OnDestory()
        {
            dungeon.OnEntered -= OnEntered;
            dungeon.OnExited -= OnExited;
            dungeon.OnPlayerMoved -= OnPlayerMoved;
            dungeon.OnPlayerMoveFailed -= OnPlayerMoveFailed;

        }

        private void OnEntered(Dungeon dungeon)
        {
            mapView = GameObject.FindObjectOfType<MapLayoutView>();
            mapView.data = dungeon.Map;
            mapView.Refresh();
            RefreshPlayerPosition();
        }

        private void OnExited(Dungeon dungeon)
        {

        }

        private void OnPlayerMoved(Dungeon dungeon, Vector2Int position)
        {
            RefreshPlayerPosition();
        }

        private void OnPlayerMoveFailed(Dungeon dungeon)
        {
            Debug.Log("OnPlayerMoveFailed");
        }

        private MapNodeView lastPlayerNodeView = null;
        public void RefreshPlayerPosition()
        {
            var node = dungeon.GetNodeOfPlayerPosition();
            if (lastPlayerNodeView != null)
            {
                lastPlayerNodeView.SetColor(NormalNodeColor);
            }
            if (node != null)
            {
                var nodeView = mapView.GetNodeView(node);
                nodeView.SetColor(PlayerNodeColor);

                lastPlayerNodeView = nodeView;
            }
        }


    }
}
