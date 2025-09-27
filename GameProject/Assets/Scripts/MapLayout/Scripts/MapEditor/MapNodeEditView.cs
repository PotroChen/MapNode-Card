using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using GameFramework;
using static Game.MapEvents;

namespace Game
{
    public class MapNodeEditView : MonoBehaviour,IPointerClickHandler
    {
        public MapLayoutEditView LayoutView { get; set; }
        public MapNode Data { get; set; }
        public RectTransform Content;
        public Text Name;

        public void Start()
        {
            Events.Unsubscribe<MapEvents.MapNodeDataUpdate, MapEvents.MapNodeDataUpdateArgs>(OnDataUpdate);
            Events.Subscribe<MapEvents.MapNodeDataUpdate, MapEvents.MapNodeDataUpdateArgs>(OnDataUpdate);
        }

        public void OnDestroy()
        {
            Events.Unsubscribe<MapEvents.MapNodeDataUpdate, MapEvents.MapNodeDataUpdateArgs>(OnDataUpdate);
        }

        private void OnDataUpdate(MapEvents.MapNodeDataUpdateArgs args)
        {
            if (args.node == Data)
                Refresh();
        }

        public void Refresh()
        {
            if (Data == null)
                return;

            Name.text = Data.Name;
        }
        #region Interactive
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                var eventArgs = new RightClickNodeViewArgs();
                eventArgs.nodeView = this;
                Events.Publish<RightClickNodeView, RightClickNodeViewArgs>(eventArgs);
            }

            if (eventData.button == PointerEventData.InputButton.Left)
            {
                MapEditorInspector.SelectedData = Data;
            }
        }
        #endregion
    }
}
