using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class HierarchyObjectIconDrawer
{
    static HierarchyObjectIconDrawer()
    {
        EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyWindowItemGUI;
    }

    static void OnHierarchyWindowItemGUI(int instanceID, Rect selectionRect)
    {
        var objectContent = EditorGUIUtility.ObjectContent(EditorUtility.InstanceIDToObject(instanceID), null);
        if (objectContent.image != null && !IgnoredObjectIconNames.Contains(objectContent.image.name))
        {
            GUI.DrawTexture(new Rect(selectionRect.xMax - IconSize, selectionRect.yMin, IconSize, IconSize), objectContent.image);
        }
    }

    static readonly int IconSize = 16;

    static readonly List<string> IgnoredObjectIconNames = new List<string>
    {
        "d_GameObject Icon",
        "d_Prefab Icon",
        "d_PrefabVariant Icon",
        "d_PrefabModel Icon"
    };
}
