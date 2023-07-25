using System.Collections.Generic;
using AD.BASE;
using UnityEngine; 

namespace AD.UI
{
    [RequireComponent(typeof(AD.UI.ViewController))]
    public class CustomWindowElement : MonoBehaviour
    { 
        public Dictionary<string,RectTransform> Childs = new Dictionary<string,RectTransform>();

        public RectTransform rectTransform { get;private set; }
        public AD.UI.ViewController background { get; private set; }

        public Vector2 capacity => new Vector2(rectTransform.rect.width, rectTransform.rect.height - TopLine.rect.height);
        public Vector2 size { get; private set; } = new Vector2(0, 0);
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

        [HideInInspector]public bool isCanBackPool = true;
        private bool isCanRefresh = true;

        [SerializeField] private RectTransform SubPage, TopLine;
        [SerializeField] private AD.UI.Text Title;

        public ADEvent OnEsc = new ADEvent();

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            background = GetComponent<AD.UI.ViewController>();
        }

        public CustomWindowElement Init()
        {
            foreach (var item in Childs)
            { 
                Destroy(item.Value.gameObject);
            }
            Childs.Clear();
            rectTransform.localPosition = Vector3.zero;
            rectTransform.sizeDelta = new Vector2(300, 100);
            size = Vector2.zero; 
            Padding = Vector4.zero;
            isCanBackPool = true;
            return this;
        }

        public void BackPool()
        {
            if (!isCanBackPool) return;
            Init();
            OnEsc.Invoke();
            CustomWindowGenerator.Despawn(this);
        }

        public CustomWindowElement MoveTo(Vector3 pos)
        {
            if (isCanRefresh)
                rectTransform.localPosition = pos;
            return this;
        }

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
            if (capacity.x < x + Padding[1] + Padding[3])
            {
                rectTransform.rect.Set(rectTransform.rect.x, rectTransform.rect.y, x + Padding[1] + Padding[3], TopLine.rect.height);
                RefreshAllChild();
                RefreshWithNeedSpace(rect);
                return;
            }
            if (capacity.x < size.x + x + Padding[3])
            {
                if (capacity.y < size.y + y + Padding[4])
                {
                    rectTransform.rect.Set(
                        rectTransform.rect.x,
                        rectTransform.rect.y,
                        rectTransform.rect.width,
                        rectTransform.sizeDelta.y + ((int)(y / TopLine.rect.height) + 1) * TopLine.rect.height);
                    RefreshWithNeedSpace(rect);
                    return;
                }
                else size = new Vector2(x, size.y + y);
            }
            else size = new Vector2(size.x + x, size.y);
            rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, size.x - x + Padding[1], rect.rect.width);
            rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, size.y + Padding[2], rect.rect.height);
        }

        private void RefreshWithNeedSpace(RectTransform rect)
        {
            if (!isCanRefresh) return;
            float x = rect.rect.width, y = rect.rect.height;
            RefreshWithNeedSpace(x, y,rect);
        }

        public T GenerateADUI<T>(string keyName, ADUI prefab) where T : ADUI, new()
        {
            if (Childs.ContainsKey(keyName)) return null;
            T target = GameObject.Instantiate(prefab.gameObject, SubPage.transform).GetComponent<T>();
            GenerateTarget(keyName, target.gameObject);
            return target;
        }

        public bool GenerateTarget(string keyName, GameObject prefab)
        {
            if (Childs.ContainsKey(keyName) || !prefab.TryGetComponent<RectTransform>(out var target)) return false;
            target.SetParent(SubPage);
            Childs.Add(keyName, target);
            RefreshWithNeedSpace(target);
            return true;
        }

        public CustomWindowElement GenerateSubWindow(Vector2 rect)
        {
            CustomWindowElement customWindow = GameObject.Instantiate(gameObject).GetComponent<CustomWindowElement>();
            customWindow.SetRect(rect);
            customWindow.OnEsc.AddListener(this.RefreshAllChild);
            customWindow.isCanRefresh = false;
            GenerateTarget(customWindow.GetHashCode().ToString(), customWindow.gameObject);
            return customWindow;
        }

        public void SetRect(Vector2 Rect)
        {
            if (!isCanRefresh) return;
            rectTransform.rect.Set(rectTransform.rect.x, rectTransform.rect.y, Rect.x, Rect.y + TopLine.rect.height);
            RefreshAllChild();
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
    }
}
