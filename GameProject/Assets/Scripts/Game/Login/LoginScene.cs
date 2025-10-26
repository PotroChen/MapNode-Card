using Game.UI;
using GameFramework.UIKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class LoginScene : IGameScene
    {
        public void OnInit(IGameData data)
        {

        }

        public void OnLoad()
        {

        }

        public void OnEnter()
        {
            UIManager.Goto<LoginPanel>();
        }

        public void OnUpdate()
        {

        }

        public void OnLeave()
        {

        }


        public void OnPurge()
        {

        }

    }
}
