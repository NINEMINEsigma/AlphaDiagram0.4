using AD.UI;
using UnityEngine;

namespace AD.Experimental.Runtime.PipeEx
{
    public class WindowManager : CustomWindowElement
    {
        public static readonly int WindowSortLayerIndex = 500;

        protected override bool isSubPageUsingOtherSetting => true;

        public RectTransform MiddleItemParentArea => rectTransform;

        public AD.UI.TouchPanel TouchPanel;

        public string KeyName;

        public void Init(string keyName)
        {
            this.KeyName = keyName;
        } 

        public void SetThisOnTop()
        {
            PipeLineArchitecture.instance.GetController<PipeLineManager>().TouchPanel = this.TouchPanel;

            this.TouchPanel.TargetCamera = PipeLineArchitecture.instance.GetController<PipeLineManager>().MainCamera;

            TouchPanel.OnEvent.AddListener(PipeLineArchitecture.instance.GetController<PipeLineManager>().MidDragAction);
            TouchPanel.OnClickWhenCurrentWasPressRight.AddListener(PipeLineArchitecture.instance.GetController<PipeLineManager>().RightButtonClick);
        }

        public void SetThisButtom()
        {
            TouchPanel.RemoveAllListeners();
        }

    }
}
