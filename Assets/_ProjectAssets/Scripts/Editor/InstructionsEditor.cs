using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(Instructions))]
public class InstructionsEditor : Editor
{
    private ReorderableList reorderableList;

    private void OnEnable()
    {
        reorderableList = new ReorderableList(
            serializedObject,
            serializedObject.FindProperty("instructions"),
            true,  // draggable
            true,  // display header
            true,  // display add button
            true   // display remove button
        );

        reorderableList.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "Instructions List");
        };

        reorderableList.elementHeightCallback = (int index) =>
        {
            var element = reorderableList.serializedProperty.GetArrayElementAtIndex(index);
            var textProp = element.FindPropertyRelative("instructionText");
            var lines = Mathf.Max(3, textProp.stringValue.Split('\n').Length);
            return EditorGUIUtility.singleLineHeight * (lines + 3) + 16;
        };

        reorderableList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            var element = reorderableList.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;

            // Name field
            EditorGUI.PropertyField(
                new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("name"),
                new GUIContent("Name")
            );

            // Sprite field
            EditorGUI.PropertyField(
                new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight + 2, rect.width, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("sprite"),
                new GUIContent("Sprite")
            );

            // AudioClip field
            EditorGUI.PropertyField(
                new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight * 2 + 4, rect.width, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("audioClip"),
                new GUIContent("Audio Clip")
            );

            // Instruction text area
            var textRect = new Rect(
                rect.x,
                rect.y + EditorGUIUtility.singleLineHeight * 2 + 4,
                rect.width,
                rect.height - EditorGUIUtility.singleLineHeight * 2 - 8
            );
            EditorGUI.LabelField(
                new Rect(textRect.x, textRect.y, textRect.width, EditorGUIUtility.singleLineHeight),
                "Instruction Text"
            );
            textRect.y += EditorGUIUtility.singleLineHeight;
            textRect.height -= EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(
                textRect,
                element.FindPropertyRelative("instructionText"),
                GUIContent.none
            );
        };
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        reorderableList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }
}