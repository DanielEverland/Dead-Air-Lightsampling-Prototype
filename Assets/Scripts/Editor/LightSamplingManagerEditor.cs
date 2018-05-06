using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LightSamplingManager))]
public class LightSamplingManagerEditor : Editor {

    private SerializedProperty _defaultSpotCookie;

    private void OnEnable()
    {
        _defaultSpotCookie = serializedObject.FindProperty("_defaultSpotCookie");

        PollAutoAssignSpotCookie();
    }
    private void PollAutoAssignSpotCookie()
    {
        SerializedProperty hasBeenAssigned = serializedObject.FindProperty("_hasAssignedSpotCookie");

        if (!hasBeenAssigned.boolValue)
        {
            hasBeenAssigned.boolValue = true;
            AssignDefaultSpotCookie();

            serializedObject.ApplyModifiedProperties();
        }
    }
    public override void OnInspectorGUI()
    {
        GUI.changed = false;

        EditorGUILayout.PropertyField(_defaultSpotCookie);

        if (GUI.changed)
            _defaultSpotCookie.objectReferenceValue = ((Texture2D)_defaultSpotCookie.objectReferenceValue).GetReadableTexture();

        serializedObject.ApplyModifiedProperties();
    }
    private void AssignDefaultSpotCookie()
    {
        _defaultSpotCookie.objectReferenceValue = ((Texture2D)Resources.FindObjectsOfTypeAll(typeof(Texture)).FirstOrDefault(x => x.name == "Soft")).GetReadableTexture();
    }
}
