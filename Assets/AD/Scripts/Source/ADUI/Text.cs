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
            GameObject obj = new GameObject("New Text");//����������
            obj.AddComponent<AD.UI.Text>();
            GameObjectUtility.SetParentAndAlign(obj, menuCommand.context as GameObject);//���ø��ڵ�Ϊ��ǰѡ������
            Undo.RegisterCreatedObjectUndo(obj, "Create " + obj.name);//ע�ᵽUndoϵͳ,������
            Selection.activeObject = obj;//���½�������Ϊ��ǰѡ������c
        }
	}  
}