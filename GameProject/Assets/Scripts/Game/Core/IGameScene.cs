using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameScene
{
    void OnInit(IGameData data);

    void OnLoad();

    void OnEnter();

    void OnLeave();

    void OnPurge();

    void OnUpdate();
}
