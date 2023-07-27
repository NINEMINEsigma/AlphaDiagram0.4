using System;
using AD.BASE;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

namespace AD.UI
{
    [Serializable]
    [AddComponentMenu("UI/AD/Toggle", 100)]
    public class Toggle : AD.UI.ADUI
    {
        public class UnRegisterInfo
        {
            public UnRegisterInfo(UnityEngine.Events.UnityAction<bool> action, Toggle toggle)
            {
                this.action = action;
                this.toggle = toggle;
            }

            private UnityEngine.Events.UnityAction<bool> action = null;
            private Toggle toggle = null;

            public void UnRegister()
            {
                toggle.RemoveListener(this.action);
                action = null;
            }

            public void RegisterAsNew(UnityEngine.Events.UnityAction<bool> action)
            {
                toggle.RemoveListener(this.action);
                toggle.AddListener(action); 
                this.action = action;
            }
        }

        #region Attribute

        public UnityEngine.UI.Image background;
        public UnityEngine.UI.Image tab;
        public UnityEngine.UI.Image mark;
        public TMP_Text title;

        private bool _IsCheck = false;
        public bool IsCheck
        {
            get { return _IsCheck; }
            private set
            {
                _IsCheck = value;
                mark.gameObject.SetActive(value);
                actions.Invoke(value);
            }
        }

        private RegisterInfo __unregisterInfo;

        private ADEvent<bool> actions = new ADEvent<bool>();

        #endregion 

        public Toggle()
        {
            ElementArea = "Toggle";
        }

        protected void Start()
        {
            AD.UI.ADUI.Initialize(this);
            __unregisterInfo = ADGlobalSystem.AddListener(Mouse.current.leftButton, () =>
            {
                if (!Selected) return;
                IsCheck = !IsCheck;
            }); 
        }
        protected void OnDestroy()
        {
            AD.UI.ADUI.Destory(this);
            __unregisterInfo.UnRegister();
        }

        #region Function  

#if UNITY_EDITOR
        [MenuItem("GameObject/AD/Toggle", false, 10)]
        private static void ADD(UnityEditor.MenuCommand menuCommand)
        {
            AD.UI.Toggle toggle = null;
            if (ADGlobalSystem.instance._Toggle != null)
            {
                toggle = GameObject.Instantiate(ADGlobalSystem.instance._Toggle) as AD.UI.Toggle;
            }
            else
            {
                toggle = GenerateToggle(); 
                toggle.background= GenerateBackground(toggle).GetComponent<UnityEngine.UI.Image>();
                toggle.tab = GenerateTab(toggle).GetComponent<UnityEngine.UI.Image>();
                toggle.mark = toggle.tab.transform.GetChild(0).gameObject.GetComponent<UnityEngine.UI.Image>();
            }
            toggle.name = "New Toggle";
            GameObjectUtility.SetParentAndAlign(toggle.gameObject, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(toggle.gameObject, "Create " + toggle.name);
            Selection.activeObject = toggle.gameObject;
        }
#endif

        public static AD.UI.Toggle Generate(string name = "New Text", Transform parent = null, params System.Type[] components)
        {
            AD.UI.Toggle toggle = null;
            if (ADGlobalSystem.instance._Toggle != null)
            {
                toggle = GameObject.Instantiate(ADGlobalSystem.instance._Toggle) as AD.UI.Toggle; 
            }
            else
            {
                toggle = GenerateToggle(); 
                toggle.title.text = name;
                foreach (var component in components) toggle.gameObject.AddComponent(component); 
                toggle.background = GenerateBackground(toggle).GetComponent<UnityEngine.UI.Image>();
                toggle.tab = GenerateTab(toggle).GetComponent<UnityEngine.UI.Image>();
                toggle.mark = toggle.tab.transform.GetChild(0).gameObject.GetComponent<UnityEngine.UI.Image>();
            } 
            toggle.transform.SetParent(parent, false);
            toggle.transform.localPosition = Vector3.zero;
            toggle.name = name;

            return toggle;
        }

        private static AD.UI.Toggle GenerateToggle()
        {
            AD.UI.Toggle toggle = new GameObject("New Toggle", new Type[] { typeof(ContentSizeFitter), typeof(TextMeshProUGUI) }).AddComponent<AD.UI.Toggle>();
            toggle.title = toggle.GetComponent<TextMeshProUGUI>();
            toggle.title.text = "Toggle";
            return toggle;
        }
        private static RectTransform GenerateBackground(AD.UI.Toggle toggle)
        {
            RectTransform background  = new GameObject("Background").AddComponent<UnityEngine.UI.Image>().gameObject.GetComponent<RectTransform>();
            background.localPosition = new Vector3(-50, 0, 0);
            background.anchorMin = new Vector2(0, 0);
            background.anchorMax = new Vector2(1, 1);
            background.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 100);
            background.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 24);
            background.transform.parent = toggle.transform;
            return background;
        } 
        private static RectTransform GenerateTab(AD.UI.Toggle toggle)
        {
            RectTransform tab  = new GameObject("Tab").AddComponent<UnityEngine.UI.Image>().gameObject.GetComponent<RectTransform>();
            tab.localPosition = new Vector3(-37.5f, 0, 0);
            tab.anchorMin = new Vector2(0, 0);
            tab.anchorMax = new Vector2(0, 1);
            tab.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 20);
            tab.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 20);
            tab.transform.parent = toggle.transform;
            GenerateMark().transform.parent = tab.transform; 
            return tab;
        }
        private static RectTransform GenerateMark()
        {
            RectTransform mark = new GameObject("Mark").AddComponent<UnityEngine.UI.Image>().gameObject.GetComponent<RectTransform>();
            mark.localPosition = new Vector3(0, 0, 0);
            mark.anchorMin = new Vector2(0, 0);
            mark.anchorMax = new Vector2(1, 1);
            mark.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 20);
            mark.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 20);
            return mark;
        }

        public AD.UI.Toggle AddListener(UnityEngine.Events.UnityAction<bool> action)
        {
            actions.AddListener(action);
            return this;
        }
        public AD.UI.Toggle AddListener(UnityEngine.Events.UnityAction<bool> action, out UnRegisterInfo info)
        {
            info = new UnRegisterInfo(action, this);
            actions.AddListener(action);
            return this;
        }
        public AD.UI.Toggle RemoveListener(UnityEngine.Events.UnityAction<bool> action)
        {
            actions.RemoveListener(action); 
            return this;
        }

        public AD.UI.Toggle SetTitle(string title)
        {
            this.title.text = title;
            return this; 
        }

        #endregion

    }
}