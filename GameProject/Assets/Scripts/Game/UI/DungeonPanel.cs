using Game.DungeonModule;
using GameFramework.UIKit;
using UnityEngine;
using System;
using GameFramework;
using Game.Inventory;
using System.Collections.Generic;
namespace Game.UI
{
    public class DungeonPanelData:IData
    {
        public Dungeon dungeon;
    }
    public partial class DungeonPanel : UIPanel
    {
        protected override PanelConfig ConfigData
         => new PanelConfig()
         {
             PrefabPath = "Assets/GameResources/UI/DungeonPanel/DungeonPanel.prefab",
             UILayer = UILayer.Normal,
             IsPermanent = true,
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

            Events.Unsubscribe<InventoryEvents.Changed>(OnInvevntoryChanged);
            Events.Subscribe<InventoryEvents.Changed>(OnInvevntoryChanged);
        }

        protected override void OnShow()
        {
            base.OnShow();
            ShowNodePanel();
            go_FirstLevelMenu.SetActive(false);
            RefreshNodePanel();
        }

        protected override void OnPurge()
        {
            base.OnPurge();
            dungeon.OnEntered -= OnDungeonEntered;
            dungeon.OnPlayerMoved -= OnPlayerMoved;
            Events.Unsubscribe<InventoryEvents.Changed>(OnInvevntoryChanged);
        }

        private void OnDungeonEntered(Dungeon dungeon)
        {
            RefreshNodePanel();
        }

        private void OnPlayerMoved(Dungeon dungeon, Vector2Int position)
        {
            RefreshNodePanel();
        }

        #region NodePanel
        private void ShowNodePanel()
        {
            nodeSubPanelRoot.SetActive(true);
        }
        private void HideNodePanel()
        {
            nodeSubPanelRoot.SetActive(false);
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
        #endregion


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

        private void RefreshFirstLevelMenuItem(int index, GameObject itemGO)
        {
            var entity = currentNode.Entities[index];
            if (entity == null)
            {
                UIUtils.SetText(itemGO, "Text", "NULL");
                return;
            }

            string entityName = string.IsNullOrEmpty(entity.name) ? "EmptyName" : entity.name;
            UIUtils.SetText(itemGO, "Text", entityName);

            bool selected = selectedIndex_FirstMenu == index;
            UIUtils.SetActive(itemGO, "Selected", selected);

            var toggle = itemGO.GetComponentInChildren<ExToggle>();
            toggle.Click.RemoveAllListeners();
            toggle.Click.AddListener(() =>
            {
                switch (entity)
                {
                    case ChestEntity chest:
                        {
                            HideNodePanel();

                            var uiData = new ChestEntityPanel.Data();
                            uiData.dungon = dungeon;
                            uiData.entity = entity as ChestEntity;
                            UIManager.Goto<ChestEntityPanel>(uiData);
                            break;
                        }
                    case DoorEntity doorEntity:
                        {
                            HideNodePanel();

                            var itemDefine = doorEntity.GetItemDefine(doorEntity.ItemToUnlock);
                            var uiData = new CommanMapEntityPanel.Data();
                            uiData.OperateItemDatas = new List<MapEntityOperateViewData>();
                            MapEntityOperateViewData op = new MapEntityOperateViewData();
                            op.Text = $"开锁(需要{itemDefine.Name} x {1})";
                            op.onClick = () =>
                            {
                                doorEntity.TryUnlock(dungeon.Player);
                            };
                            break;
                        }
                    default:
                        throw new NotImplementedException();
                }


            });
        }

        #endregion

        #region InventoryInfo
        private struct InventoryInfo
        {
            public string Name;
            public uint Count;
        }

        List<InventoryInfo> inventoryInfoes = new List<InventoryInfo>();
        private void OnInvevntoryChanged()
        {
            inventoryInfoes.Clear();

            if (dungeon.Player.dungeonInventoryData != null)
            {
                foreach (var kvp in dungeon.Player.dungeonInventoryData)
                {
                    var itemDefine = dungeon.Map.GetItemDefine(kvp.Key);
                    if (itemDefine == null)
                        continue;

                    InventoryInfo info = new InventoryInfo();
                    info.Name = itemDefine.Name;
                    info.Count = kvp.Value.Count;
                    inventoryInfoes.Add(info);
                }
            }
            List_InventoryInfoes.FillList(inventoryInfoes.Count, OnRefreshInvetoryInfo);
        }

        private void OnRefreshInvetoryInfo(int index,GameObject itemGo)
        {
            string text = $"{inventoryInfoes[index].Name} {inventoryInfoes[index].Count}";
            UIUtils.SetText(itemGo, text);

        }
        #endregion
    }
}