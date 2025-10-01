using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

namespace Game
{
    public sealed class GameRuntime : MonoBehaviour
    {
        public IGameScene Current { get { return instance.m_Current; } }

        public static void Goto<T>(IGameData data, string sceneName = "") where T : IGameScene, new()
        {
            instance.Goto_Internal<T>(data, sceneName);
        }

        private static GameRuntime instance = null;

        private IGameScene m_Current;
        private IGameScene m_Next;

        private void Awake()
        {
            if (instance != null)
            {
                Debug.LogError("GameRuntime has existed");
                DestroyImmediate(this);
                return;
            }

            instance = this;

            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;

            DontDestroyOnLoad(this);
        }

        private void OnDestroy()
        {
            if (instance == this)
            {
                instance = null;

                SceneManager.sceneLoaded -= OnSceneLoaded;
                SceneManager.sceneUnloaded -= OnSceneUnloaded;
            }
        }

        private void OnSceneUnloaded(Scene unloadedScene)
        {
            if (m_Current != null)
            {
                m_Current.OnPurge();
            }
        }

        private void OnSceneLoaded(Scene loadedScene, LoadSceneMode loadSceneMode)
        {
            m_Current = m_Next;
            if (m_Current != null)
            {
                m_Current.OnLoad();
                m_Current.OnEnter();
            }
            m_Next = null;
        }

        private void Goto_Internal<T>(IGameData data, string sceneName) where T : IGameScene, new()
        {
            if (m_Next != null)
            {
                Debug.LogError("Can not goto a new GameScene when is transiting!");
                return;
            }

            if (m_Current != null)
            {
                m_Current.OnLeave();
            }

            m_Next = new T();
            m_Next.OnInit(data);

            if (!string.IsNullOrEmpty(sceneName))
            {
                Addressables.LoadSceneAsync(sceneName);
            }
            else
            {
                if (m_Current != null)
                {
                    m_Current.OnPurge();
                }
                m_Current = m_Next;

                if (m_Current != null)
                {
                    m_Current.OnLoad();
                    m_Current.OnEnter();
                }

                m_Next = null;
            }
        }

    }
}
