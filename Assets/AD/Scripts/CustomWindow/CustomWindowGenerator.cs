using AD.BASE;
using Lean.Pool;
using UnityEngine;

namespace AD.UI
{
    public class CustomWindowGenerator : ADSystem
    {
        public RectTransform Parent;

        public GameObject WindowPerfab; 

        public override void Init()
        {

        }  

        public virtual CustomWindowElement ObtainElement()
        {
            if (Architecture == null) return null;
            return LeanPool.Spawn(WindowPerfab, Parent.transform).GetComponent<CustomWindowElement>().Init();
        }

        public virtual CustomWindowElement ObtainElement(Vector2 rect)
        {
            if (Architecture == null) return null;
            var cat = LeanPool.Spawn(WindowPerfab, Parent.transform).GetComponent<CustomWindowElement>().Init();
            cat.SetRect(rect);
            return cat;
        }

        public static void Despawn(CustomWindowElement element)
        {
            LeanPool.Despawn(element);
        }

    }
}
