using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AD.UI
{
    [Serializable, RequireComponent(typeof(UnityEngine.UI.RawImage))]
    public class RawImage : AD.UI.ADUI
    {
        public RawImage()
        {
            ElementArea = "RawImage";
        }

        protected void Start()
        {
            AD.UI.ADUI.Initialize(this);
        }

        protected void OnDestroy()  
        {
            AD.UI.ADUI.Destory(this);
        }

#if UNITY_EDITOR
        [MenuItem("GameObject/AD/RawImage", false, 10)]
        private static void ADD(UnityEditor.MenuCommand menuCommand)
        {
            AD.UI.RawImage rawImage;
            if (ADGlobalSystem.instance != null && ADGlobalSystem.instance._RawImage != null)
            {
                rawImage = GameObject.Instantiate(ADGlobalSystem.instance._Button) as AD.UI.RawImage;
            }
            else
            {
                rawImage = new GameObject().AddComponent<AD.UI.RawImage>();
            }
            rawImage.name = "New RawImage";
            GameObjectUtility.SetParentAndAlign(rawImage.gameObject, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(rawImage.gameObject, "Create " + rawImage.name);
            Selection.activeObject = rawImage.gameObject;
        }
#endif

        public static AD.UI.RawImage Generate(string name = "New RawImage", Transform parent = null, params System.Type[] components)
        {
            AD.UI.RawImage rawImage = null;
            if (ADGlobalSystem.instance._Slider != null)
            {
                rawImage = GameObject.Instantiate(ADGlobalSystem.instance._RawImage, parent) as AD.UI.RawImage;
            }
            else
            {
                rawImage = new GameObject("New RawImage", components).AddComponent<AD.UI.RawImage>();
            }

            rawImage.transform.SetParent(parent, false);
            rawImage.transform.localPosition = Vector3.zero;
            rawImage.name = name;
            foreach (var component in components) rawImage.gameObject.AddComponent(component);

            return rawImage;
        }

        private UnityEngine.UI.RawImage _source;
        public UnityEngine.UI.RawImage source
        {
            get
            {
                _source ??= GetComponent<UnityEngine.UI.RawImage>();
                return _source;
            }
        }
    }
}