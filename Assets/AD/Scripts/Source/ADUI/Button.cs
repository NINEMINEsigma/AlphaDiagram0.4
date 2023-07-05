using System;
using System.Collections;
using System.Collections.Generic;
using AD.ADbase;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace AD.UI
{
    [Serializable, RequireComponent(typeof(UnityEngine.UI.Image))]
    [AddComponentMenu("UI/AD/Button", 100)]
    public class Button : ADUI,IPointerClickHandler
    {
        public Animator animator = null;
        public ADEvent OnClick = new ADEvent(), OnRelease = new ADEvent();
        private bool _IsClick = false;
        public bool IsClick
        {
            get
            {
                return _IsClick;
            }
            set
            {
                if (animator != null) animator.SetBool("IsClick", value);
                _IsClick = value;
                if (value) OnClick.Invoke();
                else OnRelease.Invoke();
            }
        }

        public Button()
        {
            ElementArea = "Button";
        }

        protected void Start()
        {
            AD.UI.ADUI.Initialize(this); 
        }

        protected void OnDestory()
        {
            AD.UI.ADUI.Destory(this);
        }

        [MenuItem("GameObject/AD/Button", false, 10)]
        private static void ADD(UnityEditor.MenuCommand menuCommand)
        {
            AD.UI.Button button;
            if (ADGlobalSystem.instance != null && ADGlobalSystem.instance._Button != null)
            {
                button = GameObject.Instantiate(ADGlobalSystem.instance._Button) as AD.UI.Button;
            }
            else
            {
                button = new GameObject().AddComponent<AD.UI.Button>();
            }
            button.name = "New Button";
            GameObjectUtility.SetParentAndAlign(button.gameObject, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(button.gameObject, "Create " + button.name);
            Selection.activeObject = button.gameObject;
        }

        public static AD.UI.Button Generate(string name = "New Button", Transform parent = null, params System.Type[] components)
        {
            AD.UI.Button button = null;
            if (ADGlobalSystem.instance._Slider != null)
            {
                button = GameObject.Instantiate(ADGlobalSystem.instance._Button) as AD.UI.Button;
            }
            else
            {
                button = new GameObject("New Button", components).AddComponent<AD.UI.Button>(); 
            }

            GameObjectUtility.SetParentAndAlign(button.gameObject, parent.gameObject);
            button.name = name;
            foreach (var component in components) button.gameObject.AddComponent(component);

            return button;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            IsClick = !IsClick;
        }
    }
}