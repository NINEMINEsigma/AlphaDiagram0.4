using System;
using AD.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AD.Experimental.Runtime.PipeEx
{
    public class LeftItem : AD.UI.ListViewItem,IButton
    {
        static PipeLineArchitecture Architecture => PipeLineArchitecture.instance;

        [SerializeField] GameObject FillPanel,ChoosingPanel;
        [SerializeField] Text TextSource;
        public Type TargetType;

        public override ListViewItem Init()
        {
            return this;
        }

        public override void InitializeContext()
        {
            Context.OnPointerEnterEvent = InitializeContextSingleEvent(Context.OnPointerEnterEvent, true, OnPointerEnter) ;
            Context.OnPointerExitEvent = InitializeContextSingleEvent(Context.OnPointerExitEvent, true, OnPointerExit);
            Context.OnPointerClickEvent = InitializeContextSingleEvent(Context.OnPointerClickEvent, false, OnPointerClick);
        }

        public void OnPointerClickOther()
        {
            ChoosingPanel.SetActive(false);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Architecture.GetModel<CurrentChoosingLeftItem>().current = this;
            ChoosingPanel.SetActive(true);
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            FillPanel.SetActive(true);
            SetTitle(TargetType.FullName);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData); 
            FillPanel.SetActive(false);
            SetTitle(TargetType.Name);
        }

        public void SetTitle(string title)
        {
            TextSource.SetText(title);
        }

        public void SetType(Type type)
        {
            TargetType = type;
        }

    }
}
