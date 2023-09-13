using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AD.BASE;
using AD.UI;
using AD.Utility;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;

namespace AD.Experimental.Runtime.PipeEx
{
    [System.AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public sealed class ClassPipeLineFuncIgnoreAttribute : Attribute
    {

    }

    [System.AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public sealed class CPLFuncIgnoreAttribute : Attribute
    {

    }

    [System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class ClassPipeLineIgnoreAttribute : Attribute
    {

    }

    public static class PLAUtility
    {
        public static bool IsCPLFunc(this MethodBase method)
        {
            Attribute result = (method.GetCustomAttribute<CPLFuncIgnoreAttribute>() as Attribute) ?? method.GetCustomAttribute<ClassPipeLineFuncIgnoreAttribute>();
            return result == null;
        }

        public static List<MethodInfo> GetAllCPLFunc(this Type type)
        {
            return type.GetMethods(AD.Utility.ReflectionExtension.DefaultBindingFlags).GetSubList(T => T.IsCPLFunc());
        }

        public static bool IsCPLIgnore(this Type type)
        {
            if (type.Name[0] == '<') return true;
            return type.GetCustomAttribute<ClassPipeLineIgnoreAttribute>() != null;
        }

    }

    public class PipeLineArchitecture : ADArchitecture<PipeLineArchitecture>,IBase<PipeLineArchitecture_BM>
    {
        public override bool FromMap(IBaseMap from)
        {
            if (from == null) return false; 
            if (!from.As(out PipeLineArchitecture_BM bm)) throw new ADException("Type Error,Target Type:" + nameof(PipeLineArchitecture_BM));
            return FromMap(bm);
        }

        public override IBaseMap ToMap()
        {
            ToMap(out PipeLineArchitecture_BM bm);
            return bm;
        }

        public void ToMap(out IBaseMap BM)
        {
            ToMap(out PipeLineArchitecture_BM bm);
            BM = bm;
        }

        //*****************************************

        public override void Init()
        {
            RegisterModel<CurrentChoosingLeftItem>();
            RegisterModel<CurrentChoosingRightItem>();
            RegisterSystem<MiddleWindowGenerator>();
            RegisterSystem<SinglePanelGenerator>();

            ADGlobalSystem.AddListener(Mouse.current.rightButton, () =>
            {
                if (!Contains<CurrentMiddlePanelInfo>()) return;
                if (GetModel<CurrentMiddlePanelInfo>().Current == null) return;
                var window = GetController<PipeLineManager>().OnMenuInit(MiddleItem.OnRightClickMenu);
                window.rectTransform.position = GetModel<CurrentMiddlePanelInfo>().Current.transform.position - new Vector3(0, 0, -0.01f);
            }, PressType.ThisFramePressed);

        }

        public void ToMap(out PipeLineArchitecture_BM BM)
        {
            throw new System.NotImplementedException();
        }

        public bool FromMap(PipeLineArchitecture_BM from)
        {
            throw new System.NotImplementedException();
        }

        #region Tool

        public static void RegisterOnEventMenu(Dictionary<int, Dictionary<string, ADEvent>> Menu, string title, UnityAction action, int layer)
        {
            Menu.TryAdd(layer, new());
            Menu[layer].TryAdd(title, new());
            Menu[layer][title].AddListener(action);
        }

        public static void UnRegisterOnEventMenu(Dictionary<int, Dictionary<string, ADEvent>> Menu, string title, int layer)
        {
            if (Menu.TryGetValue(layer, out var dic))
                dic.Remove(title);
        }

        #endregion
    }

    [Serializable]
    public class PipeLineArchitecture_BM : IBaseMap<PipeLineArchitecture>
    {
        [Obsolete]
        public bool Deserialize(string source)
        {
            return false;
        }

        public void ToObject(out IBase obj)
        {
            ToObject(out PipeLineArchitecture _obj);
            obj = _obj;
        }

        public bool FromObject(IBase from)
        {
            if (from == null) return false;
            if(!from.As(out PipeLineArchitecture pla)) throw new ADException("Type Error,Target Type:" + nameof(PipeLineArchitecture));
            return FromObject(pla);
        }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }

        public void ToObject(out PipeLineArchitecture obj)
        {
            PipeLineArchitecture.instance.FromMap(this);
            obj = null;
        }

        public bool FromObject(PipeLineArchitecture from = null)
        {
            try
            {
                __FromObject();
            }
            catch (Exception ex)
            {
                string a = "Fail to create a " + nameof(PipeLineArchitecture_BM);
                PipeLineArchitecture.instance.AddMessage(a);
                ADGlobalSystem.TrackError(a, ex);
                return false;
            }
            return true;
        }

        //***************************************************************

        public PipeLineArchitecture_BM() { }

        private void __FromObject()
        {
            PipeLineArchitecture target = PipeLineArchitecture.instance;

        }

    }

    public class PipeLineManager : ADController
    {
        public Camera MainCamera;
        public Canvas MainCanvas;
        [SerializeField] ListView LeftItemListView, RightItemListView;
        [SerializeField] InputField LeftFieldOfChooseDLL;
        public TouchPanel TouchPanel;
        [SerializeField] RectTransform Mid_Panel;
        [SerializeField] MiddleItem DefaultMiddleItemPerfab;
        [SerializeField] WindowManager WindowManagerPerfab;
        [SerializeField] Button HamburgerMenu;
        [SerializeField] UnityEngine.UI.Image SinglePanelLinePerfab;
        [SerializeField] RectTransform SinglePanel_Panel; 
        [SerializeField] SinglePanel SinglePanelPerfab;

        private void Start()
        {
            PipeLineArchitecture.instance.RegisterController(this);
        }

        private void OnApplicationQuit()
        {
            PipeLineArchitecture.instance.SaveRecord();
            ADGlobalSystem.instance.SaveRecord();
        }

        public override void Init()
        {
            OnTouchPanelRightClickMenuEvent.Clear();
            OnHamburgerMenuEvent.Clear();

            OnTouchPanelRightClickMenuEventInit();

            LeftFieldOfChooseDLL.RemoveAllListener(InputField.PressType.OnEnd);
            LeftFieldOfChooseDLL.RemoveAllListener(InputField.PressType.OnSelect);
            LeftFieldOfChooseDLL.AddListener(TryGenerateAllLeftItemOfASM);

            OnHamburgerMenuEvent = new();
            HamburgerMenu.RemoveAllListener(PressType.ThisFramePressed);
            HamburgerMenu.RemoveAllListener(PressType.ThisFrameReleased);
            HamburgerMenu.AddListener(OnHamburgerMenu, PressType.ThisFramePressed);
            HamburgerMenu.AddListener(() =>
            {
                var a = GetSystem<SinglePanelGenerator>().Current;
                if (a != null) a.BackPool();
            }, PressType.ThisFrameReleased);

            TryGenerateAllLeftItemOfASM();

            GetSystem<MiddleWindowGenerator>().WindowPerfab = WindowManagerPerfab;
            GetSystem<MiddleWindowGenerator>().Parent = Mid_Panel;
            GetSystem<MiddleWindowGenerator>().ObtainElement("New Project", DefaultMiddleItemPerfab).As<WindowManager>().SetThisOnTop();

            GetSystem<SinglePanelGenerator>().WindowPerfab = SinglePanelPerfab;
            GetSystem<SinglePanelGenerator>().Parent = SinglePanel_Panel;
        }

        #region Left

        public void TryGenerateAllLeftItemOfASM(string dllName = "")
        {
            Assembly asm = null;
            try
            {
                asm = Assembly.Load(dllName);
            }
            catch
            {
                asm = Assembly.GetExecutingAssembly();
            }
            ClearLeftPanel();
            var types = asm.GetTypes().GetSubList(T => !T.IsCPLIgnore());
            types.Sort((T, P) => T.Name.CompareTo(P.Name));
            foreach (var type in types)
            {
                GenerateLeftItem(type.Name, type);
            }
        }

        public void GenerateLeftItem(string title,Type type)
        {
            var cat = LeftItemListView.GenerateItem() as LeftItem;
            cat.SetTitle(title);
            cat.SetType(type);
        }

        public class RefreshAllLeftItem : ADCommand
        {
            public override void OnExecute()
            {
                LeftItem current = GetModel<CurrentChoosingLeftItem>().current; 
                foreach (var _item in GetController<PipeLineManager>().LeftItemListView.Childs)
                {
                    var item = _item.Value.GetComponent<LeftItem>();
                    if (item == current) continue;
                    item.OnPointerClickOther();
                }
            }
        }

        public void ClearLeftPanel()
        {
            LeftItemListView.Clear();
        }

        #endregion

        #region Right

        public void TryGenerateAllRightItemOfType(Type type)
        {
            var funcs = type.GetAllCPLFunc();
            funcs.Sort((T,P)=>T.Name.CompareTo(P.Name));
            foreach (var method in funcs)
            {
                string argsstr = "";
                ParameterInfo[] args_p = method.GetParameters();
                if (args_p.Length > 0)
                    argsstr += args_p[0].ParameterType.Name;
                for (int i = 1, e = args_p.Length; i < e; i++)
                {
                    ParameterInfo arg_p = args_p[i];
                    argsstr += "," + arg_p.ParameterType.Name;
                }
                string fstr = method.ReturnType.Name + " " + method.Name + "(" + argsstr + ")";
                GenerateRightItem(fstr, method);
            }
        }

        public void GenerateRightItem(string title, MethodInfo method)
        {
            var cat = RightItemListView.GenerateItem() as RightItem;
            cat.SetTitle(title);
            cat.SetMethod(method);
        }

        public class RefreshAllRightItem : ADCommand
        {
            public override void OnExecute()
            {
                RightItem current = GetModel<CurrentChoosingRightItem>().current;
                foreach (var _item in GetController<PipeLineManager>().RightItemListView.Childs)
                {
                    var item = _item.Value.GetComponent<RightItem>();
                    if (item == current) continue;
                    item.OnPointerClickOther();
                }
            }
        }

        public void ClearRightPanel()
        {
            RightItemListView.Clear();
        }

        #endregion

        #region OnTouchPanelMenuAndEvent

        //<layer,<key,action>>
        public Dictionary<int, Dictionary<string, ADEvent>> OnTouchPanelRightClickMenuEvent = new();

        private Vector3 RightButtonClickArgVec;

        public void OnTouchPanelRightClickMenuEventInit()
        {
            Dictionary<string, ADEvent> dic = new();
            OnTouchPanelRightClickMenuEvent[MiddleItem.MiddleItemOperatorLayer] = dic;
            const string Key = "Create New";
            dic.Add(Key, new());
            dic[Key].AddListener(() => GetModel<CurrentMiddlePanelInfo>().SetupMiddleItem(RightButtonClickArgVec));
        }

        public void MidDragAction(Vector2 vec)
        {
            if (GetModel<CurrentMiddlePanelInfo>().IsOverBoundary()) return;
            GetModel<CurrentMiddlePanelInfo>().PosUpdate(vec);
        }

        public void RightButtonClick(Vector3 vec)
        {
            RightButtonClickArgVec = vec;
            var singlePanel = OnMenuInit(OnTouchPanelRightClickMenuEvent);
            var rects = singlePanel.rectTransform.GetRect(); 
            Vector3 lt = rects[0], rb = rects[2];
            singlePanel.rectTransform.position = new Vector3(vec.x + (rb.x - lt.x) * 0.5f, vec.y + (lt.y - rb.y) * 0.5f, vec.z - 0.01f);
        }

        public void RegisterOnTouchPanelRightClickMenu(string title, UnityAction action, int layer = 0) 
            => PipeLineArchitecture.RegisterOnEventMenu(OnTouchPanelRightClickMenuEvent, title, action, layer);

        public void UnRegisterOnTouchPanelRightClickMenu(string title, int layer) 
            => PipeLineArchitecture.UnRegisterOnEventMenu(OnTouchPanelRightClickMenuEvent, title, layer);

        #endregion

        #region OnHamburgerMenu

        //<layer,<key,action>>
        public Dictionary<int, Dictionary<string, ADEvent>> OnHamburgerMenuEvent = new();

        private void OnHamburgerMenu()
        {
            var singlePanel = OnMenuInit(OnHamburgerMenuEvent);
            var rects = singlePanel.rectTransform.GetRect();
            var targetRects = HamburgerMenu.GetComponent<RectTransform>().GetRect();
            Vector3 lt = rects[0], rb = rects[2];
            singlePanel.rectTransform.position = new Vector3(targetRects[0].x + (rb.x - lt.x) * 0.5f, targetRects[0].y + (lt.y - rb.y) * 0.5f, HamburgerMenu.transform.position.z - 0.01f);
        }

        public void RegisterOnHamburgerMenu(string title, UnityAction action, int layer = 0) 
            => PipeLineArchitecture.RegisterOnEventMenu(OnHamburgerMenuEvent, title, action, layer);

        public void UnRegisterOnHamburgerMenu(string title, int layer) 
            => PipeLineArchitecture.UnRegisterOnEventMenu(OnHamburgerMenuEvent, title, layer);

        #endregion

        #region ToolFunc

        public CustomWindowElement OnMenuInit(Dictionary<int, Dictionary<string, ADEvent>> Menu)
        {
            CustomWindowElement singlePanel = GetSystem<SinglePanelGenerator>().ObtainElement(new Vector2(200, 500)).SetTitle("Menu".Translate());

            int iiiindex = 0;
            foreach (var layer in Menu)
            {
                foreach (var item in layer.Value)
                {
                    singlePanel.GenerateButton(iiiindex.ToString() + "_" + item.Key, new Vector2(200, 23)).SetTitle(item.Key).AddListener(item.Value.Invoke);
                }
                singlePanel.SetItemOnWindow(iiiindex++.ToString() + "Layer", GameObject.Instantiate(SinglePanelLinePerfab.gameObject), new Vector2(200, 5));
            }

            return singlePanel;
        }

        #endregion

    }

    public class MiddleWindowGenerator : CustomWindowGenerator<MiddleWindowGenerator>
    {
        public CustomWindowElement ObtainElement(string keyName, MiddleItem MiddleItemPerfab)
        {
            var window = base.ObtainElement()
                .SetTitle(keyName) as WindowManager;
            window.HowBackPool = DespawnAndCallback;
            window.isCanDrag = false;
            window.transform.localPosition = Vector3.zero;
            CurrentMiddlePanelInfo cmpi = new();
            cmpi.Init(window.MiddleItemParentArea, MiddleItemPerfab, window.KeyName);
            Architecture.RegisterModel(cmpi);
            GetController<PipeLineManager>().RegisterOnHamburgerMenu(keyName, window.SetThisOnTop, WindowManager.WindowSortLayerIndex);
            return window;
        }

        public void DespawnAndCallback(CustomWindowElement element)
        {
            Despawn(element);
            GetController<PipeLineManager>().UnRegisterOnHamburgerMenu(element.As<WindowManager>().KeyName, WindowManager.WindowSortLayerIndex);
        }
    }

    public class SinglePanelGenerator : CustomWindowGenerator<SinglePanelGenerator>
    {
        public CustomWindowElement Current;

        public override CustomWindowElement ObtainElement()
        {
            if (Current != null) Current.BackPool();
            Current = base.ObtainElement();
            Current.OnEsc.AddListener(() => Current = null);
            return Current;
        }
    }

    public class CurrentChoosingLeftItem : AD.BASE.ADModel
    {
        private LeftItem CurrentChoosingLeftItemTarget;

        public LeftItem current
        {
            get => GetCirrent();
            set => SetCurrent(value);
        }

        private LeftItem GetCirrent()
        {
            return CurrentChoosingLeftItemTarget;
        }

        private void SetCurrent(LeftItem value)
        {
            if (CurrentChoosingLeftItemTarget == value) return;
            CurrentChoosingLeftItemTarget = value;
            Architecture
                .GetController<PipeLineManager>()
                .ClearRightPanel();
            if (value != null)
            {
                Architecture
                    .SendImmediatelyCommand<PipeLineManager.RefreshAllLeftItem>()
                    .GetController<PipeLineManager>()
                    .TryGenerateAllRightItemOfType(value.TargetType);
            }
        }

        public override void Init()
        {
            current = null;
        }

        public override IADModel Load(string path)
        {
            throw new NotImplementedException();
        }

        public override void Save(string path)
        {
            throw new NotImplementedException();
        }
    }

    public class CurrentChoosingRightItem : AD.BASE.ADModel
    {
        private RightItem CurrentChoosingRightItemTarget;

        public RightItem current
        {
            get => GetCirrent();
            set => SetCurrent(value);
        }

        private RightItem GetCirrent()
        {
            return CurrentChoosingRightItemTarget;
        }

        private void SetCurrent(RightItem value)
        {
            CurrentChoosingRightItemTarget = value;
            if (value != null) Architecture.SendImmediatelyCommand<PipeLineManager.RefreshAllRightItem>();
        }

        public override void Init()
        {
            current = null;
        }

        public override IADModel Load(string path)
        {
            throw new NotImplementedException();
        }

        public override void Save(string path)
        {
            throw new NotImplementedException();
        }
    }

    public class CurrentMiddlePanelInfo : ADModel
    {
        public static readonly int OperatorLayer = 0;
        public static readonly string Select = "Select";
        public static readonly string SelectNone = "SelectNone";

        public MiddleItem Current;

        public string KeyName = "Default";
        public RectTransform ParentArea;
        private GameObject Parent;
        public MiddleItem Perfab;
        //T R B L
        private RectTransform[] Boundary = new RectTransform[4];

        public Dictionary<int, Dictionary<string, ADEvent>> OnLeftClickMenu => MiddleItem.OnRightClickMenu;

        public void PosUpdate(Vector2 dragVec)
        {
            Parent.transform.localPosition += dragVec.ToVector3();
        }

        public bool IsOverBoundary()
        {
            var temp = ParentArea.GetRect();
            return
              Boundary[0] == null || (
            //左上，左下，右下，右上 
            Boundary[0].transform.position.y < temp[2].y ||
            Boundary[1].transform.position.x < temp[0].x ||
            Boundary[2].transform.position.y > temp[0].y ||
            Boundary[3].transform.position.x > temp[2].x);
        }

        public static bool IsOverBoundary(Transform _transform)
        {
            var window = PipeLineArchitecture.instance.GetModel<CurrentMiddlePanelInfo>().ParentArea.GetRect();
            return
            //左上，左下，右下，右上 
            _transform.position.y < window[2].y ||
            _transform.position.x < window[0].x ||
            _transform.position.y > window[0].y ||
            _transform.position.x > window[2].x;
        }

        public static bool IsOverBoundary(Vector3 Pos)
        {
            var window = PipeLineArchitecture.instance.GetModel<CurrentMiddlePanelInfo>().ParentArea.GetRect();
            return
            //左上，左下，右下，右上 
            Pos.y < window[2].y ||
            Pos.x < window[0].x ||
            Pos.y > window[0].y ||
            Pos.x > window[2].x;
        }

        public void SetupMiddleItem(Vector3 pos)
        {
            MiddleItem item = GameObject.Instantiate(Perfab.gameObject, Parent.transform).GetComponent<MiddleItem>();
            item.Init();
            RectTransform rect = item.GetComponent<RectTransform>();
            rect.position = pos;
            var recta = rect.GetRect();
            if (Boundary[0] == null)
            {
                Boundary[0] = Boundary[1] = Boundary[2] = Boundary[3] = rect;
            }
            else
            {
                //左上，左下，右下，右上 
                if (recta[0].y > Boundary[0].GetRect()[0].y)
                {
                    Boundary[0] = rect;
                }
                if (recta[2].x > Boundary[1].GetRect()[2].x)
                {
                    Boundary[1] = rect;
                }
                if (recta[2].y < Boundary[2].GetRect()[2].y)
                {
                    Boundary[2] = rect;
                }
                if (recta[0].y < Boundary[3].GetRect()[0].x)
                {
                    Boundary[3] = rect;
                }
            }
        }

        public override void Init()
        {
            OnLeftClickMenu.Clear();
            RegisterMiddleItemOperator();

            if (Parent == null) Parent = new GameObject("MiddlePanelInfo - CurrentParent");
            Parent.transform.SetParent(ParentArea, false);
            Parent.transform.localPosition = Vector3.zero;
        }

        private void RegisterMiddleItemOperator()
        {
            PipeLineArchitecture.RegisterOnEventMenu(OnLeftClickMenu, Select, () =>
            {
                if (Current != null &&
                Architecture.GetModel<CurrentChoosingLeftItem>().current.TargetType != null &&
                Architecture.GetModel<CurrentChoosingRightItem>().current.TargetMethod != null)
                {
                    Current.PipeStepPerproty = new()
                    {
                        BuilderType = Architecture.GetModel<CurrentChoosingLeftItem>().current.TargetType,
                        TargetMethod = Architecture.GetModel<CurrentChoosingRightItem>().current.TargetMethod,
                    };
                    var ats = Current.PipeStepPerproty.TargetMethod.GetParameters();
                    Current.PipeStepPerproty.ArgTypes = new Type[ats.Length];
                    for (int i = 0, e = ats.Length; i < e; i++)
                    {
                        Current.PipeStepPerproty.ArgTypes[i] = ats[i].ParameterType;
                    }
                    Current.PipeStepPerproty.ReturnType = Current.PipeStepPerproty.TargetMethod.ReturnType;
                    Current.ClearItemRef();
                    Current.UpdateInfo();
                }
            }, OperatorLayer);
            PipeLineArchitecture.RegisterOnEventMenu(OnLeftClickMenu, SelectNone, () =>
            {
                if (Current != null)
                {
                    Current.ClearItemRef();
                    Current.PipeStepPerproty = null;
                    Current.UpdateInfo();
                }
            }, OperatorLayer);
        }

        public void Init(RectTransform ParentArea, MiddleItem Perfab,string KeyName)
        {
            this.ParentArea = ParentArea;
            this.Perfab = Perfab;
            this.KeyName = KeyName;
        }

        public override IADModel Load(string path)
        {
            throw new NotImplementedException();
        }

        public override void Save(string path)
        {
            throw new NotImplementedException();
        }
    }

    public class TestPLAUtility
    { 
        private bool Test(int index,string str)
        {
            return false;
        }

    }

}
