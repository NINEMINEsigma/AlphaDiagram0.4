using System;
using AD.BASE;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AD.UI
{
    [Serializable, RequireComponent(typeof(UnityEngine.UI.Image))]
    [AddComponentMenu("UI/AD/Button", 100)]
    public class Button : ADUI, IPointerClickHandler
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
#if UNITY_EDITOR
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
#endif

        public static AD.UI.Button Generate(string name = "New Button", Transform parent = null, params System.Type[] components)
        {
            AD.UI.Button button = null;
            if (ADGlobalSystem.instance._Slider != null)
            {
                button = GameObject.Instantiate(ADGlobalSystem.instance._Button, parent) as AD.UI.Button;
            }
            else
            {
                button = new GameObject("New Button", components).AddComponent<AD.UI.Button>(); 
            }

            button.transform.SetParent(parent, false);
            button.transform.localPosition = Vector3.zero;
            button.name = name;
            foreach (var component in components) button.gameObject.AddComponent(component);

            return button;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            IsClick = !IsClick;
        }

        public void AddListener(UnityEngine.Events.UnityAction action, PressType type = PressType.ThisFramePressed)
        {
            if (type == PressType.ThisFramePressed) OnClick.AddListener(action);
            else if (type == PressType.ThisFrameReleased) OnRelease.AddListener(action);
            else AD.ADGlobalSystem.AddMessage("You try to add worry listener");
        }

        public void RemoveListener(UnityEngine.Events.UnityAction action, PressType type = PressType.ThisFramePressed)
        {
            if (type == PressType.ThisFramePressed) OnClick.RemoveListener(action);
            else if (type == PressType.ThisFrameReleased) OnRelease.RemoveListener(action);
            else AD.ADGlobalSystem.AddMessage("You try to remove worry listener");
        }

        public void RemoveAllListener(PressType type = PressType.ThisFramePressed)
        {
            if (type == PressType.ThisFramePressed) OnClick.RemoveAllListeners();
            else if (type == PressType.ThisFrameReleased) OnRelease.RemoveAllListeners();
        }
    }
}