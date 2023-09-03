using System.Collections;
using System.Collections.Generic;
using AD.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AD.Experimental.Runtime.PipeEx
{
    public class LeftItem : AD.UI.ListViewItem,IPointerClickHandler,IButton
    {
        static PipeLineArchitecture Architecture => PipeLineArchitecture.instance;

        [SerializeField] GameObject FillPanel,ChoosingPanel;
        [SerializeField] Text TextSource;

        public override ListViewItem Init()
        {
            return this;
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
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData); 
            FillPanel.SetActive(false);
        }

        public void SetTitle(string title)
        {
            TextSource.SetText(title);
        }
    }
}
