using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AD.UI
{
    [Serializable]
    [AddComponentMenu("UI/AD/Text", 100)] 
    public class Text : ADUI
    {
        protected void Start()
        {
            ElementArea = "Text";
            AD.UI.ADUI.Initialize(this); 
        }
        protected void OnDestory()
        { 
            AD.UI.ADUI.Destory(this);
        }

        private TMP_Text _m_text = null;
        public string text { get { return _m_text.text; } }
        public TMP_Text textObject { get { return _m_text; } }


        [MenuItem("GameObject/AD/Text", false, 10)]
        private static void ADD(MenuCommand menuCommand)
        {
            AD.UI.Text text = new GameObject("New Text").AddComponent<AD.UI.Text>();
            text._m_text = text.gameObject.AddComponent<TextMeshProUGUI>();
            GameObjectUtility.SetParentAndAlign(text.gameObject, menuCommand.context as GameObject);//设置父节点为当前选中物体
            Undo.RegisterCreatedObjectUndo(text.gameObject, "Create " + text.name);//注册到Undo系统,允许撤销
            Selection.activeObject = text.gameObject;//将新建物体设为当前选中物体
        }

        public static AD.UI.Text Generate(string name = "New Text", Transform parent = null, params System.Type[] components)
        {
            AD.UI.Text text = new GameObject(name, components).AddComponent<AD.UI.Text>();
            text._m_text = text.gameObject.AddComponent<TextMeshProUGUI>();
            text.transform.parent = parent; 

            return text;
        }



    }
}