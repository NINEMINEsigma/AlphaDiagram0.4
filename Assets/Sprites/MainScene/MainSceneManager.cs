using System.Collections;
using System.Collections.Generic;
using AD.BASE;
using UnityEngine;
using UnityEngine.UI;

namespace AD.ProjectTwilight.MainScene
{
    public class MainSceneManager : AD.SceneBaseController
    {
        [Header("Assets")]
        [SerializeField] List<Image> Images = new List<Image>();

        private void Start()
        {
            MainApp.instance.RegisterController(this);

            SceneSingleAssets.CoroutineWorker.StartCoroutine(BGInit());
        }

        public override void Init()
        {
            base.Init();

            RegisterCommand<NeedAddCurrentIndexWhenAutoMode>();
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
            Architecture.GetController<HistoryPanel>().SetActive();
        }

        private IEnumerator BGInit()
        {
            foreach (var image in Images) image.gameObject.SetActive(true);
            for (float t = 0; t < 1; t += UnityEngine.Time.deltaTime)
            {
                foreach (var image in Images) image.color = new Color(image.color.r, image.color.g, image.color.b, 1 - t);
                yield return new WaitForEndOfFrame();
            }
            foreach (var image in Images) image.gameObject.SetActive(false);
        }
    }
}
