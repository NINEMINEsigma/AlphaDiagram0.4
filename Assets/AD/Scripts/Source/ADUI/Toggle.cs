using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace AD.UI
{
    [Serializable]
    [AddComponentMenu("UI/AD/Toggle", 100)]
    public class Toggle : AD.UI.ADUI
    {
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
            }
        } 

        #endregion 

        public Toggle()
        {
            ElementArea = "Toggle";
        }

        protected void Start()
        {
            AD.UI.ADUI.Initialize(this);
            ADInputSystem.AddMouseListener(IsLeftDown, ()=> { this.OnChoose(); });
        }
        protected void OnDestory()
        {
            AD.UI.ADUI.Destory(this);
        }

        #region Function

        private static bool IsLeftDown()
        {
            Debug.LogWarning(Mouse.current.leftButton.wasPressedThisFrame);
            return Mouse.current.leftButton.wasPressedThisFrame;
        }

        private void OnChoose()
        { 
            if (!Selected) return;
            Debug.LogWarning("yes");
            IsCheck = !IsCheck;
        }

        [MenuItem("GameObject/AD/Toggle", false, 10)]
        private static void ADD(UnityEditor.MenuCommand menuCommand)
        {
            AD.UI.Toggle toggle = null;
            if (ADInputSystem.instance._Toggle != null)
            {
                toggle = GameObject.Instantiate(ADInputSystem.instance._Toggle) as AD.UI.Toggle;
            }
            else
            {
                toggle = GenerateToggle();
                toggle.name = "New Toggle";
            }
            GameObjectUtility.SetParentAndAlign(toggle.gameObject, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(toggle.gameObject, "Create " + toggle.name);
            Selection.activeObject = toggle.gameObject;
        }

        public static AD.UI.Toggle Generate(string name = "New Text", Transform parent = null, params System.Type[] components)
        {
            AD.UI.Toggle toggle = null;
            if (ADInputSystem.instance._Toggle != null)
            {
                toggle = GameObject.Instantiate(ADInputSystem.instance._Toggle) as AD.UI.Toggle;
            }
            else
            {
                toggle = GenerateToggle();
                foreach (var component in components) toggle.gameObject.AddComponent(component);
            }

            GameObjectUtility.SetParentAndAlign(toggle.gameObject, parent.gameObject);
            toggle.name = name;

            return toggle;
        }

        private static AD.UI.Toggle GenerateToggle()
        {
            AD.UI.Toggle toggle = null;
            toggle=new GameObject().AddComponent<AD.UI.Toggle>();
            return toggle;
        }

        #endregion

    }
}