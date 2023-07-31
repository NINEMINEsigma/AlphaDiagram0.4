using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AD.BASE;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace AD.UI
{
    public abstract class PropertyModule : ADUI
    {
        [Header("PropertyModule")]
        public Dictionary<int, GameObject> Childs = new();
        private GridLayoutGroup _GridLayoutGroup = null;
        public virtual GridLayoutGroup GridLayoutGroup
        {
            get
            {
                if (IsNeedLayoutGourp) return _GridLayoutGroup;
                else return null;
            }
        }

        public PropertyModule()
        {
            ElementArea = "PropertyModule";
        }

        private bool _IsNeedLayoutGourp = false;
        protected virtual bool IsNeedLayoutGourp
        {
            get { return _IsNeedLayoutGourp; }
            set
            {
                if (value)
                {
                    _GridLayoutGroup ??= gameObject.GetOrAddComponent<GridLayoutGroup>();
                    _GridLayoutGroup.enabled = true;
                }
                else if (_GridLayoutGroup = null) _GridLayoutGroup.enabled = false;
                _IsNeedLayoutGourp = value;
            }
        }

        public GameObject this[int index]
        {
            get
            {
                return Childs[index];
            }
            set
            {
                Add(index,value);
            }
        }

        public void Add(int key, GameObject child)
        {
            if (Childs.ContainsKey(key)) 
                LetChildDestroy(Childs[key]); 
            if (child != null)
            {
                Childs[key] = child;
                child.transform.SetParent(transform, false);
            }
        }

        protected virtual void LetChildDestroy(GameObject child)
        {
            GameObject.Destroy(child);
        }

        public void Remove(int index)
        {
            Childs.Remove(index);
        }

        public void Remove(GameObject target)
        {
            var result = Childs.FirstOrDefault(T => T.Value == target);
            Remove(result.Key);
            GameObject.Destroy(result.Value);
        }

    }
}
