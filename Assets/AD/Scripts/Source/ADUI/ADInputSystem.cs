using System.Collections.Generic;
using AD.ADbase;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace AD.UI
{
    [ExecuteAlways]
    public class ADInputSystem : MonoBehaviour, AD.IADInputSystem
    {
        #region Attribute

        private static ADInputSystem _m_instance = null;
        public static ADInputSystem instance
        {
            get
            {
                if (_m_instance == null) _m_instance = new GameObject("InputSystem").AddComponent<ADInputSystem>();
                return _m_instance;
            }
            private set
            {
                _m_instance = value;
            }
        }

        public ADUI _Text,_Slider,_Toggle; 
        public AudioSourceController _AudioSource;
        public ViewController _Image;

        public delegate bool Detecter();

        private Dictionary<Detecter, ADEvent> OnKeyDown = new Dictionary<Detecter, ADEvent>();
        private Dictionary<Detecter, ADEvent> OnMouseDown = new Dictionary<Detecter, ADEvent>();
        public bool DetectKey = true, DetectMouse = true;

        public string CurrentArea = "";

        #endregion

        private void Awake()
        {
            instance = this; 
        }

        private void Start()
        { 
            AD.SceneSingleAssets.inputSystems.Add(this);
        }

        private void Update()
        {
            CurrentArea = AD.UI.ADUI.UIArea;
            if (DetectKey) foreach (var pair in OnKeyDown) if (pair.Key()) pair.Value.Invoke();
            if (DetectMouse) foreach (var pair in OnMouseDown) if (pair.Key()) pair.Value.Invoke();
        }

        #region Function

        public static ADInputSystem AddKeyListener(Detecter key, UnityAction oe)
        {
            if (!instance.OnKeyDown.ContainsKey(key))
            {
                instance.OnKeyDown.TryAdd(key, new ADEvent());
            }
            instance.OnKeyDown[key].AddListener(oe);
            return instance;
        }
        public static ADInputSystem RemoveKeyLinstener(Detecter key, UnityAction oe)
        {
            if (instance.OnKeyDown.ContainsKey(key)) instance.OnKeyDown[key].RemoveListener(oe);
            return instance;
        }
        public static ADInputSystem AddMouseListener(Detecter key, UnityAction oe)
        {
            if (!instance.OnMouseDown.ContainsKey(key))
            {
                instance.OnMouseDown.TryAdd(key, new ADEvent());
            }
            instance.OnMouseDown[key].AddListener(oe);
            return instance;
        }
        public static ADInputSystem RemoveMouseLinstener(Detecter key, UnityAction oe)
        {
            if (instance.OnMouseDown.ContainsKey(key)) instance.OnMouseDown[key].RemoveListener(oe);
            return instance;
        }
        /*public static ADInputSystem RemoveAllListeners(KeyControl key) = delete;
        {
            if (instance.OnKeyDown.ContainsKey(key)) instance.OnKeyDown[key].RemoveAllListeners();
            return instance;
        }*/

        [MenuItem("GameObject/AD/InputSystem", false, 10)]
        public static void ADD(MenuCommand menuCommand)
        {
            if (instance != null) return;
            AD.UI.ADInputSystem  obj = new GameObject("InputSystem").AddComponent<AD.UI.ADInputSystem>();
            GameObjectUtility.SetParentAndAlign(obj.gameObject, menuCommand.context as GameObject);//设置父节点为当前选中物体
            Undo.RegisterCreatedObjectUndo(obj.gameObject, "Create " + obj.name);//注册到Undo系统,允许撤销
            Selection.activeObject = obj.gameObject;//将新建物体设为当前选中物体c
        }

        #endregion

    }

}