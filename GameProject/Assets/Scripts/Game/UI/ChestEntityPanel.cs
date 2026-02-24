namespace Game.UI
{
    using Game.DungeonModule;
    using GameFramework.UIKit;
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public partial class ChestEntityPanel : UIPanel
    {
        protected override PanelConfig ConfigData => new PanelConfig 
        {
            PrefabPath = "Assets/GameResources/UI/ChestEntityPanel/ChestEntityPanel.prefab",
            UILayer = UILayer.Normal,
        };
        public class Data: IData
        {
            public Dungeon dungon;
            public ChestEntity entity;
        }

        private ChestEntity entity;
        private Dungeon dungeon;
        protected override void OnInit(IData data)
        {
            var convertedData = data as ChestEntityPanel.Data;
            entity = convertedData.entity;
            dungeon = convertedData.dungon;
            dungeon.OnPlayerMoved -= OnPlayerMoved;
            dungeon.OnPlayerMoved += OnPlayerMoved;
        }

        protected override void OnShow()
        {
            toggle_GoBack.Click.RemoveAllListeners();
            toggle_GoBack.Click.AddListener(() => { UIManager.Goback(); });
            Refresh();
        }

        protected override void OnHide() 
        {
        }

        protected override void OnPurge()
        {
            dungeon.OnPlayerMoved -= OnPlayerMoved;
        }

        private void OnPlayerMoved(Dungeon dungeon, Vector2Int position)
        {
            UIManager.Goback();
        }

        private void Refresh()
        {
            menuRoot.FillList(entity.Items.Length, RefreshMenuItem);
            var rectTransform = menuRoot.GetComponent<RectTransform>();
            LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
        }

        private void RefreshMenuItem(int index,GameObject menuItemGo)
        {
            var itemData = entity.Items[index];
            uint count = itemData.Count;

            var toggle = menuItemGo.GetComponentInChildren<ExToggle>();
            toggle.Click.RemoveAllListeners();
            if (itemData.ItemSource == ChestEntity.ItemSourceType.Table)
            {
                UIUtils.SetText(menuItemGo, "Text", "暂不支持Table类型物品");
            }
            else
            {
                
                var itemDefine = entity.Owner.Layout.GetItemDefine(itemData.ItemKey);
                bool isTaken = entity.IsItemTaken(index);
                string text = $"{itemDefine.Name} X {count}";
                UIUtils.SetText(menuItemGo, "Text", text);
                toggle.interactable = !isTaken;
                toggle.Click.AddListener(() => 
                {
                    entity.TryTakeItem(index);
                    dungeon.Player.GainLocalItem(itemData.ItemKey, itemData.Count);
                    Refresh();
                });

            }
        }
    }
}
