using Game.DungeonModule;
using GameFramework.UIKit;
using UnityEngine;

namespace Game.UI
{
    public class DungeonPanelData:IData
    {
        public Dungeon dungeon;
    }
    public partial class DungeonPanel:UIPanel
    {
        protected override PanelConfig ConfigData
         => new PanelConfig()
         {
             PrefabPath = "Assets/GameResources/UI/DungeonPanel/DungeonPanel.prefab",
             UILayer = UILayer.Normal
         };

        private Dungeon dungeon;
        private MapNode currentNode;
        protected override void OnInit(IData data)
        {
            base.OnInit(data);
            var uiData = data as DungeonPanelData;
            dungeon = uiData.dungeon;
        }

        protected override void OnLoaded()
        {
            base.OnLoaded();
            dungeon.OnEntered -= OnDungeonEntered;
            dungeon.OnEntered += OnDungeonEntered;
            dungeon.OnPlayerMoved -= OnPlayerMoved;
            dungeon.OnPlayerMoved += OnPlayerMoved;
        }

        protected override void OnShow()
        {
            base.OnShow();
            RefreshNodePanel();
        }

        protected override void OnPurge()
        {
            base.OnPurge();
            dungeon.OnEntered -= OnDungeonEntered;
            dungeon.OnPlayerMoved -= OnPlayerMoved;
        }

        private void OnDungeonEntered(Dungeon dungeon)
        {
            RefreshNodePanel();
        }

        private void OnPlayerMoved(Dungeon dungeon,Vector2Int position)
        {
            RefreshNodePanel();
        }


        private void RefreshNodePanel()
        {
            currentNode = dungeon.GetNodeOfPlayerPosition();
            if (currentNode == null)
            {
                nodeSubPanelRoot.SetActive(false);
                return;
            }
            nodeSubPanelRoot.SetActive(true);
            //描述
            if (string.IsNullOrEmpty(currentNode.Description))
            {
                NodeDescRoot.SetActive(false);
            }
            else
            {
                NodeDescRoot.SetActive(true);
                UIUtils.SetText(txt_NodeDesc, currentNode.Description);
            }
        }
    }
}