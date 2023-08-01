using AD.BASE;
using AD.UI;
using Lean.Pool;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace AD.Choose
{
    public class RecordItem : ListViewItem
    {
        protected override bool IsNeedLayoutGourp 
        { 
            get => base.IsNeedLayoutGourp;
            set
            {
                if (!value) throw new ADException("you cannt set this value to false"); 
                base.IsNeedLayoutGourp = true;
            }
        }

        private void Start()
        {
            IsNeedLayoutGourp = true;
        }

        public override ListViewItem Init()
        {
            throw new System.NotImplementedException();
        }

        [SerializeField] private ViewController Box;
        [SerializeField] private RectTransform SubPage;
        [SerializeField] private GameObject Prefab;
        [SerializeField] private int index;

        protected override void LetChildAdd(GameObject child)
        {
            child.transform.SetParent(SubPage, false);
        }

        protected override void LetChildDestroy(GameObject child)
        {
            LeanPool.Despawn(child);
        }

        protected override GridLayoutGroup HowGetOrAddGridLayoutGroup()
        {
            if (GridLayoutGroup == null) return SubPage.GetOrAddComponent<GridLayoutGroup>();
            else return GridLayoutGroup;
        }

        public void GenerateItemWithTitle(string title)
        {
            var cat = LeanPool.Spawn(Prefab);
            this[index++] = cat;
            cat.GetComponent<IButton>().SetTitle(title);
        }


    }
}
