using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AD.BASE;
using AD.UI;
using AD.Utility;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

namespace AD.Experimental.Runtime.PipeEx
{
    public class PipeStep : IBase<PipeStepInfo>
    {
        public static Dictionary<Type, object> BuilderInstances = new();

        public Type BuilderType;
        public MethodInfo TargetMethod;
        public Type ReturnType;
        public Type[] ArgTypes;

        public void Build()
        {
            if(!BuilderInstances.ContainsKey(BuilderType))
                BuilderInstances[BuilderType]= BuilderType.CreateInstance();
        }

        public object Invoke(object[] args)
        {
            Build();
            return TargetMethod.Invoke(BuilderInstances[BuilderType], args);
        }

        public bool FromMap(PipeStepInfo from)
        {
            try
            {
                this.BuilderType = ADGlobalSystem.FinalCheckWithThrow(ReflectionExtension.Typen(from.BuilderType), "Cannt find BuilderType");
                this.TargetMethod = ADGlobalSystem.FinalCheckWithThrow(this.BuilderType.GetMethod(from.TargetMethod), "Cannt find Method(" + from.TargetMethod + ") on Type(" + from.BuilderType + ")");
                if (this.TargetMethod.ReturnType.FullName != from.ReturnType)
                    throw new ADException("The name(" + this.TargetMethod.ReturnType.FullName + ") of method's ReturnType does not match the Target Name(" + from.ReturnType + ")");
                var Parameters = this.TargetMethod.GetParameters();
                if (Parameters.Length != from.ArgTypes.Count)
                    throw new ADException("Parameters(" + Parameters.Length.ToString() + ") of method does not match the Target Parameters(" + from.ArgTypes.Count.ToString() + ")");
                for (int i = 0, e = from.ArgTypes.Count; i < e; i++)
                {
                    string argType = from.ArgTypes[i];
                    if (Parameters[i].ParameterType.FullName != argType)
                        throw new ADException("The Parameter(" + Parameters[i].ParameterType.FullName + ") of method does not match the Target Parameter(" + argType + ")");
                }
            }
            catch(ADException adex)
            {
                ADGlobalSystem.AddError(adex.Message);
                return false;
            }
            catch(Exception ex)
            {
                ADGlobalSystem.TrackError("Unknow Error", ex);
                return false;
            }
            return true;
        }

        public bool FromMap(IBaseMap from)
        {
            if (!from.Convertible<PipeStepInfo>()) return false;
            return FromMap(from as PipeStepInfo);
        }

        public void ToMap(out PipeStepInfo BM)
        {
            BM = new();
            BM.FromObject(this);
        }

        public void ToMap(out IBaseMap BM)
        {
            BM = new PipeStepInfo();
            BM.FromObject(this);
        }
    }

    [Serializable]
    public class NextMiddleItemLeft
    {
        public LineRenderer LineRendererObject;
        public MiddleItem Next;
        public int ArgIndexAtNext = 0;
    }

    public class MiddleItem : MonoBehaviour
    {
        public static readonly int MiddleItemOperatorLayer = 500;

        public PipeStep PipeStepPerproty;
        public Text TypeText, MethodInfo;

        public LineRenderer LineRendererObjectPerfab;
        public static readonly float LineValue = 0.05f;
        public static readonly float LineLmtDistance = 1.5f;
        public static readonly int VertexCount = 4;//= 256;
        private readonly Vector3[] Vertexs = new Vector3[VertexCount];

        [SerializeField] private MiddleItem[] ArgLinkings;
        public List<NextMiddleItemLeft> Nexts = new();

        private BehaviourContext behaviourContext;
        private DragBehaviour _dragBehaviour;

        public static Dictionary<int, Dictionary<string, ADEvent>> OnRightClickMenu = new();

        public void ClearItemRef()
        {
            foreach (var item in Nexts)
            {
                CutLink(item.Next, item.ArgIndexAtNext);
            }
            for (int i = 0, e = ArgLinkings.Length; i < e; i++)
            {
                ArgLinkings[i].CutLink(this, i);
            }
        }

        private void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log("XX");
            PipeLineArchitecture.instance.GetModel<CurrentMiddlePanelInfo>().Current = this;
        }

