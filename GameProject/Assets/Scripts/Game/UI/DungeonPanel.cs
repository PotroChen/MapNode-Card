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
            go_FirstLevelMenu.SetActive(false);
            go_SecondLevelMenu.SetActive(false);
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
            //Entities
            RefreshFirstLevelMenu(true);
        }

        #region 一级菜单(Entities)
        int selectedIndex_FirstMenu;
        private void RefreshFirstLevelMenu(bool resetSelected = false)
        {
            if (currentNode.Entities == null || currentNode.Entities.Count <= 0)
            {
                go_FirstLevelMenu.SetActive(false);
                return;
            }
            go_FirstLevelMenu.SetActive(true);
            if (resetSelected)
                selectedIndex_FirstMenu = 0;
            var recycleList = go_FirstLevelMenu.GetComponent<RecycleList>();
            recycleList.FillList(currentNode.Entities.Count, RefreshFirstLevelMenuItem);
        }

        private void RefreshFirstLevelMenuItem(int index,GameObject itemGO)
        {
            var entity = currentNode.Entities[index];
            if (entity == null)
            {
                UIUtils.SetText(itemGO,"Text","NULL");
                return;
            }

            string entityName = string.IsNullOrEmpty(entity.name) ? "EmptyName" : entity.name;
            UIUtils.SetText(itemGO, "Text", entityName);

            bool selected = selectedIndex_FirstMenu == index;
            UIUtils.SetActive(itemGO, "Selected", selected);


            UIUtils.SetText(itemGO, "InteractiveTip/Label", entity.GetInteractionName()+"(F)");
        }
        #endregion

        #region 二级菜单

        #endregion

    }
}