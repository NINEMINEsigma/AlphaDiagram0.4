using UnityEngine;
using UnityEngine.UI;

namespace AD.ProjectTwilight.Entry
{
    public class TopResultView : AD.BASE.ADController
    {
        private void Awake()
        {
            EntryApp.instance.RegisterController(this);
        }

        [SerializeField] Image viewBackground;
        [SerializeField] AD.UI.Text text;
        [SerializeField] Animator animator;

        public override void Init()
        {
            animator.SetBool("Achievement", false);
            animator.SetBool("NewStart", false);
            animator.SetBool("Start", false);
        }

        public void ChooseState(string value)
        {
            Init();
            animator.SetBool(value, true);
        }

        public void SetTitle(string text)
        {
            this.text.text = text;
        }

        public void WhereSceneTarget(string scene)
        {
            EntryApp.TargetSceneName = scene;
        }

        public void OnEnd()
        {
            EntryApp.OnEnd();
        }

    }
}
