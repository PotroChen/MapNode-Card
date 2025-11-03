using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
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
        public void OnInit(IGameData data)
        {
            var convertedData = data as DungeonSceneData;
            dungeonMapAssetOp = Addressables.LoadAssetAsync<MapLayout>(convertedData.DungeonMapAssetPath);
            dungeonMapAssetOp.WaitForCompletion();
        }

        public void OnLoad()
        {

        }

        public void OnEnter()
        {
        }


        public void OnLeave()
        {

        }


        public void OnPurge()
        {

        }

        public void OnUpdate()
        {

        }
    }
}
