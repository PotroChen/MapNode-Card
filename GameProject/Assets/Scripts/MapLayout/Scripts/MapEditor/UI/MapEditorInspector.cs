#if UNITY_EDITOR
using GameFramework;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditorInternal;
using UnityEngine;

//TODO 测试加载一个已有的layout,另存为另一个layout时，entity的引用是否还会再老的layout身上
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

        public static MapNode SelectedData
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

        private MapNode selectedData;

        private ReorderableList entityList;

        private void OnGUI()
        {
            if (selectedData == null)
                return;

            DrawMapNode(selectedData);
            //switch (selectedData)
            //{
            //    case MapNode node:
            //        DrawMapNode(node);
            //        break;
            //    default:
            //        EditorGUILayout.HelpBox($"Unsupported type:{selectedData.GetType()}", MessageType.Error);
            //        break;
            //}
        }

        private void DrawMapNode(MapNode node)
        {
            EditorGUI.BeginChangeCheck();
            //Name
            node.Name = EditorGUILayout.TextField("Name:",node.Name);
            //Description
            EditorGUILayout.LabelField("Description:");
            node.Description = EditorGUILayout.TextArea(node.Description);
            //Entities
            DrawEntityList_MapNode(node);

            if (EditorGUI.EndChangeCheck())
            {
                Events.Publish<MapEvents.MapNodeDataUpdate, MapEvents.MapNodeDataUpdateArgs>(new MapEvents.MapNodeDataUpdateArgs() { node = node});
            }
        }

        #region EntityList
        private void DrawEntityList_MapNode(MapNode node)
        {
            if (node.Entities == null)
                node.Entities = new System.Collections.Generic.List<MapEntity>();

            if (entityList == null)
            {
                entityList = new ReorderableList(node.Entities, typeof(MapEntity));
                entityList.onAddCallback += OnClick_AddEntity;
                entityList.onRemoveCallback+= OnClick_RemoveEntity;
                entityList.onSelectCallback += OnClick_SelectEntity;
                entityList.drawElementCallback += DrawEntityListItem;
            }
            entityList.DoLayoutList();
        }

        private void OnClick_AddEntity(ReorderableList reorderableList)
        {
            ShowAddEntityMenu();

        }

        private void OnClick_RemoveEntity(ReorderableList reorderableList)
        {
            if (reorderableList.selectedIndices != null && reorderableList.selectedIndices.Count > 0)
            {
                int count = reorderableList.selectedIndices.Count;
                for (int i = count - 1; i >= 0; i--)
                {
                    var index = reorderableList.selectedIndices[i];
                    var entity = reorderableList.list[index] as MapEntity;
                    reorderableList.list.RemoveAt(index);
                    if (entity != null && AssetDatabase.IsSubAsset(entity))
                        AssetDatabase.RemoveObjectFromAsset(entity);
                }
            }
           
        }

        private void OnClick_SelectEntity(ReorderableList reorderableList)
        {
            if (reorderableList.selectedIndices == null 
                || reorderableList.selectedIndices.Count <= 0)
                return;
            int selectedIndex = reorderableList.selectedIndices[0];
            var mapEntity = reorderableList.list[selectedIndex] as MapEntity;
            Selection.activeObject = mapEntity;
        }

        private void DrawEntityListItem(Rect rect, int index, bool isActive, bool isFocused)
        {
            var entity = selectedData.Entities[index];
            var newName = EditorGUI.TextField(rect, entity.DisplayName);
            if(newName!= entity.DisplayName)
                entity.DisplayName = newName;
        }
        #endregion

        private void ShowAddEntityMenu()
        {
            GenericMenu menu = new GenericMenu();

            menu.AddItem(new GUIContent("Chest Entity"), false, () => {
                var chestEntity =  ScriptableObject.CreateInstance<ChestEntity>();
                AddEntity(chestEntity,selectedData);
                AssetDatabase.Refresh();
            });
            menu.AddItem(new GUIContent("Door Entity"), false, () =>
            {
                var doorEntity = ScriptableObject.CreateInstance<DoorEntity>();
                AddEntity(doorEntity, selectedData);
                AssetDatabase.Refresh();
            });
            menu.ShowAsContext();
        }

        private void AddEntity(MapEntity entity, MapNode node)
        {
            if (node.Layout == null)
            {
                Debug.LogError("node.Layout == null");
                return;
            }
            selectedData.Entities.Add(entity);
            EditorUtility.SetDirty(node.Layout);
        }
    }
}
#endif