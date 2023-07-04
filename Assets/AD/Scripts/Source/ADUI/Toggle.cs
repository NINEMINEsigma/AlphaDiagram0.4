using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;
using AD.ADbase;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;
using UnityEngine.UIElements;

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

        public UnityEngine.UI.Image background = null;
        public UnityEngine.UI.Image tab = null;
        public UnityEngine.UI.Image mark = null;
        public TMP_Text title = null;

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

        private RegisterInfo __unregisterInfo = null;

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
        protected void OnDestory()
        {
            AD.UI.ADUI.Destory(this);
            __unregisterInfo.UnRegister();
        }

        #region Function  

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

            GameObjectUtility.SetParentAndAlign(toggle.gameObject, parent.gameObject);
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
            GameObjectUtility.SetParentAndAlign(background.gameObject, toggle.gameObject);
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
            GameObjectUtility.SetParentAndAlign(tab.gameObject,toggle.gameObject);
            GameObjectUtility.SetParentAndAlign(GenerateMark().gameObject, tab.gameObject); 
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

        #endregion

    }
}