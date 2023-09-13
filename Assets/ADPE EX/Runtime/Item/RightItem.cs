using System.Reflection;
using AD.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AD.Experimental.Runtime.PipeEx
{
    public class RightItem : AD.UI.ListViewItem, IButton
    {
        static PipeLineArchitecture Architecture => PipeLineArchitecture.instance;

        [SerializeField] GameObject FillPanel, ChoosingPanel;
        [SerializeField] Text TextSource;
        public MethodInfo TargetMethod;

        public override ListViewItem Init()
        {
            return this;
        }

        public override void InitializeContext()
        {
            Context.OnPointerEnterEvent = InitializeContextSingleEvent(Context.OnPointerEnterEvent, false, OnPointerEnter);
            Context.OnPointerExitEvent = InitializeContextSingleEvent(Context.OnPointerExitEvent, false, OnPointerExit);
            Context.OnPointerClickEvent = InitializeContextSingleEvent(Context.OnPointerClickEvent, false, OnPointerClick);
        }

        public void OnPointerClickOther()
        {
            ChoosingPanel.SetActive(false);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Architecture.GetModel<CurrentChoosingRightItem>().current = this;
            ChoosingPanel.SetActive(true);
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            FillPanel.SetActive(true);
            string argsstr = "";
            ParameterInfo[] args_p =TargetMethod.GetParameters(); 
            if(args_p.Length>0)
            argsstr += args_p[0].ParameterType.FullName;
            for (int i = 1, e = args_p.Length; i < e; i++)
            {
                ParameterInfo arg_p = args_p[i];
                argsstr += "," + arg_p.ParameterType.FullName;
            }
            string fstr = TargetMethod.ReturnType.FullName + " " + TargetMethod.Name + "(" + argsstr + ")";
            SetTitle(fstr);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            FillPanel.SetActive(false);
            string argsstr = "";
            ParameterInfo[] args_p = TargetMethod.GetParameters();
            if (args_p.Length > 0)
                argsstr += args_p[0].ParameterType.Name;
            for (int i = 1, e = args_p.Length; i < e; i++)
            {
                ParameterInfo arg_p = args_p[i];
                argsstr += "," + arg_p.ParameterType.Name;
            }
            string fstr = TargetMethod.ReturnType.Name + " " + TargetMethod.Name + "(" + argsstr + ")";
            SetTitle(fstr);
        }

        public void SetTitle(string title)
        {
            TextSource.SetText(title);
        }

        public void SetMethod(MethodInfo method)
        {
            TargetMethod = method;
        }

    }
}
