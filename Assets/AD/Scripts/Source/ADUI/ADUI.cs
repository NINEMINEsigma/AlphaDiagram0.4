using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AD.UI
{
    public interface IADUI
    {
        IADUI Obtain(int serialNumber);
        IADUI Obtain(string elementName);
        string ElementName { get; set; }
        int SerialNumber { get; set; }
    }

    [Serializable]
    public abstract class ADUI : MonoBehaviour, IADUI, IPointerEnterHandler, IPointerExitHandler
    {
        public bool Selected = false;

        public static List<IADUI> Items { get; private set; } = new List<IADUI>();
        public static int TotalSerialNumber { get; private set; } = 0;
        public static string UIArea { get; private set; } = "null";

        public string ElementName { get; set; } = "null";
        public int SerialNumber { get; set; } = 0;
        public string ElementArea  = "null";
         
        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            Selected = true;
            UIArea = ElementArea;
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            Selected = false;
            UIArea = "null";
        }

        public virtual IADUI Obtain(int serialNumber)
        {
            return Items.Find((P) => P.SerialNumber == serialNumber);
        }

        public virtual IADUI Obtain(string elementName)
        {
            return Items.Find((P) => P.ElementName == elementName);
        }

        public virtual IADUI TryObtain(int serialNumber)
        {
            foreach (var item in Items) 
                if (item.SerialNumber == serialNumber) return item;
            return null;
        }

        public virtual IADUI TryObtain(string elementName)
        {
            foreach (var item in Items)
                if (item.ElementName == elementName) return item;
            return null;
        }

        public virtual List<IADUI> ObtainAll(Predicate<IADUI> _Right)
        {
            List<IADUI> result = new List<IADUI>();
            foreach (var item in Items)
                if (_Right(item)) result.Add(item);
            return (result.Count > 0) ? result : null;
        }

        public static void Initialize(IADUI obj)
        { 
            obj.SerialNumber = TotalSerialNumber++; 
            Items.Add(obj);
        }

        public static void Destory(IADUI obj)
        {
            Items.Remove(obj); 
        }
    }
}
