using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AD.UI
{
    public abstract class PropertyModule : ADUI
    {
        [Header("PropertyModule")]
        public List<GameObject> Childs = new List<GameObject>();
        public GameObject Parent = null;

    }
}
