using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LightSampler))]
[CanEditMultipleObjects()]
public class LightSamplerEditor : Editor {

    private SerializedProperty _sizeProperty;
    private SerializedProperty _overrideCenter;
    private SerializedProperty _centerProperty;

    private const string TOOLTIP_OVERRIDE_CENTER = "Use a value rather than the position of the transform";
    
    public override void OnInspectorGUI()
    {
        DrawBoundsProperties();

        serializedObject.ApplyModifiedProperties();
    }
    private void DrawBoundsProperties()
    {
        EditorGUILayout.PropertyField(_overrideCenter);
        EditorGUILayout.PropertyField(_sizeProperty);

        if (_overrideCenter.boolValue)
        {
            EditorGUI.indentLevel++;

            EditorGUILayout.PropertyField(_centerProperty, new GUIContent("Center", TOOLTIP_OVERRIDE_CENTER));

            EditorGUI.indentLevel--;
        }
    }
    private void OnEnable()
    {
        CreatePropertyReferences();
    }
    private void CreatePropertyReferences()
    {
        _centerProperty = serializedObject.FindProperty("_center");
        _sizeProperty = serializedObject.FindProperty("_size");
        _overrideCenter = serializedObject.FindProperty("_overrideCenter");
    }
}