        private void OnPointerExit(PointerEventData eventData)
        {
            PipeLineArchitecture.instance.GetModel<CurrentMiddlePanelInfo>().Current = null;
        }

        public void Init()
        {
            _dragBehaviour = this.GetOrAddComponent<DragBehaviour>();
            _dragBehaviour.Init(transform as RectTransform);
            behaviourContext = _dragBehaviour.GetBehaviourContext();

            behaviourContext.OnPointerEnterEvent ??= new();
            behaviourContext.OnPointerExitEvent ??= new();
            //behaviourContext.OnPointerClickEvent ??= new();

            behaviourContext.OnPointerEnterEvent.RemoveListener(this.OnPointerEnter);
            behaviourContext.OnPointerExitEvent.RemoveListener(this.OnPointerExit);
            //behaviourContext.OnPointerClickEvent.RemoveListener(this.OnPointerClick);

            behaviourContext.OnPointerEnterEvent.AddListener(this.OnPointerEnter);
            behaviourContext.OnPointerExitEvent.AddListener(this.OnPointerExit);
            //behaviourContext.OnPointerClickEvent.AddListener(this.OnPointerClick);

            UpdateInfo();
            UpdateLine();
        }

        public void UpdateInfo()
        {
            if (PipeStepPerproty == null)
            {
                TypeText.SetText("None");
                MethodInfo.SetText("------");
                return;
            }
            if (ArgLinkings == null || ArgLinkings.Length != PipeStepPerproty.ArgTypes.Length)
                ArgLinkings = new MiddleItem[PipeStepPerproty.ArgTypes.Length];
            TypeText.SetText(PipeStepPerproty.ReturnType.Name);
            string str = "";
            foreach (var SingleArgType in PipeStepPerproty.ArgTypes)
            {
                str += SingleArgType.Name;
            }
            MethodInfo.SetText(PipeStepPerproty.BuilderType.Name + "::" + PipeStepPerproty.TargetMethod.Name + "(" + str + ")");
        }

        public bool Check()
        {
            if (Nexts.Count == 0) return true;
            foreach (var SingleNext in Nexts)
            {

                if (SingleNext.ArgIndexAtNext >= SingleNext.Next.PipeStepPerproty.ArgTypes.Length ||
                    SingleNext.ArgIndexAtNext < 0 ||
                    SingleNext.Next.PipeStepPerproty.ArgTypes[SingleNext.ArgIndexAtNext] != this.PipeStepPerproty.ReturnType)
                    return false;
            }
            return true;
        }

        public Vector3 GetArgsLinkAt(int index)
        {
            var rect = transform.As<RectTransform>().GetRect();
            Vector3 Top = rect[1] + (rect[0] - rect[1]) * 0.75f, Buttom = rect[1] + (rect[0] - rect[1]) * 0.25f;
            return Vector3.Lerp(Top, Buttom, (float)index / (float)PipeStepPerproty.ArgTypes.Length);
        }

        public void UpdateLine()
        {
            float its = 1 / PipeLineArchitecture.instance.GetController<PipeLineManager>().MainCanvas.transform.localScale.x;
            foreach (var SingleNext in Nexts)
            {
                if (SingleNext.LineRendererObject == null)
                    SingleNext.LineRendererObject = GameObject.Instantiate(LineRendererObjectPerfab, transform);
                Vector3 temp = SingleNext.Next.GetArgsLinkAt(SingleNext.ArgIndexAtNext);
                if (CurrentMiddlePanelInfo.IsOverBoundary(temp)) continue;
                if (CurrentMiddlePanelInfo.IsOverBoundary(SingleNext.LineRendererObject.transform.position)) continue;
                Vector3 mpos = Vector3.zero, targetpos = (temp - SingleNext.LineRendererObject.transform.position) * its;
                Vertexs[0] = mpos;
                if (Mathf.Abs(mpos.x - targetpos.x) < LineLmtDistance * its)
                {
                    Vertexs[1] = mpos;
                    Vertexs[2] = targetpos;
                }
                else
                {
                    Vertexs[1] = mpos + new Vector3(LineLmtDistance * 0.5f * its, 0, 0);
                    Vertexs[2] = targetpos - new Vector3(LineLmtDistance * 0.5f * its, 0, 0);
                }
                Vertexs[3] = targetpos;
                SingleNext.LineRendererObject.positionCount = VertexCount;
                SingleNext.LineRendererObject.SetPositions(Vertexs);
                SingleNext.LineRendererObject.widthCurve = AnimationCurve.Linear(0, LineValue, 1, LineValue);
            }
        }

