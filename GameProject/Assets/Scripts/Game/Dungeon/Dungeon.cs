using System;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Game.DungeonModule
{
    public class Dungeon 
    {
        #region Static Function
        public static Dungeon Create(string mapPath)
        {
            Dungeon dungeon = new Dungeon();
            dungeon.OnCreate(mapPath);
            return dungeon;
        }

        public static void Destory(Dungeon dungeon)
        {
            dungeon.OnDestroy();
        }
        #endregion

        public MapLayout Map => map;

        #region Events
        public event Action<Dungeon> OnEntered;
        public event Action<Dungeon> OnExited;
        #endregion

        private AsyncOperationHandle<MapLayout> dungeonMapAssetOp;
        private MapLayout map;
        private DungeonPlayer player;

        public void Enter()
        {
            OnEnter();
        }

        public void Exit()
        {
            OnExit();
        }

        private void OnCreate(string mapPath)
        {
            dungeonMapAssetOp = Addressables.LoadAssetAsync<MapLayout>(mapPath);
            dungeonMapAssetOp.WaitForCompletion();
            map = dungeonMapAssetOp.Result;
            map.Init();

            player = new DungeonPlayer();
        }

        private void OnDestroy()
        {
            dungeonMapAssetOp.Release();
        }

        private void OnEnter()
        {
            OnEntered?.Invoke(this);
        }

        private void OnExit()
        {
            OnExited?.Invoke(this);
        }
    }

}