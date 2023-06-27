using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
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
        public Text()
        {
            ElementArea = "Text"; 
        }

        protected void Start()
        {
            AD.UI.ADUI.Initialize(this);
        }
        protected void OnDestory()
        {
            AD.UI.ADUI.Destory(this);
        }

        public TMP_Text source = null;
        public string text { get { return source.text; } set { source.text = value; } } 


        [MenuItem("GameObject/AD/Text", false, 10)]
        private static void ADD(UnityEditor.MenuCommand menuCommand)
        {
            AD.UI.Text text;
            if (ADInputSystem.instance != null && ADInputSystem.instance._Text != null)
            {
                text = GameObject.Instantiate(ADInputSystem.instance._Text).GetComponent<AD.UI.Text>();
            }
            else
            {
                text = new GameObject("New Text").AddComponent<AD.UI.Text>();
                text.source = text.gameObject.AddComponent<TextMeshProUGUI>(); 
            }
            GameObjectUtility.SetParentAndAlign(text.gameObject, menuCommand.context as GameObject);//设置父节点为当前选中物体
            Undo.RegisterCreatedObjectUndo(text.gameObject, "Create " + text.name);//注册到Undo系统,允许撤销
            Selection.activeObject = text.gameObject;//将新建物体设为当前选中物体
        }

        public static AD.UI.Text Generate(string name = "New Text", string defaultText = "", Transform parent = null, params System.Type[] components)
        {

            AD.UI.Text text;
            if (ADInputSystem.instance != null && ADInputSystem.instance._Text != null)
            {
                text = GameObject.Instantiate(ADInputSystem.instance._Text).GetComponent<AD.UI.Text>();
            }
            else
            {
                text = new GameObject(name, components).AddComponent<AD.UI.Text>();
                text.source = text.gameObject.AddComponent<TextMeshProUGUI>(); 
            }
            GameObjectUtility.SetParentAndAlign(text.gameObject, parent.gameObject);//设置父节点为当前选中物体 
            text.text = defaultText;

            return text;
        }

    }
}