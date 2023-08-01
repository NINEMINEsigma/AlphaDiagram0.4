using Michsky.UI.ModernUIPack;
using UnityEngine;

namespace AD.UI
{
    public class ModernUIButton : PropertyModule, IButton
    {
        [SerializeField] private ButtonManager _Source; 
        public ButtonManager Source
        {
            get
            {
                _Source ??= GetComponent<ButtonManager>();
                return _Source;
            }
        }

        public ModernUIButton()
        {
            ElementArea = "ModernUIButton";
        }

        protected void Start()
        {
            AD.UI.ADUI.Initialize(this);
        }

        protected void OnDestroy()
        {
            AD.UI.ADUI.Destory(this);
        }

        public ModernUIButton SetTitle(string title)
        {
            Source.buttonText = title;
            Source.UpdateUI();
            return this;
        }

        void IButton.SetTitle(string title)
        {
            this.SetTitle(title);
        }
    }
}
