using System.Collections;
using System.Collections.Generic; 
using UnityEditor;
using UnityEngine;
using AD.UI;

[CustomEditor(typeof(AudioSourceController)), CanEditMultipleObjects]
public class ASCEditor : Editor
{
    private AudioSourceController that = null;

    private /*List<SourcePair>*/SerializedProperty SourcePairs;

    /*private SourcePair CurrentSourcePair = null;
    private AudioClip CurrentClip = null;
    private int CurrentIndex = 0;
    private bool IsPlay = false;
    private float CurrentTime = 0;*/

    private /*AudioPostMixer*/SerializedProperty _Mixer;

    private /*bool*/SerializedProperty LoopAtAll;
    private /*bool*/SerializedProperty DrawingLine;
    private /*bool*/SerializedProperty Sampling;


    private /*SpectrumLength*/SerializedProperty SpectrumCount;
    private /*float[]*/SerializedProperty samples;
    private /*uint*/SerializedProperty BandCount;
    private /*BufferDecreasingType*/SerializedProperty decreasingType;
    private /*float*/SerializedProperty decreasing;
    private /*float*/SerializedProperty DecreaseAcceleration;
    private /*BufferIncreasingType*/SerializedProperty increasingType;
    private /*float*/SerializedProperty increasing;

    private void OnEnable()
    {
        that = target as AudioSourceController;

        SourcePairs = serializedObject.FindProperty("SourcePairs");

        _Mixer = serializedObject.FindProperty("_Mixer");

        LoopAtAll = serializedObject.FindProperty("LoopAtAll");
        DrawingLine = serializedObject.FindProperty("DrawingLine");
        Sampling = serializedObject.FindProperty("Sampling");

        SpectrumCount = serializedObject.FindProperty("SpectrumCount");
        samples = serializedObject.FindProperty("samples");
        BandCount = serializedObject.FindProperty("BandCount");
        decreasingType = serializedObject.FindProperty("decreasingType");
        decreasing = serializedObject.FindProperty("decreasing");
        DecreaseAcceleration = serializedObject.FindProperty("DecreaseAcceleration");
        increasingType = serializedObject.FindProperty("increasingType");
        increasing = serializedObject.FindProperty("increasing");
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        serializedObject.Update();

        GUI.enabled = false;

        if (Application.isPlaying)
        {
            EditorGUILayout.IntSlider("SerialNumber", that.SerialNumber, 0, AD.UI.ADUI.TotalSerialNumber - 1);
            EditorGUILayout.TextField("ElementName", that.ElementName);
        }

        GUI.enabled = true;

        EditorGUILayout.PropertyField(SourcePairs);

        if (Application.isPlaying)
        {
            GUI.enabled = false;

            EditorGUILayout.IntSlider("CurrentIndex", that.CurrentIndex, 0, that.SourcePairs.Count - 1, null);
            if (that.CurrentClip != null) EditorGUILayout.Slider("CurrentTime", that.CurrentTime, 0, that.CurrentClip.length + 0.2f, null);

            GUI.enabled = true;
            if (GUILayout.Button("Next", new GUILayoutOption[] { })) that.NextPair();
            if (GUILayout.Button("Previous", new GUILayoutOption[] { })) that.PreviousPair();
            if (GUILayout.Button("Random", new GUILayoutOption[] { })) that.RandomPair();
            if (GUILayout.Button("Play", new GUILayoutOption[] { })) that.Play();
            if (GUILayout.Button("Pause", new GUILayoutOption[] { })) that.Pause();
            if (GUILayout.Button("Stop", new GUILayoutOption[] { })) that.Stop();
        }

        EditorGUILayout.PropertyField(_Mixer);
        EditorGUILayout.PropertyField(LoopAtAll);
        EditorGUILayout.PropertyField(Sampling);

        if (Sampling.boolValue)
        {
            EditorGUILayout.PropertyField(DrawingLine);
            EditorGUILayout.PropertyField(SpectrumCount);
            EditorGUILayout.PropertyField(samples);
            EditorGUILayout.PropertyField(BandCount);
            EditorGUILayout.PropertyField(decreasingType);
            EditorGUILayout.PropertyField(decreasing);
            EditorGUILayout.PropertyField(DecreaseAcceleration);
            EditorGUILayout.PropertyField(increasingType);
            EditorGUILayout.PropertyField(increasing);
        }


        serializedObject.ApplyModifiedProperties();
    }
}
