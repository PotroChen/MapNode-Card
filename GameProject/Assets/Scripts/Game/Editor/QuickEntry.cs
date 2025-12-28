using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEditor;

public static class QuickEntry
{
    [MenuItem("Game/快速入口/Launcher")]
    public static void EnterLauncher()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/Launcher.unity");
    }

    [MenuItem("Game/快速入口/MapEditor")]
    public static void EnterMapEditor()
    {
        EditorSceneManager.OpenScene("Assets/Scripts/MapLayout/Editor/Scene/MapEditor.unity");
    }

}
