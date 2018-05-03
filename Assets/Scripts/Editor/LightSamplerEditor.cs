using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LightSampler))]
[CanEditMultipleObjects()]
public class LightSamplerEditor : Editor {

    private LightSampler Target { get { return (LightSampler)target; } }

    private SerializedProperty _sizeProperty;
    private SerializedProperty _overrideCenter;
    private SerializedProperty _centerProperty;

    private readonly Color _primaryColor = new Color(0, 1, 1, 0.1f);
    private readonly Color _primaryColorOpaque = new Color(0, 1, 1, 1);

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
    private void OnSceneGUI()
    {
        DrawHandles();
    }
    private void DrawHandles()
    {
        Rect rect = new Rect()
        {
            center = Target.Center - Target.Size / 2,
            size = Target.Size,
        };
        
        Handles.DrawSolidRectangleWithOutline(rect, _primaryColor, _primaryColorOpaque);
    }
    private void CreatePropertyReferences()
    {
        _centerProperty = serializedObject.FindProperty("_center");
        _sizeProperty = serializedObject.FindProperty("_size");
        _overrideCenter = serializedObject.FindProperty("_overrideCenter");
    }
}
