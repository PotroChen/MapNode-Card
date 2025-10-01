using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public struct SceneData
    {
        public Scene Scene;
    }

    public class SceneSystem : MonoBehaviour
    {
        public static Action<SceneData> SceneLoaded;
        public static Action<SceneData> SceneUnloaded;

        private static SceneSystem instance;

        private static SceneData baseScene;
        public static SceneData BaseScene
        {
            get { return baseScene; }
        }

        private static List<SceneData> additiveScenes = new List<SceneData>();
        public static SceneData[] AdditiveScenes
        {
            get { return additiveScenes.ToArray(); }
        }


        private void Awake()
        {
            if (instance != null)
            {
                Debug.LogError("SceneSystem has existed");
                DestroyImmediate(this);
                return;
            }

            instance = this;

            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
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

        private void OnSceneLoaded(Scene loadedScene, LoadSceneMode loadSceneMode)
        {
            SceneLoaded?.Invoke(BaseScene);
        }

        private void OnSceneUnloaded(Scene unloadedScene)
        {
            SceneUnloaded?.Invoke(BaseScene);
        }


    }
}
