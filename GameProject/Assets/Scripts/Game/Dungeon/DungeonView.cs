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
        }
        private void OnDestory()
        {
            dungeon.OnEntered -= OnEntered;
            dungeon.OnExited -= OnExited;
        }

        private void OnEntered(Dungeon dungeon)
        {
            mapView = GameObject.FindObjectOfType<MapLayoutView>();
            mapView.data = dungeon.Map;
            mapView.Refresh();
        }

        private void OnExited(Dungeon dungeon)
        {

        }
    }
}
