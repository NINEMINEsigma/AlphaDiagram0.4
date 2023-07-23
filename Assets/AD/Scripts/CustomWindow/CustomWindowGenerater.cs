using AD.BASE;
using Lean.Pool;
using UnityEngine;

namespace AD.UI
{
    public class CustomWindowGenerater : ADSystem
    {
        public Canvas MainCanvas;

        public GameObject MainPerfab; 

        public override void Init()
        {

        }  

        public CustomWindowElement ObtainElement()
        { 
            return LeanPool.Spawn(MainPerfab, MainCanvas.transform).GetComponent<CustomWindowElement>().Init();
        }

        public CustomWindowElement ObtainElement(Vector2 rect)
        { 
            var cat = LeanPool.Spawn(MainPerfab, MainCanvas.transform).GetComponent<CustomWindowElement>().Init();
            cat.rectTransform.sizeDelta = rect;
            return cat;
        }

        public static void Despawn(CustomWindowElement element)
        {
            LeanPool.Despawn(element);
        }

    }
}
