using System;
using AD.BASE;
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
            TextProperty = new(this);
            ValueProperty = new(this);
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

        public TextProperty TextProperty { get; private set; }
        public TextValueProperty ValueProperty { get; private set; }

        public Text SetText(string text)
        {
            this.text = text;
            return this;
        }

    }

    public class BindTextAsset:AD.BASE.Property<string>.PropertyAsset
    {
        public BindTextAsset(Text source)
        {
            this.source = source;
        }

        Text source;

        public override string value { get => source.text; set => source.text = value; }
    }

    public class BindTextValueAsset : AD.BASE.Property<float>.PropertyAsset
    {
        public BindTextValueAsset(Text source)
        {
            this.source = source;
        }

        Text source;

        public override float value { get => float.Parse(source.text); set => source.text = value.ToString(); }
    }

    public class TextProperty:AD.BASE.BindProperty<string>
    {
        public TextProperty(Text source)
        {
            SetPropertyAsset(new BindTextAsset(source));
        } 
    }

    public class TextValueProperty : AD.BASE.BindProperty<float>
    {
        public TextValueProperty(Text source)
        {
            SetPropertyAsset(new BindTextValueAsset(source));
        } 
    }

}
