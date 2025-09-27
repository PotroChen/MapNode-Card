#if UNITY_EDITOR
using GameFramework;
using UnityEditor;


namespace Game
{
    public class MapEditorInspector : EditorWindow
    {
        public static MapEditorInspector OpenWindow()
        {
            var window = EditorWindow.GetWindow<MapEditorInspector>();
            window.Show();
            return window;
        }

        public static object SelectedData
        {
            get
            {
                var window  = EditorWindow.GetWindow<MapEditorInspector>();
                if (window != null)
                    return window.selectedData;
                return null;
            }
            set
            {
                var window = EditorWindow.GetWindow<MapEditorInspector>();
                if (window != null)
                     window.selectedData = value;
            }
        }

        private object selectedData;

        private void OnGUI()
        {
            if (selectedData == null)
                return;

            switch (selectedData)
            {
                case MapNode node:
                    DrawMapNode(node);
                    break;
                default:
                    EditorGUILayout.HelpBox($"Unsupported type:{selectedData.GetType()}", MessageType.Error);
                    break;
            }
        }

        private void DrawMapNode(MapNode node)
        {
            EditorGUI.BeginChangeCheck();
            node.Name = EditorGUILayout.TextField("Name:",node.Name);
            if (EditorGUI.EndChangeCheck())
            {
                Events.Publish<MapEvents.MapNodeDataUpdate, MapEvents.MapNodeDataUpdateArgs>(new MapEvents.MapNodeDataUpdateArgs() { node = node});
            }
        }
    }
}
#endif