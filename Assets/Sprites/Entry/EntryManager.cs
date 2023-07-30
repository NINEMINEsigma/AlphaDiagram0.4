using System.Collections;
using UnityEngine;
using AD.UI;
using UnityEngine.Rendering;

namespace AD.Entry
{
    public class DrawLine : AD.UI.ICanDrawLine
    {
        void ICanDrawLine.DrawLine(LineRenderer renderer, AudioSourceController source)
        {
            int vextcount = source.normalizedBands.Length;
            Keyframe[] keyframes = new Keyframe[vextcount];
            Vector3[] vexts = new Vector3[vextcount];
            vextcount = (int)source.BandCount;
            for (int i = 0; i < vextcount; i++)
                vexts[i] = Vector3.Lerp(line_left, line_right, i / (float)(vextcount - 1));
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

        public DrawLine(RectTransform rect)
        {
            var cat = rect.GetRect();
            line_left = cat[1];
            line_right = cat[2];
        }

        Vector3 line_left, line_right;
    }

    //Main Controller
    public class EntryManager : AD.SceneBaseController
    {
        private void Start()
        {
            EntryApp.instance.RegisterController(this);
        }

        public override void Init()
        {
            base.Init();
            Level1GUI.SetActive(true);
            StartCoroutine(SetLevelOnActive(Level1GUI, true));
            Level2GUI.SetActive(false); 
            var bloom = GetComponent<Volume>();
            bloom.weight = 0;
            audioSource.CurrentSourcePair.LineDrawer = new DrawLine(DrawCanvas.transform as RectTransform);
        }

        [SerializeField] Canvas DrawCanvas;
        [SerializeField] GameObject Level1GUI, Level2GUI;
        [SerializeField] VolumeProfile MainVolumeProfile;
        [SerializeField] AD.UI.AudioSourceController audioSource;

        //View

        public void SetLevel(int level)
        {
            if (level != 1) Level1GUI.SetActive(false);
            else
            {
                Level1GUI.SetActive(true);
                StartCoroutine(SetLevelOnActive(Level1GUI, true));
            }
            if (level != 2) Level2GUI.SetActive(false);
            else
            {
                Level2GUI.SetActive(true);
                StartCoroutine(SetLevelOnActive(Level2GUI, true));
            }

            StopCoroutine(nameof(SetBloom));
            StartCoroutine(SetBloom(level == 2));

            Architecture.GetController<BackgroundView>().SetLevel(level);
        }

        IEnumerator SetLevelOnActive(GameObject Level, bool value)
        {
            float clock = Time.time;
            if (value)
                while (Time.time - clock < 1)
                {
                    Level.transform.localPosition = new Vector3(-60 * (1 - (Time.time - clock)), 0, Level.transform.localPosition.z);
                    yield return new WaitForEndOfFrame();
                }
            else yield return null;
            Level.transform.localPosition = Vector3.zero;
        }

        IEnumerator SetBloom(bool value)
        {
            var bloom = GetComponent<Volume>();
            while ((bloom.weight < 1 && value) || (bloom.weight > 0 && !value))
            {
                bloom.weight += Time.deltaTime * (value ? 1 : -1) * 0.2f;
                yield return new WaitForEndOfFrame();
            }
            bloom.weight = (value ? 1 : 0);
        }

    }
}