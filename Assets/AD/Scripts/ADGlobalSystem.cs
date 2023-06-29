using System.Collections.Generic;
using System.Linq;
using AD.ADbase;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace AD.UI
{
    public class ADGlobalSystem : MonoBehaviour, AD.IADInputSystem
    {
        public class UnregisterInfo
        {
            public UnregisterInfo(ButtonControl key, UnityEngine.Events.UnityAction action)
            {
                this.keys.Add(key);
                this.action = action;
            }
            public UnregisterInfo(List<ButtonControl> keys, UnityEngine.Events.UnityAction action)
            {
                this.keys = keys;
                this.action = action;
            }

            private List<ButtonControl> keys = new List<ButtonControl>();
            private UnityEngine.Events.UnityAction action = null;

            public void Unregister()
            {
                ADGlobalSystem.RemoveListener(keys, action);
            }
        }

        private static ADGlobalSystem _m_instance = null;
        public static ADGlobalSystem instance
        {
            get
            {
                if (_m_instance == null)
                {
                    _m_instance = new GameObject().AddComponent<ADGlobalSystem>();
                    _m_instance.name = "GlobalSystem";
                    //Ichni.IchniUtility.utility.AddMessage("InputSystem was builded");
                }
                return _m_instance;
            }
        }

        public Dictionary<List<ButtonControl>, Dictionary<PressType, UnityEvent>> multipleInputController
            = new Dictionary<List<ButtonControl>, Dictionary<PressType, UnityEvent>>();

        public ADUI _Toggle, _Slider, _Text, _Button;
        public ViewController _Image;
        public AudioSourceController _AudioSource;

        public enum PressType
        {
            JustPressed,
            ThisFramePressed,
            ThisFrameReleased
        }

        private void Update()
        {
            foreach (var key in multipleInputController)
            {
                if (key.Key.All((P) => P.isPressed))
                {
                    key.Value.TryGetValue(PressType.JustPressed, out var events);
                    events?.Invoke();
                }
                if (key.Key.All((P) => P.wasPressedThisFrame))
                {
                    key.Value.TryGetValue(PressType.ThisFramePressed, out var events);
                    events?.Invoke();
                }
                if (key.Key.All((P) => P.wasReleasedThisFrame))
                {
                    key.Value.TryGetValue(PressType.ThisFrameReleased, out var events);
                    events?.Invoke();
                }
            }
        }

        public bool _IsOnValidate = false;

        private void OnValidate()
        {
            _IsOnValidate = true;
        }

        #region Function 

        public static ADGlobalSystem AddListener(ButtonControl key, UnityEngine.Events.UnityAction action, out UnregisterInfo info, PressType type = PressType.JustPressed)
        {
            KeyValuePair<List<ButtonControl>, Dictionary<PressType, UnityEvent>> pair
                = instance.multipleInputController.FirstOrDefault((P) => { return P.Key[0].Equals(key) && P.Key.Count == 1; });
            if (pair.Equals(default(KeyValuePair<List<ButtonControl>, Dictionary<PressType, UnityEvent>>)))
            {
                UnityEvent currentEv = new UnityEvent();
                currentEv.AddListener(action);

                instance.multipleInputController.Add(new List<ButtonControl>() { key }, new Dictionary<PressType, UnityEvent> { { type, currentEv } });

                //Ichni.IchniUtility.utility.AddMessage(key.ToString() + "-based event was established");
            }
            else
            {
                if (!pair.Value.ContainsKey(type)) pair.Value.Add(type, new UnityEvent());
                pair.Value[type].AddListener(action);
            }
            instance._IsOnValidate = true;
            info = new UnregisterInfo(key, action);
            return instance;
        }
        public static ADGlobalSystem AddListener(List<ButtonControl> keys, UnityEngine.Events.UnityAction action, out UnregisterInfo info, PressType type = PressType.JustPressed)
        {
            KeyValuePair<List<ButtonControl>, Dictionary<PressType, UnityEvent>> pair
                = instance.multipleInputController.FirstOrDefault((P) => { return P.Key.Intersect(keys).ToList().Count == keys.Count; });
            if (pair.Equals(default(KeyValuePair<List<ButtonControl>, Dictionary<PressType, UnityEvent>>)))
            {
                UnityEvent currentEv = new UnityEvent();
                currentEv.AddListener(action);

                instance.multipleInputController.Add(keys, new Dictionary<PressType, UnityEvent> { { type, currentEv } });

                //Ichni.IchniUtility.utility.AddMessage(keys.ToString() + "-based event was established");
            }
            else
            {
                if (!pair.Value.ContainsKey(type)) pair.Value.Add(type, new UnityEvent());
                pair.Value[type].AddListener(action);
            }
            instance._IsOnValidate = true;
            info = new UnregisterInfo(keys, action);
            return instance;
        }
        public static ADGlobalSystem AddListener(ButtonControl key, UnityEngine.Events.UnityAction action, PressType type = PressType.JustPressed)
        {
            KeyValuePair<List<ButtonControl>, Dictionary<PressType, UnityEvent>> pair
                = instance.multipleInputController.FirstOrDefault((P) => { return P.Key[0].Equals(key) && P.Key.Count == 1; });
            if (pair.Equals(default(KeyValuePair<List<ButtonControl>, Dictionary<PressType, UnityEvent>>)))
            {
                UnityEvent currentEv = new UnityEvent();
                currentEv.AddListener(action);

                instance.multipleInputController.Add(new List<ButtonControl>() { key }, new Dictionary<PressType, UnityEvent> { { type, currentEv } });

                //Ichni.IchniUtility.utility.AddMessage(key.ToString() + "-based event was established");
            }
            else
            {
                if (!pair.Value.ContainsKey(type)) pair.Value.Add(type, new UnityEvent());
                pair.Value[type].AddListener(action);
            }
            instance._IsOnValidate = true;
            return instance;
        }
        public static ADGlobalSystem AddListener(List<ButtonControl> keys, UnityEngine.Events.UnityAction action, PressType type = PressType.JustPressed)
        {
            KeyValuePair<List<ButtonControl>, Dictionary<PressType, UnityEvent>> pair
                = instance.multipleInputController.FirstOrDefault((P) => { return P.Key.Intersect(keys).ToList().Count == keys.Count; });
            if (pair.Equals(default(KeyValuePair<List<ButtonControl>, Dictionary<PressType, UnityEvent>>)))
            {
                UnityEvent currentEv = new UnityEvent();
                currentEv.AddListener(action);

                instance.multipleInputController.Add(keys, new Dictionary<PressType, UnityEvent> { { type, currentEv } });

                //Ichni.IchniUtility.utility.AddMessage(keys.ToString() + "-based event was established");
            }
            else
            {
                if (!pair.Value.ContainsKey(type)) pair.Value.Add(type, new UnityEvent());
                pair.Value[type].AddListener(action);
            }
            instance._IsOnValidate = true;
            return instance;
        }
        public static ADGlobalSystem RemoveListener(ButtonControl key, UnityEngine.Events.UnityAction action, PressType type = PressType.JustPressed)
        {
            KeyValuePair<List<ButtonControl>, Dictionary<PressType, UnityEvent>> pair
                = instance.multipleInputController.FirstOrDefault((P) => { return P.Key[0].Equals(key) && P.Key.Count == 1; });
            if (!pair.Equals(default(KeyValuePair<List<ButtonControl>, UnityEvent>)) && pair.Value.ContainsKey(type))
            {
                pair.Value[type].RemoveListener(action);

                //Ichni.IchniUtility.utility.AddMessage(key.ToString() + "-based event was removed member");
            }
            instance._IsOnValidate = true;
            return instance;
        }
        public static ADGlobalSystem RemoveListener(List<ButtonControl> keys, UnityEngine.Events.UnityAction action, PressType type = PressType.JustPressed)
        {
            KeyValuePair<List<ButtonControl>, Dictionary<PressType, UnityEvent>> pair
                = instance.multipleInputController.FirstOrDefault((P) => { return P.Key.Intersect(keys).ToList().Count == keys.Count; });
            if (!pair.Equals(default(KeyValuePair<List<ButtonControl>, UnityEvent>)) && pair.Value.ContainsKey(type))
            {
                pair.Value[type].RemoveListener(action);

                //Ichni.IchniUtility.utility.AddMessage(keys.ToString() + "-based event was removed member");
            }
            instance._IsOnValidate = true;
            return instance;
        }
        public static ADGlobalSystem RemoveAllListeners(ButtonControl key, PressType type = PressType.JustPressed)
        {
            KeyValuePair<List<ButtonControl>, Dictionary<PressType, UnityEvent>> pair
                = instance.multipleInputController.FirstOrDefault((P) => { return P.Key[0].Equals(key) && P.Key.Count == 1; });
            if (!pair.Equals(default(KeyValuePair<List<ButtonControl>, UnityEvent>)) && pair.Value.ContainsKey(type))
            {
                pair.Value[type].RemoveAllListeners();

                //Ichni.IchniUtility.utility.AddMessage(key.ToString() + "-based event was removed");
            }
            instance._IsOnValidate = true;
            return instance;
        }
        public static ADGlobalSystem RemoveAllListeners(List<ButtonControl> keys, PressType type = PressType.JustPressed)
        {
            KeyValuePair<List<ButtonControl>, Dictionary<PressType, UnityEvent>> pair
                = instance.multipleInputController.FirstOrDefault((P) => { return P.Key.Intersect(keys).ToList().Count == keys.Count; });
            if (!pair.Equals(default(KeyValuePair<List<ButtonControl>, UnityEvent>)) && pair.Value.ContainsKey(type))
            {
                pair.Value[type].RemoveAllListeners();

                //Ichni.IchniUtility.utility.AddMessage(keys.ToString() + "-based event was removed");
            }
            instance._IsOnValidate = true;
            return instance;
        }
        public static ADGlobalSystem RemoveAllButtonListeners()
        {
            instance.multipleInputController = new Dictionary<List<ButtonControl>, Dictionary<PressType, UnityEvent>>();
            return instance;
        }

        [MenuItem("GameObject/AD/GlobalSystem", false, 10)]
        public static void ADD(MenuCommand menuCommand)
        {
            if (instance != null) return;
            AD.UI.ADGlobalSystem obj = new GameObject("GlobalSystem").AddComponent<AD.UI.ADGlobalSystem>();
            _m_instance = obj;
            GameObjectUtility.SetParentAndAlign(obj.gameObject, menuCommand.context as GameObject);//设置父节点为当前选中物体
            Undo.RegisterCreatedObjectUndo(obj.gameObject, "Create " + obj.name);//注册到Undo系统,允许撤销
            Selection.activeObject = obj.gameObject;//将新建物体设为当前选中物体c
        }

        #endregion

    }

#if UNITY_EDITOR

    [CustomEditor(typeof(ADGlobalSystem))]
    public class GlobalSystemEditor : Editor
    {
        ADGlobalSystem that = null;

        List<string> buttons = new List<string>();

        private SerializedProperty _Toggle, _Slider, _Text, _Button;
        private SerializedProperty _Image;
        private SerializedProperty _AudioSource;

        private void OnEnable()
        {
            that = target as ADGlobalSystem;

            buttons = new List<string>();
            foreach (var key in that.multipleInputController)
            {
                foreach (var button in key.Key)
                {
                    buttons.Add(button.ToString() + "  ");
                }
            }
            that._IsOnValidate = false;

            _Toggle = serializedObject.FindProperty("_Toggle");
            _Slider = serializedObject.FindProperty("_Slider");
            _Text = serializedObject.FindProperty("_Text");
            _Button = serializedObject.FindProperty("_Button");
            _Image = serializedObject.FindProperty("_Image");
            _AudioSource = serializedObject.FindProperty("_AudioSource");
        }

        public override void OnInspectorGUI()
        {
            if (that._IsOnValidate)
            {
                buttons = new List<string>();
                foreach (var key in that.multipleInputController)
                {
                    foreach (var button in key.Key)
                    {
                        buttons.Add(button.ToString() + "  ");
                    }
                }
                that._IsOnValidate = false;
            }

            serializedObject.Update();

            EditorGUILayout.PropertyField(_Toggle);
            EditorGUILayout.PropertyField(_Slider);
            EditorGUILayout.PropertyField(_Text);
            EditorGUILayout.PropertyField(_Button);
            EditorGUILayout.PropertyField(_Image);
            EditorGUILayout.PropertyField(_AudioSource);

            EditorGUILayout.Space(25);

            if (buttons.Count == 0) EditorGUILayout.TextArea("No Event was register");
            else foreach (var key in buttons) EditorGUILayout.TextArea(key);

            serializedObject.ApplyModifiedProperties();
        }
    }

#endif
}