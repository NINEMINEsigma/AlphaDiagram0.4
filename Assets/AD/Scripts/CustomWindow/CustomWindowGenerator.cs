using System.Collections.Generic;
using AD.BASE;
using UnityEngine;

namespace AD.UI
{
    internal static class CustomWindowGeneratorAssets
    {
        //<Pool,Queue>
        public static GameObject CustomWindowGeneratorPool;
        public static Queue<GameObject> Objects = new();
    }

    internal static class CustomWindowGeneratorAssets<T> where T : CustomWindowGenerator<T>
    {
        //<Pool,Queue>
        public static GameObject CustomWindowGeneratorPool;
        public static Queue<GameObject> Objects = new();
    }

    public class CustomWindowGenerator : ADSystem
    {
        public RectTransform Parent;

        public CustomWindowElement WindowPerfab;

        public override void Init()
        {
            if (CustomWindowGeneratorAssets.CustomWindowGeneratorPool != null) GameObject.Destroy(CustomWindowGeneratorAssets.CustomWindowGeneratorPool);
            CustomWindowGeneratorAssets.CustomWindowGeneratorPool = new GameObject("CustomWindow Pool");
            foreach (var SingleObject in CustomWindowGeneratorAssets.Objects)
                GameObject.Destroy(SingleObject.As<CustomWindowElement>());
            CustomWindowGeneratorAssets.Objects.Clear();
        }

        public virtual CustomWindowElement ObtainElement()
        {
            if (Architecture == null) return null;
            var cat = (CustomWindowGeneratorAssets.Objects.Count > 0
                ? CustomWindowGeneratorAssets.Objects.Dequeue()
                : GameObject.Instantiate(WindowPerfab.gameObject)).GetComponent<CustomWindowElement>();
            cat.gameObject.SetActive(true);
            cat.transform.SetParent(Parent, false);
            cat.HowBackPool = Despawn;
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
            CustomWindowGeneratorAssets.Objects.Enqueue(element.gameObject);
            element.transform.SetParent(CustomWindowGeneratorAssets.CustomWindowGeneratorPool.transform, false);
        }

    }

    public class CustomWindowGenerator<T> : ADSystem where T : CustomWindowGenerator<T>
    {
        public RectTransform Parent;

        public CustomWindowElement WindowPerfab;

        public override void Init()
        {
            if (CustomWindowGeneratorAssets<T>.CustomWindowGeneratorPool != null) GameObject.Destroy(CustomWindowGeneratorAssets<T>.CustomWindowGeneratorPool);
            CustomWindowGeneratorAssets<T>.CustomWindowGeneratorPool = new GameObject(typeof(T).Name + " Pool");
            foreach (var SingleObject in CustomWindowGeneratorAssets<T>.Objects)
                GameObject.Destroy(SingleObject.As<CustomWindowElement>());
            CustomWindowGeneratorAssets<T>.Objects.Clear();
        }

        public virtual CustomWindowElement ObtainElement()
        {
            if (Architecture == null) return null;
            var cat = (CustomWindowGeneratorAssets<T>.Objects.Count > 0
                ? CustomWindowGeneratorAssets<T>.Objects.Dequeue()
                : GameObject.Instantiate(WindowPerfab.gameObject)).GetComponent<CustomWindowElement>();
            cat.gameObject.SetActive(true);
            cat.transform.SetParent(Parent, false);
            cat.Init();
            cat.HowBackPool = Despawn;
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
            CustomWindowGeneratorAssets<T>.Objects.Enqueue(element.gameObject);
            element.transform.SetParent(CustomWindowGeneratorAssets<T>.CustomWindowGeneratorPool.transform, false);
        }

    }
}
