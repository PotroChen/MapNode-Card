using Game.DungeonModule;
using Game.UI;
using GameFramework.UIKit;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Game
{
    public class DungeonSceneData: IGameData
    {
        public string DungeonMapAssetPath;
    }
    public class DungeonScene : IGameScene
    {
        public const string SceneAsset = "Assets/GameResources/Scenes/DungeonScene.unity";
        private AsyncOperationHandle<MapLayout> dungeonMapAssetOp;
        private Dungeon dungeon;
        public void OnInit(IGameData data)
        {
            dungeon = Dungeon.Create(SceneAsset);
        }

        public void OnLoad()
        {
            var layoutView = GameObject.FindObjectOfType<MapLayoutView>();
            layoutView.data = dungeonMapAssetOp.Result;
            layoutView.data.Init();
            layoutView.Refresh();

            UIManager.Goto<DungeonPanel>();
        }

        public void OnEnter()
        {
            dungeon.Enter();
        }


        public void OnLeave()
        {
            dungeon.Exit();
        }


        public void OnPurge()
        {
            if (dungeon != null)
            {
                Dungeon.Destory(dungeon);
                dungeon = null;
            }
        }

        public void OnUpdate()
        {

        }
    }
}
