using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

namespace AD.UI
{
    public abstract class PropertyModule : ADUI
    {
        [Header("PropertyModule")]
        public Dictionary<int, GameObject> Childs = new();
        private GridLayoutGroup _GridLayoutGroup = null;
        public virtual GridLayoutGroup GridLayoutGroup => IsNeedLayoutGourp ? _GridLayoutGroup : null;

        public PropertyModule()
        {
            ElementArea = "PropertyModule";
        }

        private bool _IsNeedLayoutGourp = false;
        protected virtual GridLayoutGroup HowGetOrAddGridLayoutGroup()
        {
            if (_GridLayoutGroup == null) return gameObject.GetOrAddComponent<GridLayoutGroup>();
            else return _GridLayoutGroup;
        }
        protected virtual bool IsNeedLayoutGourp
        {
            get
            {
                if (_IsNeedLayoutGourp)
                {
                    _GridLayoutGroup = HowGetOrAddGridLayoutGroup();
                    _GridLayoutGroup.enabled = true;
                }
                return _IsNeedLayoutGourp; 
            }
            set
            {
                if (value)
                {
                    _GridLayoutGroup = HowGetOrAddGridLayoutGroup();
                    _GridLayoutGroup.enabled = true;
                }
                else if (_GridLayoutGroup != null) _GridLayoutGroup.enabled = false;
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
                LetChildAdd(child);
            }
        }

        protected virtual void LetChildAdd(GameObject child)
        {
            child.transform.SetParent(transform, false);
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
