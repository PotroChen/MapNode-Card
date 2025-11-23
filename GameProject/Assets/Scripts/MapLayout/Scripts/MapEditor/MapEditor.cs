using GameFramework;
using UnityEditor;
using UnityEngine;

namespace Game
{
    //TODO can not delete startNode
    public class MapEditor : MonoBehaviour
    {
        public string MapLayoutFolder;
        public MapLayoutEditView LayoutView;
        public void Start()
        {
            if (string.IsNullOrEmpty(MapLayoutFolder))
            {
                Debug.LogError("MapLayoutFolder is empty");
                return;
            }
            LayoutView.mapEditor = this;
            Events.Unsubscribe<MapEvents.RightClickNodeView, MapEvents.RightClickNodeViewArgs>(OnRightClickNodeView);
            Events.Subscribe<MapEvents.RightClickNodeView, MapEvents.RightClickNodeViewArgs>(OnRightClickNodeView);
        }

        public void OnDestroy()
        {
            Events.Unsubscribe<MapEvents.RightClickNodeView, MapEvents.RightClickNodeViewArgs>(OnRightClickNodeView);
        }

        MapLayout currentMapLayout = null;
        public void NewMapLayout()
        {
            currentMapLayout = ScriptableObject.CreateInstance<MapLayout>();

            if (currentMapLayout.StartNode == null)
            {
                currentMapLayout.StartNode = currentMapLayout.CreateNode("StartNode",Vector2Int.zero);
            }
            currentMapLayout.Init();
            LayoutView.Data = currentMapLayout;
            LayoutView.Refresh();
        }

        public void LoadMapLayout(MapLayout mapLayout)
        {
            currentMapLayout  = mapLayout;
            if (currentMapLayout.StartNode == null)
            {
                currentMapLayout.StartNode = currentMapLayout.CreateNode("StartNode", Vector2Int.zero);
            }
            currentMapLayout.Init();
            LayoutView.Data = currentMapLayout;
            LayoutView.Refresh();
        }

        #region UI
        [Header("UI")]
        public Transform mainPanel;
        public Transform toolPanel;
        public NodeEditMenu nodeEditMenu;
        public void OnNewBtn_Click()
        {
            mainPanel.gameObject.SetActive(false);
            toolPanel.gameObject.SetActive(true);
            NewMapLayout();
        }

        public void OnLoadBtn_Click()
        {
            string path = UnityEditor.EditorUtility.OpenFilePanel("加载MapLayout", "Assets/MapLayoutAsset", "asset");
            if (!string.IsNullOrEmpty(path))
            {
                path = "Assets" + path.Replace(Application.dataPath, "");
                var mapLayout =  UnityEditor.AssetDatabase.LoadAssetAtPath<MapLayout>(path);
                var instance = Instantiate_Editor(mapLayout); 
                instance.name = instance.name.Replace("(Clone)","");
                LoadMapLayout(instance);

                mainPanel.gameObject.SetActive(false);
                toolPanel.gameObject.SetActive(true);
            }
        }

        public void OnSaveBtn_Click()
        {
            string path = UnityEditor.EditorUtility.SaveFilePanel("保存MapLayout", "Assets/GameResources/MapLayoutAssets", "NewMapLayout", "asset");
            if (!string.IsNullOrEmpty(path))
            {
                path = "Assets" + path.Replace(Application.dataPath, "");
                SaveLayout(LayoutView.Data, path);
            }
        }

        public void OnRightClickNodeView(MapEvents.RightClickNodeViewArgs eventArgs)
        {
            nodeEditMenu.Show(eventArgs.nodeView);
        }


        public static MapLayout Instantiate_Editor(MapLayout layout)
        {
            if(layout == null)
                return null;
            var instance = ScriptableObject.Instantiate(layout);
            instance.name = instance.name.Replace("(Clone)", "");

            //Entity要用cloned出来的，不然还是引用原先asset中的Entity
            if (instance.AllNodes != null)
            {
                foreach (var node in instance.AllNodes)
                {
                    if (node == null || node.Entities == null)
                        continue;
                    for (int i = 0; i < node.Entities.Count; i++)
                    {
                        var entity = node.Entities[i];
                        if (entity == null) continue;
                        if (AssetDatabase.IsSubAsset(entity))
                        {
                            var clonedEntity = ScriptableObject.Instantiate(entity);
                            clonedEntity.name = clonedEntity.name.Replace("(Clone)", "");
                            node.Entities[i] = clonedEntity;
                        }
                    }
                }
            }
            return instance;
        }

        public void SaveLayout(MapLayout data,string path)
        {
            var oldAsset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
            if (oldAsset != null)
                AssetDatabase.DeleteAsset(path);

            UnityEditor.AssetDatabase.CreateAsset(data, path);
            var allEntities = LayoutView.Data.GetAllEntities();
            foreach (var entity in allEntities)
            {
                AssetDatabase.AddObjectToAsset(entity, path);
            }
            AssetDatabase.Refresh();
        }



        #endregion

    }
}
