using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Game
{
    public class MapEntityEditor : Editor
    {
        SerializedProperty nameProp;
        protected virtual void OnEnable()
        {
            nameProp = serializedObject.FindProperty("m_Name");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(nameProp);
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
            base.OnInspectorGUI();
        }
    }

}