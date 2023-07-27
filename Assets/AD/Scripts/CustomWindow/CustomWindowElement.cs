using System.Collections.Generic;
using AD.BASE;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AD.UI
{
    [RequireComponent(typeof(AD.UI.ViewController))]
    public class CustomWindowElement : MonoBehaviour,IDragHandler,IBeginDragHandler
    {
        public Dictionary<string, RectTransform> Childs = new Dictionary<string, RectTransform>();

        private RectTransform _rectTransform;
        public RectTransform rectTransform
        {
            get
            {
                _rectTransform ??= GetComponent<RectTransform>();
                return _rectTransform;
            }
        }
        private AD.UI.ViewController _background;
        public AD.UI.ViewController background
        {
            get
            {
                _background ??= GetComponent<AD.UI.ViewController>();
                return _background;
            }
        }

        public Vector2 capacity => new Vector2(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y - TopLine.sizeDelta.y);
        public Vector2 size { get; private set; }
        [SerializeField] private Vector4 _Padding = Vector4.zero;
        public Vector4 Padding
        {
            get
            {
                return _Padding;
            }
            set
            {
                _Padding = value;
                RefreshAllChild();
            }
        }

        [HideInInspector] public bool isCanBackPool = true;
        private bool isCanRefresh = true;

        [SerializeField] private RectTransform SubPage, TopLine;
        [SerializeField] private AD.UI.Text Title;

        public ADEvent OnEsc = new ADEvent();

        public CustomWindowElement Init()
        {
            foreach (var item in Childs)
            {
                Destroy(item.Value.gameObject);
            }
            Childs.Clear();
            rectTransform.localPosition = Vector3.zero;
            rectTransform.sizeDelta = new Vector2(300, 100);
            size = new Vector2(capacity.x, 0);
            Padding = Vector4.zero;
            isCanBackPool = true;
            return this;
        }

        public void BackPool()
        {
            if (!isCanBackPool) return;
            OnEsc.Invoke();
            Init();
            CustomWindowGenerator.Despawn(this);
        }

        public CustomWindowElement MoveTo(Vector3 pos)
        {
            if (isCanRefresh)
                rectTransform.localPosition = pos;
            return this;
        }

        public CustomWindowElement MoveWithRectControl(Vector4 args)
        {
            if (!isCanRefresh) return this;
            rectTransform.rect.Set(args[0], args[1], args[2], args[3]);
            return this;
        }

        public CustomWindowElement SetRect(Vector2 Rect)
        {
            if (!isCanRefresh) return this;
            rectTransform.sizeDelta = Rect + new Vector2(0, TopLine.sizeDelta.y);
            return this;
        }

        public RectTransform GetChild(string keyName)
        {
            if (Childs.ContainsKey(keyName)) return Childs[keyName];
            else return null;
        }

        public CustomWindowElement SetTitle(string title)
        {
            Title.text = title;
            return this;
        }

        #region  Refresh

        public void RefreshAllChild()
        {
            if (isCanRefresh)
                foreach (var item in Childs)
                {
                    RefreshWithNeedSpace(item.Value);
                }
        }

        private void RefreshWithNeedSpace(float x, float y, RectTransform rect)
        {
            if (!isCanRefresh) return;
            if (capacity.x < x + Padding[0] + Padding[2])
            {
                SetRect(new Vector2(x + Padding[0] + Padding[2], size.y)); 
                RefreshAllChild();
                RefreshWithNeedSpace(x, y, rect);
                return;
            }
            if (capacity.x > size.x + x + Padding[0] + Padding[2])
            {
                rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, size.x + Padding[0], rect.rect.width);
                rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, size.y + Padding[1], rect.rect.height);
                size = new Vector2(size.x + x, size.y);
            }
            else
            {
                if (capacity.y < size.y + y + Padding[1] + Padding[3]) 
                    SetRect(new Vector2(size.x , size.y + y + Padding[1] + Padding[3]));
                rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, Padding[0], rect.rect.width);
                rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, size.y + Padding[1], rect.rect.height);
                size = new Vector2(x, size.y + y);
            }
        }

        private void RefreshWithNeedSpace(RectTransform rect)
        {
            if (!isCanRefresh) return;
            float x = rect.sizeDelta.x, y = rect.sizeDelta.y;
            RefreshWithNeedSpace(x, y, rect);
        }

        #endregion

        #region Generate

        public T SetADUIOnWindow<T>(string keyName, ADUI item) where T : ADUI, new()
        {
            return SetADUIOnWindow<T>(keyName, item, item.GetComponent<RectTransform>().sizeDelta);
        }

        public bool SetItemOnWindow(string keyName, GameObject item)
        {
            return SetItemOnWindow(keyName, item, item.GetComponent<RectTransform>().sizeDelta);
        }

        public T SetADUIOnWindow<T>(string keyName, ADUI item, Vector2 Rect) where T : ADUI, new()
        {
            if (Childs.ContainsKey(keyName)) return null; 
            if (SetItemOnWindow(keyName, item.gameObject, Rect))
                return item.GetComponent<T>();
            else return null;
        }

        public bool SetItemOnWindow(string keyName, GameObject prefab, Vector2 Rect)
        {
            if (Childs.ContainsKey(keyName) || !prefab.TryGetComponent<RectTransform>(out var target)) return false;
            target.SetParent(SubPage, false);
            target.sizeDelta = Rect;
            Childs.Add(keyName, target);
            RefreshWithNeedSpace(target);
            return true;
        }

        public CustomWindowElement GenerateSubWindow(string keyName, Vector2 rect,string title)
        { 
            var subWindow = GameObject.Instantiate(gameObject).GetComponent<CustomWindowElement>();
            if (SetItemOnWindow(keyName, subWindow.gameObject, rect))
                return subWindow.SetTitle(title);
            else return null;
        }

        #region Button

        public AD.UI.Button GenerateButton(string keyName)
        {
            return SetADUIOnWindow<AD.UI.Button>(keyName, AD.UI.Button.Generate(keyName)).SetTitle(keyName);
        }

        public AD.UI.Button GenerateButton(string keyName, Vector2 rect)
        {
            return SetADUIOnWindow<AD.UI.Button>(keyName, AD.UI.Button.Generate(keyName), rect).SetTitle(keyName);
        }

        #endregion

        #region Slider

        public AD.UI.Slider GenerateSlider(string keyName)
        {
            return SetADUIOnWindow<AD.UI.Slider>(keyName, AD.UI.Slider.Generate(keyName));
        }

        public AD.UI.Slider GenerateSlider(string keyName, Vector2 rect)
        {
            return SetADUIOnWindow<AD.UI.Slider>(keyName, AD.UI.Slider.Generate(keyName), rect);
        }

        #endregion

        #region Text

        public AD.UI.Text GenerateText(string keyName)
        {
            return SetADUIOnWindow<AD.UI.Text>(keyName, AD.UI.Text.Generate(keyName));
        }

        public AD.UI.Text GenerateText(string keyName, string defaultText)
        {
            return SetADUIOnWindow<AD.UI.Text>(keyName, AD.UI.Text.Generate(keyName, defaultText));
        }

        public AD.UI.Text GenerateText(string keyName, Vector2 rect)
        {
            return SetADUIOnWindow<AD.UI.Text>(keyName, AD.UI.Text.Generate(keyName), rect);
        }

        public AD.UI.Text GenerateText(string keyName, string defaultText, Vector2 rect)
        {
            return SetADUIOnWindow<AD.UI.Text>(keyName, AD.UI.Text.Generate(keyName, defaultText), rect);
        }

        #endregion

        #region Toggle

        public AD.UI.Toggle GenerateToggle(string keyName)
        {
            return SetADUIOnWindow<AD.UI.Toggle>(keyName, AD.UI.Toggle.Generate(keyName));
        }

        public AD.UI.Toggle GenerateToggle(string keyName, string defaultText)
        {
            return SetADUIOnWindow<AD.UI.Toggle>(keyName, AD.UI.Toggle.Generate(keyName)).SetTitle(defaultText);
        }

        public AD.UI.Toggle GenerateToggle(string keyName, Vector2 rect)
        {
            return SetADUIOnWindow<AD.UI.Toggle>(keyName, AD.UI.Toggle.Generate(keyName), rect);
        }

        public AD.UI.Toggle GenerateToggle(string keyName, string defaultText, Vector2 rect)
        {
            return SetADUIOnWindow<AD.UI.Toggle>(keyName, AD.UI.Toggle.Generate(keyName), rect).SetTitle(defaultText);
        }

        #endregion

        #region InputField

        public AD.UI.InputField GenerateInputField(string keyName)
        {
            return SetADUIOnWindow<AD.UI.InputField>(keyName, AD.UI.InputField.Generate(keyName));
        }

        public AD.UI.InputField GenerateInputField(string keyName, Vector2 Rect)
        {
            return SetADUIOnWindow<AD.UI.InputField>(keyName, AD.UI.InputField.Generate(keyName), Rect);
        }

        #endregion

        #region RawImage

        public AD.UI.RawImage GenerateRawImage(string keyName)
        {
            return SetADUIOnWindow<AD.UI.RawImage>(keyName, AD.UI.RawImage.Generate(keyName));
        }

        public AD.UI.RawImage GenerateRawImage(string keyName, Vector2 Rect)
        {
            return SetADUIOnWindow<AD.UI.RawImage>(keyName, AD.UI.RawImage.Generate(keyName), Rect);
        }

        #endregion

        #endregion

        #region Drag

        public bool topOnClick = true;

        private Vector2 originalLocalPointerPosition;
        private Vector3 originalPanelLocalPosition;

        private RectTransform DragObjectInternal => rectTransform;

        private RectTransform DragAreaInternal => transform.parent.transform as RectTransform;

        public void OnBeginDrag(PointerEventData data)
        {
            originalPanelLocalPosition = DragObjectInternal.localPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(DragAreaInternal, data.position, data.pressEventCamera, out originalLocalPointerPosition);
            gameObject.transform.SetAsLastSibling();

            if (topOnClick == true)
                DragObjectInternal.SetAsLastSibling();
        }

        public void OnDrag(PointerEventData data)
        {
            Vector2 localPointerPosition;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(DragAreaInternal, data.position, data.pressEventCamera, out localPointerPosition))
            {
                Vector3 offsetToOriginal = localPointerPosition - originalLocalPointerPosition;
                DragObjectInternal.localPosition = originalPanelLocalPosition + offsetToOriginal;
            }

            ClampToArea();
        }

        private void ClampToArea()
        {
            Vector3 pos = DragObjectInternal.localPosition;

            Vector3 minPosition = DragAreaInternal.rect.min - DragObjectInternal.rect.min;
            Vector3 maxPosition = DragAreaInternal.rect.max - DragObjectInternal.rect.max;

            pos.x = Mathf.Clamp(DragObjectInternal.localPosition.x, minPosition.x, maxPosition.x);
            pos.y = Mathf.Clamp(DragObjectInternal.localPosition.y, minPosition.y, maxPosition.y);

            DragObjectInternal.localPosition = pos;
        }

        #endregion

    }
}
