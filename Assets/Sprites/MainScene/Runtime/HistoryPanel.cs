using System.Collections;
using System.Collections.Generic;
using AD.BASE;
using UnityEngine;

namespace AD.ProjectTwilight.MainScene
{
    public class HistoryPanel : ADController
    {
        public override void Init()
        {
            gameObject.SetActive(false);
        }

        public void SetActive()
        {
            gameObject.SetActive(true);
        }

        public void SetNotActive()
        {
            gameObject.SetActive(false);
        }

        private void Start()
        {
            MainApp.instance
                .RegisterController(this)
                .RegisterCommand<ActiveHistoryPanel>()
                .RegisterCommand<HideHistoryPanel>();
        }
    }

    public class ActiveHistoryPanel : ADCommand
    {
        public override void OnExecute()
        {
            GetController<HistoryPanel>().SetActive();
        }
    }

    public class HideHistoryPanel : ADCommand
    {
        public override void OnExecute()
        {
            GetController<HistoryPanel>().SetNotActive();
        }
    }

}
