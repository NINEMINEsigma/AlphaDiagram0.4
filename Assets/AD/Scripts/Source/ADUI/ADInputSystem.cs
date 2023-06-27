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

        public static ADInputSystem instance = null;

        public ADUI _Text; 
        public AudioSourceController _AudioSource;
        public ViewController _Image;

        private Dictionary<KeyControl, ADEvent> OnKeyDown = new Dictionary<KeyControl, ADEvent>();
        private Dictionary<ButtonControl, ADEvent> OnMouseDown = new Dictionary<ButtonControl, ADEvent>();
        public bool DetectKey = true, DetectMouse = true;

        #endregion


        private void Start()
        {
            instance = this;

            AD.SceneSingleAssets.inputSystems.Add(this);
        }

        private void Update()
        {
            if (DetectKey)  foreach (var pair in OnKeyDown) if (pair.Key.wasPressedThisFrame) pair.Value.Invoke();
            if (DetectMouse) foreach (var pair in OnMouseDown) if (pair.Key.wasPressedThisFrame) pair.Value.Invoke();
        }

        public static ADInputSystem AddListener(KeyControl key, UnityAction oe)
        {
            if (!instance.OnKeyDown.ContainsKey(key))
            {
                instance.OnKeyDown.TryAdd(key, new ADEvent());
            }
            instance.OnKeyDown[key].AddListener(oe);
            return instance;
        }
        public static ADInputSystem RemoveLinstener(KeyControl key, UnityAction oe)
        {
            if (instance.OnKeyDown.ContainsKey(key)) instance.OnKeyDown[key].RemoveListener(oe);
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
            GameObject obj = new GameObject("InputSystem");//创建新物体
            obj.AddComponent<AD.UI.ADInputSystem>();
            GameObjectUtility.SetParentAndAlign(obj, menuCommand.context as GameObject);//设置父节点为当前选中物体
            Undo.RegisterCreatedObjectUndo(obj, "Create " + obj.name);//注册到Undo系统,允许撤销
            Selection.activeObject = obj;//将新建物体设为当前选中物体c
        }

    }

}