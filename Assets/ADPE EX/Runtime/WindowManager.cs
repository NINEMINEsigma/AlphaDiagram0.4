using AD.UI;
using UnityEngine;

namespace AD.Experimental.Runtime.PipeEx
{
    public class WindowManager : CustomWindowElement
    {
        public static readonly int WindowSortLayerIndex = 500;

        protected override bool isSubPageUsingOtherSetting => true;

        public RectTransform MiddleItemParentArea => rectTransform;

        public string KeyName;

        public void Init(string keyName)
        {
            this.KeyName = keyName;
        } 

        public void SetThisOnTop()
        {

        }

        public void SetThisButtom()
        {

        }

    }
}
