namespace Game.UI
{
    using GameFramework.UIKit;
    using UnityEngine;
    using System;
    using Game.DungeonModule;

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
        }

        protected override void OnShow()
        {
            menuRoot.FillList(entity.Items.Length, RefreshMenuItem);
        }

        protected override void OnHide() 
        {
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
                string text = $"{itemDefine.Name} X {count}";
                UIUtils.SetText(menuItemGo, "Text", text);
                toggle.Click.AddListener(() => 
                {
                    entity.TryTakeItem(index);
                    dungeon.Player.GainLocalItem(itemData.ItemKey, itemData.Count);
                });

            }
        }
    }
}
