using System.Collections.Generic; 
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace AD.UI
{
    [RequireComponent(typeof(AD.UI.ViewController))]
    public class CustomWindowElement : MonoBehaviour
    { 
        public Dictionary<string,RectTransform> Childs = new Dictionary<string,RectTransform>();

        public RectTransform rectTransform { get;private set; }
        public AD.UI.ViewController background { get; private set; }

        public Vector2 capacity => new Vector2(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y - 50);
        public Vector2 size { get; private set; } = new Vector2(0, 0);

        public bool isCanBackPool = true;

        [SerializeField] private RectTransform SubPage, TopLine;
        [SerializeField] private AD.UI.Text Title;

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
            size = new Vector2(0, 0);
            isCanBackPool = true;
            return this;
        }

        public void BackPool()
        {
            if (!isCanBackPool) return;
            Init();
            CustomWindowGenerater.Despawn(this);
        }

        public CustomWindowElement MoveTo(Vector3 pos)
        {
            rectTransform.localPosition = pos;
            return this;
        }

        public void RefreshWithNeedSpace(RectTransform rect)
        {
            float x = rect.sizeDelta.x, y = rect.sizeDelta.y;
            if (capacity.x < size.x + x)
            {
                if (capacity.y < size.y + y)
                {
                    rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y + ((int)(y / 50) + 1) * 50);
                    RefreshWithNeedSpace(rect);
                    return;
                }
                else size = new Vector2(x, size.y + y);
            }
            else size = new Vector2(size.x + x, size.y);
            rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, size.x - rect.sizeDelta.x, rect.sizeDelta.x);
            rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, size.y, rect.sizeDelta.y);
        }

        public T GenerateUI<T>(string keyName, ADUI prefab) where T : ADUI, new()
        {
            if (Childs.ContainsKey(keyName)) return null;
            T target = GameObject.Instantiate(prefab.gameObject, SubPage.transform).GetComponent<T>();
            GenerateTarget(keyName, target.GetComponent<RectTransform>());
            return target;
        }

        public void GenerateTarget(string keyName, RectTransform target)
        {
            if (Childs.ContainsKey(keyName)) return;
            target.SetParent(SubPage);
            Childs.Add(keyName, target);
            RefreshWithNeedSpace(target);
            return;
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
