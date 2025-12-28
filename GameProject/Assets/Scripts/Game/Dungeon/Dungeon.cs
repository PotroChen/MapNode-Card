using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Game.DungeonModule
{
    public class Dungeon 
    {
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

        }

        private void OnExit()
        {
            
        }
    }

}