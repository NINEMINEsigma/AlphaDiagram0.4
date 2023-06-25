using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AD.UI
{
    [AddComponentMenu("UI/AD/Text", 100)]
    public class Text:UnityEngine.UI.Text
	{
        [MenuItem("GameObject/AD/Text", false, 10)]
        private static void ADD(MenuCommand menuCommand) 
        {
            GameObject obj = new GameObject("New Text");//创建新物体
            obj.AddComponent<AD.UI.Text>();
            GameObjectUtility.SetParentAndAlign(obj, menuCommand.context as GameObject);//设置父节点为当前选中物体
            Undo.RegisterCreatedObjectUndo(obj, "Create " + obj.name);//注册到Undo系统,允许撤销
            Selection.activeObject = obj;//将新建物体设为当前选中物体c
        }
	}  
}