        public bool LinkTo(MiddleItem target, int index)
        {
            UpdateInfo();
            if (index >= target.PipeStepPerproty.ArgTypes.Length ||
                index < 0 ||
                target.PipeStepPerproty.ArgTypes[index] != this.PipeStepPerproty.ReturnType)
                return false;
            if (target.ArgLinkings.ElementAt(index) != null)
                target.ArgLinkings[index].CutLink(target, index);
            target.ArgLinkings[index] = this;
            this.Nexts.Add(new NextMiddleItemLeft() { ArgIndexAtNext = index, Next = target });
            UpdateLine();
            return true;
        }

        public void CutLink(MiddleItem target, int index)
        {
            if (target.ArgLinkings.ElementAt(index) != this) return;
            target.ArgLinkings[index] = null;
            NextMiddleItemLeft cat = this.Nexts.Find(T => T.Next == target);
            this.Nexts.Remove(cat);
            GameObject.Destroy(cat.LineRendererObject.gameObject);
            UpdateLine();
        }
    }

    [Serializable]
    public class PipeStepInfo : IBaseMap<PipeStep>
    {
        public string BuilderType, TargetMethod, ReturnType;
        public List<string> ArgTypes;

        public bool Deserialize(string source)
        {
            try
            {
                string[] strs = source.Split(' ', '(', ')', ',');
                if (strs.Length < 2) return false;
                if (strs.Length >= 2)
                {
                    ReturnType = strs[0];
                    string[] typespace = strs[1].Split("::");
                    if (typespace.Length != 2) return false;
                    BuilderType = typespace[0];
                    TargetMethod = typespace[1];
                    for (int i = 3; i < strs.Length; i++)
                    {
                        ArgTypes.Add(strs[i]);
                    }
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool FromObject(PipeStep from)
        {
            try
            {
                BuilderType = from.BuilderType.FullName;
                TargetMethod = from.TargetMethod.Name;
                ReturnType = from.ReturnType.FullName;
                ArgTypes.Clear();
                foreach (var argRype in from.ArgTypes)
                {
                    ArgTypes.Add(argRype.FullName);
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool FromObject(IBase from)
        {
            if (!from.Convertible<PipeStep>()) return false;
            return FromObject(from as PipeStep);
        }

        public string Serialize()
        {
            string result = ReturnType + " " + BuilderType + "::" + TargetMethod;
            result += "(";
            if (ArgTypes.Count > 0) result += ArgTypes[0];
            for (int i = 1, e = ArgTypes.Count; i < e; i++)
            {
                result += "," + ArgTypes[i];
            }
            result += ")";
            return result;
        }

        public void ToObject(out PipeStep obj)
        {
            obj = new PipeStep();
            obj.BuilderType = ADGlobalSystem.FinalCheckWithThrow(ReflectionExtension.Typen(BuilderType),"Cannt find BuilderType");
            obj.TargetMethod = ADGlobalSystem.FinalCheckWithThrow(obj.BuilderType.GetMethod(TargetMethod), "Cannt find Method(" + TargetMethod + ") on Type(" + BuilderType + ")");
            if (obj.TargetMethod.ReturnType.FullName != ReturnType) 
                throw new ADException("The name("+ obj.TargetMethod.ReturnType.FullName + ") of method's ReturnType does not match the Target Name("+ ReturnType + ")");
            var Parameters = obj.TargetMethod.GetParameters();
            if (Parameters.Length != ArgTypes.Count)
                throw new ADException("Parameters(" + Parameters.Length.ToString() + ") of method does not match the Target Parameters(" + ArgTypes.Count.ToString() + ")");
            for (int i = 0, e = ArgTypes.Count; i < e; i++)
            {
                string argType = ArgTypes[i];
                if (Parameters[i].ParameterType.FullName != argType)
                    throw new ADException("The Parameter(" + Parameters[i].ParameterType.FullName + ") of method does not match the Target Parameter(" + argType + ")");
            }
        }

        public void ToObject(out IBase obj)
        {
            ToObject(out PipeStep step);
            obj = step;
        }
    }

}
