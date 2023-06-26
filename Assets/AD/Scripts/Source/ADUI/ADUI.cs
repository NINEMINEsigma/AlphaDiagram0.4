using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

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
        public string ElementArea = "null";

        public void OnPointerEnter(PointerEventData eventData)
        {
            Selected = true;
            UIArea = ElementArea;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Selected = false;
            UIArea = "null";
        }

        public IADUI Obtain(int serialNumber)
        {
            return Items.Find((P) => P.SerialNumber == serialNumber);
        }

        public IADUI Obtain(string elementName)
        {
            return Items.Find((P) => P.ElementName == elementName);
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
