using AD.UI;

namespace AD.Experimental.Runtime.PipeEx
{
    public class SinglePanel : CustomWindowElement
    {
        protected override bool isSubPageUsingOtherSetting => true;

        public override CustomWindowElement Init()
        { 
            base.Init(); 
            isCanRefresh = false;
            return this;
        }
    }
}