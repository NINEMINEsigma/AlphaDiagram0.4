using Lean.Pool;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AD.UI
{
    public class ListView : PropertyModule
    {
        [Header("ListView")]
        [SerializeField] private ScrollRect _Scroll;
        [SerializeField] private VerticalLayoutGroup _List;
        [SerializeField] private TMP_Text _Title;
        [SerializeField] private GameObject Prefab;
        [SerializeField] private int index = 0;

        public ScrollRect.ScrollRectEvent onValueChanged
        {
            get => _Scroll.onValueChanged;
            set => _Scroll.onValueChanged = value;
        }

        public void SetTitle(string title)
        {
            _Title.text = title;
        }
        public void SetPrefab(GameObject prefab)
        {
            Prefab = prefab;
        }

        public GameObject GenerateItem()
        {
            if (Prefab == null) return null;
            GameObject item  = LeanPool.Spawn(Prefab);
            this[index++] = item;
            return item;
        }

        protected override void LetChildDestroy(GameObject child)
        {
            LeanPool.Despawn(child);
        }

        public GameObject FindItem(int index)
        {
            return this[index];
        }

    }
}
