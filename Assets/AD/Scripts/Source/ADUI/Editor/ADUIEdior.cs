
using System.Threading;
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


[CustomEditor(typeof(AD.UI.Toggle)), CanEditMultipleObjects]
public class ToggleEdior : Editor
{
    private AD.UI.Toggle that = null;

    private SerializedProperty background = null;
    private SerializedProperty tab = null;
    private SerializedProperty mark = null;
    private SerializedProperty title = null;

    private SerializedProperty _IsCheck = null; 

    private void OnEnable()
    {
        that = target as AD.UI.Toggle;

        background = serializedObject.FindProperty("background");
        tab = serializedObject.FindProperty("tab");
        mark = serializedObject.FindProperty("mark");
        title = serializedObject.FindProperty("title");
        _IsCheck = serializedObject.FindProperty("_IsCheck");
    }

    public override void OnInspectorGUI()
    {
        Sprite sprite = null;
        Object @object = null;
        string str = "";

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
        EditorGUILayout.PropertyField(tab);
        EditorGUILayout.PropertyField(mark);
        EditorGUILayout.PropertyField(title);

        EditorGUI.BeginChangeCheck();
        GUIContent gUIContent = new GUIContent("Background");
        sprite = EditorGUILayout.ObjectField(gUIContent, that.background.sprite as Object, typeof(Sprite), @object) as Sprite;
        if (EditorGUI.EndChangeCheck()) that.background.sprite = sprite;

        EditorGUI.BeginChangeCheck();
        GUIContent tUIContent = new GUIContent("Tab");
        sprite = EditorGUILayout.ObjectField(tUIContent, that.tab.sprite as Object, typeof(Sprite), @object) as Sprite;
        if (EditorGUI.EndChangeCheck()) that.tab.sprite = sprite;

        EditorGUI.BeginChangeCheck();
        GUIContent mUIContent = new GUIContent("Mark");
        sprite = EditorGUILayout.ObjectField(mUIContent, that.mark.sprite as Object, typeof(Sprite), @object) as Sprite;
        if (EditorGUI.EndChangeCheck()) that.mark.sprite = sprite;

        EditorGUI.BeginChangeCheck();
        GUIContent tiUIContent = new GUIContent("Title");
        str = EditorGUILayout.TextField(tiUIContent, that.title.text);
        if (EditorGUI.EndChangeCheck()) that.title.text = str;

        GUI.enabled = false;

        if (Application.isPlaying)
        {
            EditorGUILayout.Toggle("IsCheck", that.IsCheck);
        }

        GUI.enabled = true;

        serializedObject.ApplyModifiedProperties();
    }
}