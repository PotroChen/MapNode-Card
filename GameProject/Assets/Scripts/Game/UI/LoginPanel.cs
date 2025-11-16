using GameFramework.UIKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public partial class LoginPanel : UIPanel
    {
        protected override PanelConfig ConfigData
            => new PanelConfig()
            {
                PrefabPath = "Assets/GameResources/UI/LoginPanel/LoginPanel.prefab",
                UILayer = UILayer.Normal
            };

        protected override void OnLoaded()
        {
            base.OnLoaded();
            newGameBtn.onClick.RemoveListener(NewGameBtn_OnClick);
            newGameBtn.onClick.AddListener(NewGameBtn_OnClick);
        }

        protected override void OnPurge()
        {
            base.OnPurge();
            newGameBtn.onClick.RemoveListener(NewGameBtn_OnClick);
        }

        #region UI Callback
        private void NewGameBtn_OnClick()
        {
            DungeonSceneData sceneData = new DungeonSceneData();
            sceneData.DungeonMapAssetPath = "Assets/GameResources/MapLayoutAssets/DemoDungeon.asset";
            GameRuntime.Goto<DungeonScene>(sceneData, DungeonScene.SceneAsset);
        }
        #endregion
    }
}
