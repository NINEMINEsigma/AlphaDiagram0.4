using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AD.Choose
{
    public class CanTransformScene : MonoBehaviour
    {
        public string target;

        public void Transform()
        {
            ChooseApp.TargetSceneName = target;
            ChooseApp.OnEnd();
        }
    }
}
