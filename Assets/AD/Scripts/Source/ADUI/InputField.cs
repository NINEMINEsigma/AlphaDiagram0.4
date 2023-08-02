using System;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace AD.UI
{
    [Serializable, RequireComponent(typeof(TMP_InputField))]
    public class InputField : AD.UI.ADUI
    {
        public InputField()
        {
            ElementArea = "InputField";
        }

        protected void Start()
        {
            AD.UI.ADUI.Initialize(this);
        }

        protected void OnDestroy()
        {
            AD.UI.ADUI.Destory(this);
        }

#if UNITY_EDITOR
        [MenuItem("GameObject/AD/InputField", false, 10)]
        private static void ADD(UnityEditor.MenuCommand menuCommand)
        {
            AD.UI.InputField inputField;
            if (ADGlobalSystem.instance != null && ADGlobalSystem.instance._RawImage != null)
            {
                inputField = GameObject.Instantiate(ADGlobalSystem.instance._InputField) as AD.UI.InputField;
            }
            else
            {
                inputField = new GameObject().AddComponent<AD.UI.InputField>();
            }
            inputField.name = "New InputField";
            GameObjectUtility.SetParentAndAlign(inputField.gameObject, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(inputField.gameObject, "Create " + inputField.name);
            Selection.activeObject = inputField.gameObject;
        }
#endif

        public static AD.UI.InputField Generate(string name = "New InputField", Transform parent = null, params System.Type[] components)
        {
            AD.UI.InputField inputField = null;
            if (ADGlobalSystem.instance._Slider != null)
            {
                inputField = GameObject.Instantiate(ADGlobalSystem.instance._InputField, parent) as AD.UI.InputField;
            }
            else
            {
                inputField = new GameObject("New InputField", components).AddComponent<AD.UI.InputField>();
            }

            inputField.transform.SetParent(parent, false);
            inputField.transform.localPosition = Vector3.zero;
            inputField.name = name;
            foreach (var component in components) inputField.gameObject.AddComponent(component);

            return inputField;
        }

        private TMP_InputField _source;
        public TMP_InputField source
        {
            get
            {
                _source ??= GetComponent<TMP_InputField>();
                return _source;
            }
        }

        public string input
        {
            set { source.text = value; }
        }
        public string output
        {
            get { return source.text; }
        }
        public string text
        {
            get { return source.text; }
            set { source.text = value; }
        }

        public InputField SetText(string text)
        {
            this.text = text;
            return this;
        }

        public enum PressType
        {
            OnSelect,
            OnEnd
        }

        public void AddListener(UnityEngine.Events.UnityAction<string> action, PressType type = PressType.OnEnd)
        {
            if (type == PressType.OnSelect) source.onSelect.AddListener(action);
            else if (type == PressType.OnEnd) source.onEndEdit.AddListener(action);
            else AD.ADGlobalSystem.AddMessage("You try to add worry listener");
        }

        public void RemoveListener(UnityEngine.Events.UnityAction<string> action, PressType type = PressType.OnEnd)
        {
            if (type == PressType.OnSelect) source.onSelect.RemoveListener(action);
            else if (type == PressType.OnEnd) source.onEndEdit.RemoveListener(action);
            else AD.ADGlobalSystem.AddMessage("You try to remove worry listener");
        }

        public void RemoveAllListener(PressType type = PressType.OnEnd)
        {
            if (type == PressType.OnSelect) source.onSelect.RemoveAllListeners();
            else if (type == PressType.OnEnd) source.onEndEdit.RemoveAllListeners();
        }

    }
}
