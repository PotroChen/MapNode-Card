using GameFramework.UIKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public partial class DungeonPanel 
    {
        protected override PanelConfig ConfigData
         => new PanelConfig()
         {
             PrefabPath = "Assets/GameResources/UI/DungeonPanel/DungeonPanel.prefab",
             UILayer = UILayer.Normal
         };
    }

}