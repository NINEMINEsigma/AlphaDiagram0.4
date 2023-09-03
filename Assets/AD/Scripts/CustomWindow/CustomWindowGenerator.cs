using System.Collections;
using System.Collections.Generic;
using AD.BASE;
using UnityEngine;

namespace AD.UI
{
    public class CustomWindowGenerator : ADSystem
    {
        public RectTransform Parent;

        public CustomWindowElement WindowPerfab;

        private static GameObject Pool;
        private static Queue<GameObject> Objects = new();

        public override void Init()
        {
            if (Pool != null) GameObject.Destroy(Pool);
            Pool = new GameObject("CustomWindow(Generator Pool)");
            foreach (var SingleObject in Objects)
                GameObject.Destroy(SingleObject.As<CustomWindowElement>());
            Objects.Clear();
        }  

        public virtual CustomWindowElement ObtainElement()
        {
            if (Architecture == null) return null;
            var cat = (Objects.Count > 0 ? Objects.Dequeue() : GameObject.Instantiate(WindowPerfab.gameObject)).GetComponent<CustomWindowElement>();
            cat.gameObject.SetActive(true);
            cat.transform.SetParent(Parent);
            return cat;
        }

        public virtual CustomWindowElement ObtainElement(Vector2 rect)
        {
            if (Architecture == null) return null;
            var cat = ObtainElement();
            cat.SetRect(rect);
            return cat;
        }

        public static void Despawn(CustomWindowElement element)
        {
            element.gameObject.SetActive(false);
            Objects.Enqueue(element.gameObject);
            element.transform.SetParent(Pool.transform);
        }

    }
}
