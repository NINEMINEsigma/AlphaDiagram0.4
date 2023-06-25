using System.Collections;
using System.Collections.Generic;
using AD.UI;
using AD.Utility;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ViewController)), CanEditMultipleObjects]
public class VCEditor : Editor
{
    private ViewController that = null;

    private /*List<SourcePair>*/SerializedProperty SourcePairs;

    private SerializedProperty mCanvasInitializer;

    /*private SourcePair CurrentSourcePair = null;
    private AudioClip CurrentClip = null;
    private int CurrentIndex = 0;
    private bool IsPlay = false;
    private float CurrentTime = 0;*/


    private void OnEnable()
    {
        that = target as ViewController; 

        SourcePairs = serializedObject.FindProperty("SourcePairs");
        mCanvasInitializer = serializedObject.FindProperty("mCanvasInitializer");
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        bool IsSourcePairsChange = false;

        serializedObject.Update();

        if (!Application.isPlaying)
        {
            EditorGUILayout.PropertyField(mCanvasInitializer);
        }

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(SourcePairs);
        IsSourcePairsChange = EditorGUI.EndChangeCheck();

        if (Application.isPlaying)
        {
            GUI.enabled = false;

            EditorGUILayout.IntSlider("CurrentIndex", that.CurrentIndex, 0, that.SourcePairs.Count - 1, null);

            GUI.enabled = true;
            if (GUILayout.Button("Next", new GUILayoutOption[] { })) that.NextPair();
            if (GUILayout.Button("Previous", new GUILayoutOption[] { })) that.PreviousPair();
            if (GUILayout.Button("Random", new GUILayoutOption[] { })) that.RandomPair();
        }

        serializedObject.ApplyModifiedProperties();

        if(IsSourcePairsChange)
        {
            that.ViewImage.sprite = that.CurrentImage; 
        }
    }
}
