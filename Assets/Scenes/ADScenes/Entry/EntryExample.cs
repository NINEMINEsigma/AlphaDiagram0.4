using System.Collections;
using System.Collections.Generic;
using AD.BASE;
using AD.UI;
using UnityEngine;
using AD.Utility;
using System;

namespace AD.Experimental.Example.Entry 
{
    [Serializable]
    public class LineDrawer : AD.UI.ICanDrawLine
    {
        public Vector3 lt, lb, rb, rt;

        public LineDrawer(Vector3 lt, Vector3 lb, Vector3 rb, Vector3 rt)
        {
            this.lt = lt;
            this.lb = lb;
            this.rb = rb;
            this.rt = rt;
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
                keyframes[i].value = Mathf.Clamp(source.bands[i], 0.01f, 100);
            }
            m_curve.keys = keyframes;
            renderer.widthCurve = m_curve;
        }
    }

    public class EntryExample : SceneBaseController
    {
        public AudioSourceController AudioC;
        public Canvas TCanvas;
        public LineDrawer drawer;

        private void Start()
        {
            EntryApp.instance.RegisterController(this);
        }

        public override void Init()
        {
            Vector3[] rect = TCanvas.transform.As<RectTransform>().GetRect();
            AudioC.CurrentSourcePair.LineDrawer = drawer = new LineDrawer(rect[0], rect[1], rect[2], rect[3]);
            AudioC.Refresh();
            AudioC.Play();
        }
    }

    public class EntryApp : ADArchitecture<EntryApp>
    {
        public override bool FromMap(IBaseMap from)
        {
            throw new System.NotImplementedException();
        }

        public override IBaseMap ToMap()
        {
            throw new System.NotImplementedException();
        }
    }

}
