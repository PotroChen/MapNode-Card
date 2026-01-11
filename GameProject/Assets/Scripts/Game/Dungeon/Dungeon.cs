using System;
using System.Numerics;
using UnityEngine;
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
        public DungeonPlayer Player => player;
        #region Events
        public event Action<Dungeon> OnEntered;
        public event Action<Dungeon> OnExited;
        public event Action<Dungeon> OnPlayerMoveFailed;
        public event Action<Dungeon, Vector2Int> OnPlayerMoved;
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
            player.Position = map.StartNode.Position;
            OnEntered?.Invoke(this);
        }

        private void OnExit()
        {
            OnExited?.Invoke(this);
        }

        public void PlayerMove(Vector2Int moveDirection)
        {
            Vector2Int nextPosition = player.Position + moveDirection;
            MapNode nextNode = map.GetNode(nextPosition);
            if (nextNode == null)
            {
                OnPlayerMoveFailed?.Invoke(this);
            }
            else
            {
                player.Position = nextPosition;
                OnPlayerMoved?.Invoke(this,nextPosition);
            }
        }
    }

}