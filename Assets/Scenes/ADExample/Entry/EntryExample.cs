using AD.BASE;
using AD.UI;
using UnityEngine;
using AD.Utility;
using System;

namespace AD.Experimental.Example.Entry
{
    public interface ICanTakeAnimationWithAudioSimple
    {
        void Refresh(float band);
    }

    [Serializable]
    public class LineDrawer : AD.UI.ICanDrawLine
    {
        public Vector3 lt, lb, rb, rt;
        public ICanTakeAnimationWithAudioSimple itemContainer;

        public LineDrawer(Vector3 lt, Vector3 lb, Vector3 rb, Vector3 rt, ICanTakeAnimationWithAudioSimple itemContainer)
        {
            this.lt = lt;
            this.lb = lb;
            this.rb = rb;
            this.rt = rt;
            this.itemContainer = itemContainer;
        }

        public void DrawLine(LineRenderer renderer, AudioSourceController source)
        {
            int vextcount = source.normalizedBands.Length;
            Keyframe[] keyframes = new Keyframe[vextcount];
            Vector3[] vexts = new Vector3[vextcount];
            vextcount = (int)source.BandCount;
            for (int i = 0; i < vextcount / 4; i++)
                vexts[i] = Vector3.Lerp(lt, lb, i / (float)(vextcount / 4 - 1));
            for (int i = 0; i < vextcount / 4; i++)
                vexts[i + vextcount / 4] = Vector3.Lerp(lb, rb, i / (float)(vextcount / 4 - 1));
            for (int i = 0; i < vextcount / 4; i++)
                vexts[i + vextcount / 2] = Vector3.Lerp(rb, rt, i / (float)(vextcount / 4 - 1));
            for (int i = 0; i < vextcount / 4; i++)
                vexts[i + vextcount / 4 * 3] = Vector3.Lerp(rt, lt, i / (float)(vextcount / 4 - 1));
            renderer.positionCount = vextcount;
            renderer.SetPositions(vexts);
            AnimationCurve m_curve = AnimationCurve.Linear(0, 1, 1, 1);
            for (int i = 0; i < vextcount; i++)
            {
                keyframes[i].time = i / (float)(vextcount - 1);
                keyframes[i].value = Mathf.Clamp(source.bands[i], 1, 100) * 10;
            }
            m_curve.keys = keyframes;
            renderer.widthCurve = m_curve;
            itemContainer?.Refresh(source.normalizedBands.Max());
        }
    }

    public class EntryExample : MonoBehaviour
    {
        [Header("Assets")]
        public AudioSourceController AudioC;
        public Canvas TCanvas;
        public RectTransform ButtomUI,TopUI;
        public LineDrawer drawer;
        [Header("Entry Animation")]
        public Animator eanimator;
        public string AnimationStartStr;
        public MonoBehaviour WhichCanTakeAnimationWithAudioSimple;
        [Header("Entry Runtime")]
        public AD.UI.Text MainTitle;
        public AD.UI.Text SubTitle;
        public string ProjectName = "NIGHT LIGHT";
        public bool IsRefreshMainTitleDefault = false;
        public bool IsSubTitleShowNowTime = false;
        public RectTransform LeftButtomButtonFill;

        private void Awake()
        { 
            Vector3[] rect = TCanvas.transform.As<RectTransform>().GetRect();
            if (AudioC != null)
            {
                AudioC.Refresh();
                AudioC.Play();
                if (AudioC.SourcePairs.Count > 0)
                    AudioC.CurrentSourcePair.LineDrawer = drawer = new LineDrawer(rect[0], rect[1], rect[2], rect[3], (WhichCanTakeAnimationWithAudioSimple == null)
                    ? null
                    : WhichCanTakeAnimationWithAudioSimple.GetComponent<ICanTakeAnimationWithAudioSimple>());
            }
            if (eanimator != null) eanimator.SetBool(AnimationStartStr, true);
            if (IsRefreshMainTitleDefault) 
                MainTitle.SetText("[ " + ProjectName + " ]"); 
        }

        private void Update()
        {
            if (IsSubTitleShowNowTime) 
                SubTitle.SetText(System.DateTime.Now.ToShortDateString() + "&" + System.DateTime.Now.ToShortTimeString()); 
        }

    }
}

