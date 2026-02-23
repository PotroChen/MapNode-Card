using GameFramework.UIKit;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class MapEntityOperateViewData
    {
        public string Text;
        public Action onClick;
    }
    public partial class CommanMapEntityPanel
    {
        protected override PanelConfig ConfigData => new PanelConfig
        {
            PrefabPath = "Assets/GameResources/UI/CommanMapEntityPanel/CommanMapEntityPanel.prefab",
            UILayer = UILayer.Normal,
        };

        public class Data : IData
        {
            public List<MapEntityOperateViewData> OperateItemDatas;
        }

        private List<MapEntityOperateViewData> opItems = new List<MapEntityOperateViewData>();
        protected override void OnInit(IData data)
        {
            opItems.Clear();
            var convertedData = data as CommanMapEntityPanel.Data;
            if (convertedData?.OperateItemDatas != null)
            {
                opItems.AddRange(convertedData.OperateItemDatas);
            }
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

        private void Refresh()
        {
            menuRoot.FillList(opItems.Count, RefreshMenuItem);
            var rectTransform = menuRoot.GetComponent<RectTransform>();
            LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
        }

        private void RefreshMenuItem(int index, GameObject itemGo)
        {
            var toggle = itemGo.GetComponentInChildren<ExToggle>();
            toggle.Click.RemoveAllListeners();

            var opItemData = opItems[index];
            UIUtils.SetText(itemGo, opItemData.Text);
            toggle.Click.AddListener(() =>
            {
                opItemData.onClick?.Invoke();
                Refresh();
            });
        }


    }

}