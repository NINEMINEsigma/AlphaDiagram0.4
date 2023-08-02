using UnityEngine;

namespace AD.ProjectTwilight.Choose
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
