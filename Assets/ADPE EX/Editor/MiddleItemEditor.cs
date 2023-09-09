using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using AD.Experimental.Runtime.PipeEx;
using System;

[CustomEditor(typeof(MiddleItem))]
public class MiddleItemEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        if (GUILayout.Button("TestUpdata"))
        {
            foreach (var SingleNext in (target as MiddleItem).Nexts)
            {
                if (SingleNext.Next.PipeStepPerproty == null)
                {
                    SingleNext.Next.PipeStepPerproty = new();
                    SingleNext.Next.PipeStepPerproty.ArgTypes = new Type[1];
                }

            }
            (target as MiddleItem).UpdateLine();
        }

        serializedObject.ApplyModifiedProperties();

    }
}
