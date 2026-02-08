namespace Game.UI
{
    using GameFramework.UIKit;
    using UnityEngine;

    public partial class ChestEntityPanel : UIPanel
    {
        public class Data
        {
            public ChestEntity entity;
        }

        private ChestEntity entity;
        protected override void OnInit(IData data)
        {
            var convertedData = data as ChestEntityPanel.Data;
            entity = convertedData.entity;
        }

        protected override void OnShow()
        {

        }
    }
}
