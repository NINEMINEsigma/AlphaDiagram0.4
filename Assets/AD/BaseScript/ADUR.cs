using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AD
{
    [Serializable]
    public class ADUR : MonoBehaviour
    {
        public List<string> messages = new List<string>();

        private void OnDestroy()
        {
            AD.ADUtility._Destory();
        }
    }
}