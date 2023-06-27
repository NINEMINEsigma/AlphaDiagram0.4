using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

namespace AD.UI
{
    [Serializable]
    [AddComponentMenu("UI/AD/Slider", 100)]
    public class Slider : ADUI
    {
        public Slider()
        {
            ElementArea = "Slider"; 
        }

        protected void Start()
        {
            AD.UI.ADUI.Initialize(this);
        }
        protected void OnDestory()
        {
            AD.UI.ADUI.Destory(this);
        }

        public UnityEngine.UI.Slider source = null;

        [SerializeField] private UnityEngine.UI.Image background = null;
        [SerializeField] private UnityEngine.UI.Image handle = null;
        [SerializeField] private UnityEngine.UI.Image fill = null;

        public float value { get { return source.value; } } 

        public Sprite backgroundView 
        { 
            get { if (background == null) return null; else return background.sprite; }
            set { if (background != null) background.sprite = value; } 
        }
        public Sprite handleView
        {
            get { if (handle == null) return null; else return handle.sprite; }
            set { if (handle != null) handle.sprite = value; }
        }
        public Sprite fillView
        {
            get { if (fill == null) return null; else return fill.sprite; }
            set { if (fill != null) fill.sprite = value; }
        }

        [MenuItem("GameObject/AD/Slider", false, 10)]
        private static void ADD(UnityEditor.MenuCommand menuCommand)
        {
            AD.UI.Slider slider;
            if (ADInputSystem.instance != null && ADInputSystem.instance._Text != null)
            {
                slider = GameObject.Instantiate(ADInputSystem.instance._Text).GetComponent<AD.UI.Slider>();
            }
            else
            {
                slider = GenerateSliderParent();
                UnityEngine.UI.Slider slider_sl = slider.gameObject.AddComponent<UnityEngine.UI.Slider>();

                slider.background = GenerateBackground(slider).gameObject.GetComponent<UnityEngine.UI.Image>();
                slider_sl.fillRect = GenerateFillArea(slider);
                slider_sl.handleRect = GenerateHandleSlideArea(slider);
                slider_sl.targetGraphic = slider_sl.handleRect.gameObject.GetComponent<UnityEngine.UI.Image>();

                slider.fill = slider_sl.fillRect.gameObject.GetComponent<UnityEngine.UI.Image>();
                slider.handle = slider_sl.handleRect.gameObject.GetComponent<UnityEngine.UI.Image>();
            }
            GameObjectUtility.SetParentAndAlign(slider.gameObject, menuCommand.context as GameObject);//设置父节点为当前选中物体
            Undo.RegisterCreatedObjectUndo(slider.gameObject, "Create " + slider.name);//注册到Undo系统,允许撤销
            Selection.activeObject = slider.gameObject;//将新建物体设为当前选中物体


        }

        static Slider GenerateSliderParent(string name = "New Slider")
        {
            Slider slider = new GameObject(name).AddComponent<AD.UI.Slider>();
            RectTransform sliderR = slider.gameObject.AddComponent<RectTransform>();
            sliderR.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 160);
            sliderR.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 20);
            return slider;
        }
        static RectTransform GenerateBackground(Slider slider)
        {
            RectTransform Backgound = new GameObject("Background").AddComponent<RectTransform>();
            GameObjectUtility.SetParentAndAlign(Backgound.gameObject, slider.gameObject);
            Backgound.gameObject.AddComponent<UnityEngine.UI.Image>();
            Backgound.localPosition = new Vector3(0, 0, 0);
            Backgound.anchorMin = new Vector2(0, 0.25f);
            Backgound.anchorMax = new Vector2(1, 0.75f);
            Backgound.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 160);
            Backgound.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 10);
            Backgound.pivot = new Vector2(0.5f, 0.5f);
            return Backgound;
        }
        static RectTransform GenerateFillArea(Slider slider)
        {
            RectTransform FillArea = new GameObject("Fill Area").AddComponent<RectTransform>();
            GameObjectUtility.SetParentAndAlign(FillArea.gameObject, slider.gameObject);
            FillArea.localPosition = new Vector3(-5, 0, 0);
            FillArea.anchorMin = new Vector2(0, 0.25f);
            FillArea.anchorMax = new Vector2(1, 0.75f);
            FillArea.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 140);
            FillArea.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 10);
            FillArea.pivot = new Vector2(0.5f, 0.5f);

            return GenerateFill(FillArea);
        }
        static RectTransform GenerateHandleSlideArea(Slider slider)
        {
            RectTransform HandleSlideArea = new GameObject("Handle Slide Area").AddComponent<RectTransform>();
            GameObjectUtility.SetParentAndAlign(HandleSlideArea.gameObject, slider.gameObject);
            HandleSlideArea.localPosition = new Vector3(0, 0, 0);
            HandleSlideArea.anchorMin = new Vector2(0, 0);
            HandleSlideArea.anchorMax = new Vector2(1, 1);
            HandleSlideArea.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 140);
            HandleSlideArea.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 20);
            HandleSlideArea.pivot = new Vector2(0.5f, 0.5f);

            return GenerateHandle(HandleSlideArea);
        }
        static RectTransform GenerateFill(RectTransform FullArea)
        {
            RectTransform fill = new GameObject("Fill").AddComponent<RectTransform>();
            fill.gameObject.AddComponent<UnityEngine.UI.Image>();
            GameObjectUtility.SetParentAndAlign(fill.gameObject, FullArea.gameObject);

            fill.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 10);
            fill.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);
            fill.pivot = new Vector2(0.5f, 0.5f);

            return fill;
        }
        static RectTransform GenerateHandle(RectTransform HandleSlideArea)
        {
            RectTransform Handle = new GameObject("Handle").AddComponent<RectTransform>();
            Handle.gameObject.AddComponent<UnityEngine.UI.Image>();
            GameObjectUtility.SetParentAndAlign(Handle.gameObject, HandleSlideArea.gameObject);

            Handle.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 20);
            Handle.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);
            Handle.pivot = new Vector2(0.5f, 0.5f);

            return Handle;
        }

        public static AD.UI.Slider Generate(string name = "New Slider", Transform parent = null, params System.Type[] components)
        {
            AD.UI.Slider slider = new GameObject(name, components).AddComponent<AD.UI.Slider>();
            RectTransform sliderR = slider.gameObject.AddComponent<RectTransform>();
            sliderR.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 160);
            sliderR.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 20);
            GameObjectUtility.SetParentAndAlign(slider.gameObject, parent.gameObject);//设置父节点为当前选中物体
            UnityEngine.UI.Slider slider_sl = slider.gameObject.AddComponent<UnityEngine.UI.Slider>();

            slider.background = GenerateBackground(slider).gameObject.GetComponent<UnityEngine.UI.Image>();
            slider_sl.fillRect = GenerateFillArea(slider);
            slider_sl.handleRect = GenerateHandleSlideArea(slider);
            slider_sl.targetGraphic = slider_sl.handleRect.gameObject.GetComponent<UnityEngine.UI.Image>();

            slider.fill= slider_sl.fillRect.gameObject.GetComponent<UnityEngine.UI.Image>();
            slider.handle = slider_sl.handleRect.gameObject.GetComponent<UnityEngine.UI.Image>();

            return slider;
        }

        public Slider AddListener(UnityEngine.Events.UnityAction<float> call)
        {
            source.onValueChanged.AddListener(call);
            return this;
        } 

    }
}