using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static Game.ChestEntity;

namespace Game
{
    [CustomEditor(typeof(ChestEntity))]
    public class ChestEntityEditor : MapEntityEditor 
    {
        private SerializedProperty m_ItemSource;
        private SerializedProperty m_ItemKey;
        private SerializedProperty m_ItemID;
        private SerializedProperty m_Count;
        protected override void OnEnable()
        {
            base.OnEnable();
            m_ItemSource = serializedObject.FindProperty("m_ItemSource");
            m_ItemKey = serializedObject.FindProperty("m_ItemKey");
            m_ItemID = serializedObject.FindProperty("m_ItemID");
            m_Count = serializedObject.FindProperty("m_Count");

        }
        protected override void OnInspectorGUI_Internal()
        {
            EditorGUILayout.PropertyField(m_ItemSource);
            if ((ItemSourceType)m_ItemSource.intValue == ItemSourceType.MapLayout)
            {
                EditorGUILayout.PropertyField(m_ItemKey);
            }
            else
            {
                EditorGUILayout.PropertyField(m_ItemID);
            }
            EditorGUILayout.PropertyField(m_Count);
        }
    }
}
