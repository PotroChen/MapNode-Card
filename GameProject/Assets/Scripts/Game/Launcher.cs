using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game
{
    public class Launcher : MonoBehaviour
    {
        public IEnumerator Start()
        {
            DontDestroyOnLoad(this);

            yield return Addressables.InitializeAsync();
            //GameDataRuntime.Instance.Init(true);

            //GameRuntime.Goto<Login>(null, "Login");
        }
    }
}
