using System;
using TMPro;
using UnityEditor;
using UnityEngine;

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

        private TextMeshProUGUI _source = null;
        public TextMeshProUGUI source
        {
            get
            {
                _source ??= GetComponent<TextMeshProUGUI>();
                return _source;
            }
        }
        public string text { get { return source.text; } set { source.text = value; } }

#if UNITY_EDITOR
        [MenuItem("GameObject/AD/Text", false, 10)]
        private static void ADD(UnityEditor.MenuCommand menuCommand)
        {
            AD.UI.Text text;
            if (ADGlobalSystem.instance != null && ADGlobalSystem.instance._Text != null)
            {
                text = GameObject.Instantiate(ADGlobalSystem.instance._Text).GetComponent<AD.UI.Text>();
                text.name = "New Text";
            }
            else
            {
                text = new GameObject("New Text").AddComponent<AD.UI.Text>();
                text.gameObject.AddComponent<TextMeshProUGUI>();
            }
            GameObjectUtility.SetParentAndAlign(text.gameObject, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(text.gameObject, "Create " + text.name);
            Selection.activeObject = text.gameObject;
        }
#endif

        public static AD.UI.Text Generate(string name = "New Text", string defaultText = "", Transform parent = null, params System.Type[] components)
        {

            AD.UI.Text text;
            if (ADGlobalSystem.instance != null && ADGlobalSystem.instance._Text != null)
            {
                text = GameObject.Instantiate(ADGlobalSystem.instance._Text).GetComponent<AD.UI.Text>();
                text.name = "New Text";
            }
            else
            {
                text = new GameObject(name, components).AddComponent<AD.UI.Text>();
                text.gameObject.AddComponent<TextMeshProUGUI>();
            } 
            text.transform.SetParent(parent, false);
            text.transform.localPosition = Vector3.zero;
            text.text = defaultText;

            return text;
        }

    }
}