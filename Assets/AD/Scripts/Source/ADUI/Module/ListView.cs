using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AD.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AD.UI
{
    public abstract class ListViewItem: PropertyModule
    {
        public abstract ListViewItem Init();
    }

    public class ListView : PropertyModule
    {
        public static readonly string ListViewDefaultPerfabSpawnKey = "ListViewDefault";
        public static string LVDPSK => ListViewDefaultPerfabSpawnKey;

        [Header("ListView")]
        [SerializeField] private ScrollRect _Scroll;
        [SerializeField] private VerticalLayoutGroup _List;
        [SerializeField] private TMP_Text _Title;
        [SerializeField] private ListViewItem Prefab;
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
        public void SetPrefab(ListViewItem prefab)
        {
            Prefab = prefab;
        }

        public ListViewItem GenerateItem()
        {
            if (Prefab == null) return null;
            GameObject item  = Spawn(ListViewDefaultPerfabSpawnKey, Prefab.gameObject);
            this[index++] = item;
            return item.GetComponent<ListViewItem>().Init();
        }

        protected override void LetChildDestroy(GameObject child)
        {
            Despawn(ListViewDefaultPerfabSpawnKey, child);
        }

        protected override void LetChildAdd(GameObject child)
        {
            child.transform.SetParent(_List.gameObject.transform, false);
        }

        public GameObject FindItem(int index)
        {
            return this[index];
        }

        public void Clear()
        {
            List<int> indexs = new();
            indexs.AddRange(from KeyValuePair<int, GameObject> item in Childs
                            select item.Key);
            foreach (int index in indexs)
            {
                Remove(index);
            }
        }

    }
}
