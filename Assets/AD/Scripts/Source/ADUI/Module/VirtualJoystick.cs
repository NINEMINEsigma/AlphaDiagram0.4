using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AD.Utility;
using UnityEngine.EventSystems;
using AD.ADbase;
using static UnityEngine.Rendering.DebugUI;

namespace AD.UI
{
    public class VirtualJoystick : PropertyModule, IDragHandler, IInitializePotentialDragHandler, IEndDragHandler
    {
        [Header("VirtualJoystick")]
        public Camera TargetCamera;

        public Image MineVirtualJoystick;
        public Image Background;
        public Image Fill;
        public Image JoyStick;

        public Vector3 Value = new Vector3();

        public ADEvent<PointerEventData> OnDragEvent = new ADEvent<PointerEventData>(),
            OnStart = new ADEvent<PointerEventData>(),
            OnEnd = new ADEvent<PointerEventData>();

        private void Start()
        {
            AD.UI.ADUI.Initialize(this);

            MineVirtualJoystick.SetColor_A(0);
            Background.SetColor_A(0);
            JoyStick.SetColor_A(0);
            Fill.fillAmount = 0;

            var vertexs = MineVirtualJoystick.rectTransform.GetRect();
            MaxX = Vector3.Distance(vertexs[1], vertexs[2]);
        }

        public VirtualJoystick()
        {
            ElementArea = "VirtualJoystick";
        }

        private void OnDestroy()
        {
            AD.UI.ADUI.Destory(this);
        }

        private bool IsStart = false, IsEnd = false;
        private float MaxX;
        [HideInInspector] public float DateValue = 180;

        public void OnDrag(PointerEventData eventData)
        {
            OnDragEvent.Invoke(eventData);
            var Pos = TargetCamera.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, Vector3.Distance(transform.position, TargetCamera.transform.position)));
            Value = (Pos - transform.position).normalized;
            if ((Pos - transform.position).magnitude > MaxX / 2.0f) Pos = transform.position + Value * MaxX / 2.0f;
            JoyStick.transform.position = new Vector3(Pos.x, Pos.y, JoyStick.transform.position.z);
            float angle = DateValue / 2.0f - 90 + Mathf.Atan2(JoyStick.transform.position.y - transform.position.y, JoyStick.transform.position.x - transform.position.x) * 180 / Mathf.PI;
            Fill.rectTransform.eulerAngles = new Vector3(0, 0, angle);

        }
        public IEnumerator OnDragKeep()
        {
            DateValue = 180;
            while (DateValue > 30)
            {
                DateValue -= 1.5f;
                Fill.fillAmount = DateValue / 360.0f;
                Value.z = DateValue;
                yield return new WaitForEndOfFrame();
            }
        }

        public void OnInitializePotentialDrag(PointerEventData eventData)
        {
            OnStart.Invoke(eventData);
            StopCoroutine(nameof(OnInitializePotentialDrag_Start));
            StartCoroutine(OnInitializePotentialDrag_Start());

            OnDragEvent.Invoke(eventData);
            var Pos = TargetCamera.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, Vector3.Distance(transform.position, TargetCamera.transform.position)));
            if ((Pos - transform.position).magnitude > MaxX / 2.0f) Pos = transform.position + (Pos - transform.position).normalized * MaxX / 2.0f;
            JoyStick.transform.position = new Vector3(Pos.x, Pos.y, JoyStick.transform.position.z);
            float angle = DateValue / 2.0f - 90 + Mathf.Atan2(JoyStick.transform.position.y - transform.position.y, JoyStick.transform.position.x - transform.position.x) * 180 / Mathf.PI;
            Fill.rectTransform.eulerAngles = new Vector3(0, 0, angle);
        }
        private IEnumerator OnInitializePotentialDrag_Start()
        {
            while (IsEnd) yield return new WaitForEndOfFrame();
            IsStart = true;
            Value = new Vector3();
            float end = 15.0f;
            for (int i = 0; i < end; i++)
            {
                MineVirtualJoystick.SetColor_A(i / end);
                Background.SetColor_A(i / end);
                JoyStick.SetColor_A(i / end);
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForEndOfFrame();
            MineVirtualJoystick.SetColor_A(1);
            Background.SetColor_A(1);
            JoyStick.SetColor_A(1);
            JoyStick.transform.localPosition = new Vector3();
            IsStart = false;
            StartCoroutine(OnDragKeep());
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            OnEnd.Invoke(eventData);
            StopCoroutine(nameof(OnInitializePotentialDrag_End));
            StartCoroutine(OnInitializePotentialDrag_End());
        }
        private IEnumerator OnInitializePotentialDrag_End()
        {
            while (IsStart) yield return new WaitForEndOfFrame();
            IsEnd = true;
            float end = 15.0f;
            DateValue = 0;
            Value = new Vector3();
            Fill.fillAmount = DateValue;
            for (int i = 0; i < end; i++)
            {
                MineVirtualJoystick.SetColor_A(1 - i / end);
                Background.SetColor_A(1 - i / end);
                JoyStick.SetColor_A(1 - i / end);
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForEndOfFrame();
            MineVirtualJoystick.SetColor_A(0);
            Background.SetColor_A(0);
            JoyStick.SetColor_A(0);
            JoyStick.transform.localPosition = new Vector3();
            IsEnd = false;
            StopCoroutine(nameof(OnDragKeep));
        }
    }
}
