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
        private DungeonSceneData sceneData;
        private Dungeon dungeon;
        private DungeonView dungeonView;
        public void OnInit(IGameData data)
        {
            sceneData = data as DungeonSceneData;

            dungeon = Dungeon.Create(sceneData.DungeonMapAssetPath);
            dungeonView = DungeonView.Create(dungeon);
        }

        public void OnLoad()
        {
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
            if (dungeonView != null)
            {
                DungeonView.Destory(dungeonView);
                dungeonView = null;
            }
            if (dungeon != null)
            {
                Dungeon.Destory(dungeon);
                dungeon = null;
            }
        }

        public void OnUpdate()
        {
            Vector2Int moveDirection = Vector2Int.zero;
            if (Input.GetKeyUp(KeyCode.W))
            {
                moveDirection = Vector2Int.up;
            }
            else if (Input.GetKeyUp(KeyCode.A))
            {
                moveDirection = Vector2Int.left;
            }
            else if (Input.GetKeyUp(KeyCode.S))
            {
                moveDirection = Vector2Int.down;
            }
            else if (Input.GetKeyUp(KeyCode.D))
            {
                moveDirection = Vector2Int.right;
            }

            if (moveDirection != Vector2.zero)
            {
                dungeon.PlayerMove(moveDirection);
            }

        }
    }
}
