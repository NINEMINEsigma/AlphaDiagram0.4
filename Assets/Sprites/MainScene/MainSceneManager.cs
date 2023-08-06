using System.Collections;
using AD.BASE;
using UnityEngine;

namespace AD.ProjectTwilight.MainScene
{
    public class MainSceneManager : AD.SceneBaseController
    {
        private void Start()
        {
            MainApp.instance.RegisterController(this);
        }

        public override void Init()
        {
            base.Init();

            RegisterCommand<NeedAddCurrentIndexWhenAutoMode>();

            OnSceneEnd.AddListener(() => { MainApp.instance.SaveRecord(); MainApp.Destory(); });
        }

        public void BackToEntry()
        {
            TargetSceneName = "Entry";
            PTApp.instance.SavePlayerModel();
            OnEnd();
        }

        bool isOpenAuto = false;

        public void ChangeAutoMode()
        {
            if (isOpenAuto) StopCoroutine(nameof(ChangeAutoModeClock));
            else StartCoroutine(ChangeAutoModeClock());
            isOpenAuto = !isOpenAuto;
        }

        IEnumerator ChangeAutoModeClock()
        {
            while (true)
            {
                for (float t = 0; t < Architecture.As<MainApp>().AudoDelayTime; t += UnityEngine.Time.deltaTime)
                {
                    yield return new WaitForEndOfFrame();
                }
                SendCommand<NeedAddCurrentIndexWhenAutoMode>();
            }
        }

        public void JumpIntoHistory()
        { 
            SendCommand<ActiveHistoryPanel>();
        }

        public void Skip()
        {
            isOpenAuto = false;
            StopCoroutine(nameof(ChangeAutoModeClock));
            Architecture.SendImmediatelyCommand<AddCurrentIndox>(new(1));
        }
    }
}
