using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static Game.ChestEntity;

namespace Game
{
    [CustomPropertyDrawer(typeof(Game.ChestEntity.ChestItemInfo))]
    public class ChestItemInfoDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var m_ItemSource = property.FindPropertyRelative("m_ItemSource");
            var m_ItemKey = property.FindPropertyRelative("m_ItemKey");
            var m_ItemID = property.FindPropertyRelative("m_ItemID");
            var m_Count = property.FindPropertyRelative("m_Count");

            var rect1 = position;
            rect1.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(rect1,m_ItemSource);

            rect1.y += EditorGUIUtility.singleLineHeight;
            if ((ItemSourceType)m_ItemSource.intValue == ItemSourceType.MapLayout)
            {
                EditorGUI.PropertyField(rect1,m_ItemKey);
            }
            else
            {
                EditorGUI.PropertyField(rect1, m_ItemID);
            }
            rect1.y += EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(rect1, m_Count);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 3;
        }
    }
}
