using System.Collections;
using System.Collections.Generic; 
using UnityEditor;
using UnityEngine; 

[CustomEditor(typeof(AD.UI.Text)), CanEditMultipleObjects]
public class TextEdior : Editor
{
    private AD.UI.Text that = null;

    private void OnEnable()
    { 
        that = target as AD.UI.Text;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        GUI.enabled = false;

        if (Application.isPlaying)
        {
            EditorGUILayout.IntSlider("SerialNumber", that.SerialNumber, 0, AD.UI.ADUI.TotalSerialNumber - 1);
            EditorGUILayout.TextField("ElementName", that.ElementName);
            EditorGUILayout.TextField("ElementArea", that.ElementArea);
        }

        GUI.enabled = true;

        serializedObject.ApplyModifiedProperties();
    }
}


[CustomEditor(typeof(AD.UI.Slider)), CanEditMultipleObjects]
public class SliderEdior : Editor
{
    private AD.UI.Slider that = null;

    private SerializedProperty background = null;
     private SerializedProperty handle = null;
    private SerializedProperty fill = null;

    private void OnEnable()
    {
        that = target as AD.UI.Slider;

        background = serializedObject.FindProperty("background");
        handle = serializedObject.FindProperty("handle");
        fill = serializedObject.FindProperty("fill");
    }

    public override void OnInspectorGUI()
    {
        Object @object = null;
        Sprite sprite = null;

        serializedObject.Update();

        GUI.enabled = false;

        if (Application.isPlaying)
        {
            EditorGUILayout.IntSlider("SerialNumber", that.SerialNumber, 0, AD.UI.ADUI.TotalSerialNumber - 1);
            EditorGUILayout.TextField("ElementName", that.ElementName);
            EditorGUILayout.TextField("ElementArea", that.ElementArea);
        }

        GUI.enabled = true;

        EditorGUILayout.PropertyField(background);
        EditorGUILayout.PropertyField(handle);
        EditorGUILayout.PropertyField(fill);

        EditorGUI.BeginChangeCheck();
        GUIContent gUIContent = new GUIContent("Background");
        sprite = EditorGUILayout.ObjectField(gUIContent, that.backgroundView as Object, typeof(Sprite), @object) as Sprite;
        if( EditorGUI.EndChangeCheck()) that.backgroundView = sprite;

        EditorGUI.BeginChangeCheck();
        GUIContent fUIContent = new GUIContent("Fill");
        sprite = EditorGUILayout.ObjectField(fUIContent, that.fillView as Object, typeof(Sprite), @object) as Sprite;
        if (EditorGUI.EndChangeCheck()) that.fillView = sprite;

        EditorGUI.BeginChangeCheck();
        GUIContent hUIContent = new GUIContent("Handle");
        sprite = EditorGUILayout.ObjectField(hUIContent, that.handleView as Object, typeof(Sprite), @object) as Sprite;
        if (EditorGUI.EndChangeCheck()) that.handleView = sprite;


        serializedObject.ApplyModifiedProperties();
    }
}
