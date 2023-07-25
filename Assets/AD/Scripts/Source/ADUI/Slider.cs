using System;
using UnityEditor;
using UnityEngine;

namespace AD.UI
{
    [Serializable]
    [AddComponentMenu("UI/AD/Slider", 100)]
    public class Slider : ADUI
    {
        #region Attribute

        private UnityEngine.UI.Slider _source = null;
        public UnityEngine.UI.Slider source
        {
            get
            {
                _source ??= GetComponent<UnityEngine.UI.Slider>();
                return _source;
            }
        }

        [SerializeField] private UnityEngine.UI.Image background = null;
        [SerializeField] private UnityEngine.UI.Image handle = null;
        [SerializeField] private UnityEngine.UI.Image fill = null;

        public float value { get { return transformer(source.value); } } 

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

        public delegate float Transformer(float value);
        public Transformer transformer = (T) => { return T; };

        #endregion

        public Slider()
        {
            ElementArea = "Slider"; 
        }

        protected void Start()
        {
            AD.UI.ADUI.Initialize(this); 
        }
        protected void OnDestroy()
        {
            AD.UI.ADUI.Destory(this);
        }

        #region Function 

#if UNITY_EDITOR
        [MenuItem("GameObject/AD/Slider", false, 10)]
        private static void ADD(UnityEditor.MenuCommand menuCommand)
        {
            AD.UI.Slider slider;
            if (ADGlobalSystem.instance != null && ADGlobalSystem.instance._Slider != null)
            {
                slider = GameObject.Instantiate(ADGlobalSystem.instance._Slider) as AD.UI.Slider;
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
            slider.name = "New Slider";
            GameObjectUtility.SetParentAndAlign(slider.gameObject, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(slider.gameObject, "Create " + slider.name);
            Selection.activeObject = slider.gameObject;


        }

#endif
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
            RectTransform Background = new GameObject("Background").AddComponent<RectTransform>();
            Background.SetParent(slider.transform);
            Background.gameObject.AddComponent<UnityEngine.UI.Image>();
            Background.localPosition = new Vector3(0, 0, 0);
            Background.anchorMin = new Vector2(0, 0.25f);
            Background.anchorMax = new Vector2(1, 0.75f);
            Background.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 160);
            Background.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 10);
            Background.pivot = new Vector2(0.5f, 0.5f);
            return Background;
        }
        static RectTransform GenerateFillArea(Slider slider)
        {
            RectTransform FillArea = new GameObject("Fill Area").AddComponent<RectTransform>();
            FillArea.SetParent(slider.transform);
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
            HandleSlideArea.SetParent(slider.transform);
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
            fill.SetParent(fill.transform);

            fill.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 10);
            fill.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);
            fill.pivot = new Vector2(0.5f, 0.5f);

            return fill;
        }
        static RectTransform GenerateHandle(RectTransform HandleSlideArea)
        {
            RectTransform Handle = new GameObject("Handle").AddComponent<RectTransform>();
            Handle.gameObject.AddComponent<UnityEngine.UI.Image>();
            Handle.SetParent(HandleSlideArea.transform);

            Handle.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 20);
            Handle.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);
            Handle.pivot = new Vector2(0.5f, 0.5f);

            return Handle;
        }

        public static AD.UI.Slider Generate(string name = "New Slider", Transform parent = null, params System.Type[] components)
        {
            AD.UI.Slider slider = null;
            if (ADGlobalSystem.instance._Slider != null)
            {
                slider = GameObject.Instantiate(ADGlobalSystem.instance._Slider) as AD.UI.Slider; 
            }
            else
            {
                slider = new GameObject(name, components).AddComponent<AD.UI.Slider>();
                RectTransform sliderR = slider.gameObject.AddComponent<RectTransform>();
                sliderR.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 160);
                sliderR.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 20);
                UnityEngine.UI.Slider slider_sl = slider.gameObject.AddComponent<UnityEngine.UI.Slider>();

                slider.background = GenerateBackground(slider).gameObject.GetComponent<UnityEngine.UI.Image>();
                slider_sl.fillRect = GenerateFillArea(slider);
                slider_sl.handleRect = GenerateHandleSlideArea(slider);
                slider_sl.targetGraphic = slider_sl.handleRect.gameObject.GetComponent<UnityEngine.UI.Image>();

                slider.fill = slider_sl.fillRect.gameObject.GetComponent<UnityEngine.UI.Image>();
                slider.handle = slider_sl.handleRect.gameObject.GetComponent<UnityEngine.UI.Image>();
            } 
            slider.transform.SetParent(parent, false);
            slider.transform.localPosition = Vector3.zero;
            slider.name = name;
            foreach (var component in components) slider.gameObject.AddComponent(component);

            return slider;
        }

        public Slider AddListener(UnityEngine.Events.UnityAction<float> call)
        {
            source.onValueChanged.AddListener(call);
            return this;
        }

        #endregion

    }
}