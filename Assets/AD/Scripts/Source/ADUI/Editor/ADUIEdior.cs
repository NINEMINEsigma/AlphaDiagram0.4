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
        //base.OnInspectorGUI(); 

        serializedObject.Update();

        GUI.enabled = false;

        if (Application.isPlaying)
        {
            EditorGUILayout.IntSlider("SerialNumber", that.SerialNumber, 0, AD.UI.ADUI.TotalSerialNumber - 1);
            EditorGUILayout.TextField("ElementName", that.ElementName);
        }

        GUI.enabled = true;



        serializedObject.ApplyModifiedProperties();
    }
}
