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
    }
}
