using System;
using System.Collections.Generic;
using AD.UI;
using UnityEngine;

namespace AD.Experimental.Performance
{
    /// <summary>
    /// Debug ����camera�� �ӿڡ���׶ �� FOV
    /// </summary>
    /// 
    [ExecuteInEditMode,RequireComponent(typeof( Camera))]
    public class ConeAllegation : MonoBehaviour
    {
        public float _farDistance = 10;//Զ�ӿھ���
        public float _nearDistance = 3;//���ӿھ���

        private Camera _camera;
        private Camera TargetCamera
        {
            get
            {
                if (_camera == null) _camera = GetComponent<Camera>();
                return _camera;
            }
        }

        /// <summary>
        /// ����ͼ��
        /// </summary>
        void OnDrawGizmos()
        {
            OnDrawFarView();
            OnDrawNearView();
            OnDrawFOV();
            OnDrawConeOfCameraVision();
        }

        /// <summary>
        /// ���ƽ�Զ���ӿ�
        /// </summary>
        void OnDrawFarView()
        {
            Vector3[] corners = GetCorners(_farDistance);

            // for debugging
            Debug.DrawLine(corners[0], corners[1], Color.red); // UpperLeft -> UpperRight
            Debug.DrawLine(corners[1], corners[3], Color.red); // UpperRight -> LowerRight
            Debug.DrawLine(corners[3], corners[2], Color.red); // LowerRight -> LowerLeft
            Debug.DrawLine(corners[2], corners[0], Color.red); // LowerLeft -> UpperLeft


            //������
            Vector3 vecStart = TargetCamera.transform.position;
            Vector3 vecEnd = vecStart;
            vecEnd += TargetCamera.transform.forward * _farDistance;
            Debug.DrawLine(vecStart, vecEnd, Color.red);
        }

        /// <summary>
        /// ���ƽϽ����ӿ�
        /// </summary>
        void OnDrawNearView()
        {
            Vector3[] corners = GetCorners(_nearDistance);

            // for debugging
            Debug.DrawLine(corners[0], corners[1], Color.green);//����-����
            Debug.DrawLine(corners[1], corners[3], Color.green);//����-����
            Debug.DrawLine(corners[3], corners[2], Color.green);//����-����
            Debug.DrawLine(corners[2], corners[0], Color.green);//����-����
        }

        /// <summary>
        /// ���� camera �� FOV
        /// </summary>
        void OnDrawFOV()
        {
            float halfFOV = (_camera.fieldOfView * 0.5f) * Mathf.Deg2Rad;//һ��fov
            float halfHeight = _farDistance * Mathf.Tan(halfFOV);//distance����λ�ã�����ӿڸ߶ȵ�һ��

            //���
            Vector3 vecStart = TargetCamera.transform.position;

            //����
            Vector3 vecUpCenter = vecStart;
            vecUpCenter.y -= halfHeight;
            vecUpCenter.z += _farDistance;

            //����
            Vector3 vecBottomCenter = vecStart;
            vecBottomCenter.y += halfHeight;
            vecBottomCenter.z += _farDistance;

            Debug.DrawLine(vecStart, vecUpCenter, Color.blue);
            Debug.DrawLine(vecStart, vecBottomCenter, Color.blue);
        }

        /// <summary>
        /// ���� camera ����׶ ����
        /// </summary>
        void OnDrawConeOfCameraVision()
        {
            Vector3[] corners = GetCorners(_farDistance);

            var CameraTransform = TargetCamera.transform;
            // for debugging
            Debug.DrawLine(CameraTransform.position, corners[1], Color.green); // UpperLeft -> UpperRight
            Debug.DrawLine(CameraTransform.position, corners[3], Color.green); // UpperRight -> LowerRight
            Debug.DrawLine(CameraTransform.position, corners[2], Color.green); // LowerRight -> LowerLeft
            Debug.DrawLine(CameraTransform.position, corners[0], Color.green); // LowerLeft -> UpperLeft
        }

        //��ȡ����ӿ��ĸ��ǵ�����
        //���� distance  �ӿھ���
        Vector3[] GetCorners(float distance)
        {
            Vector3[] corners = new Vector3[4];

            //fovΪ��ֱ��Ұ  ˮƽfovȡ�����ӿڵĿ�߱�  �Զ�Ϊ��λ

            var CameraTransform = TargetCamera.transform;

            float halfFOV = (_camera.fieldOfView * 0.5f) * Mathf.Deg2Rad;//һ��fov
            float aspect = _camera.aspect;//����ӿڿ�߱�

            float height = distance * Mathf.Tan(halfFOV);//distance����λ�ã�����ӿڸ߶ȵ�һ��
            float width = height * aspect;//����ӿڿ�ȵ�һ��

            //����
            corners[0] = CameraTransform.position - (CameraTransform.right * width);//������� - �ӿڿ��һ��
            corners[0] += CameraTransform.up * height;//+�ӿڸߵ�һ��
            corners[0] += CameraTransform.forward * distance;//+�ӿھ���

            // ����
            corners[1] = CameraTransform.position + (CameraTransform.right * width);//������� + �ӿڿ��һ��
            corners[1] += CameraTransform.up * height;//+�ӿڸߵ�һ��
            corners[1] += CameraTransform.forward * distance;//+�ӿھ���

            // ����
            corners[2] = CameraTransform.position - (CameraTransform.right * width);//������� - �ӿڿ��һ��
            corners[2] -= CameraTransform.up * height;//-�ӿڸߵ�һ��
            corners[2] += CameraTransform.forward * distance;//+�ӿھ���

            // ����
            corners[3] = CameraTransform.position + (CameraTransform.right * width);//������� + �ӿڿ��һ��
            corners[3] -= CameraTransform.up * height;//-�ӿڸߵ�һ��
            corners[3] += CameraTransform.forward * distance;//+�ӿھ���

            return corners;
        }

        [Flags]
        public enum RectInFrustumKey
        {
            None = 0,
            LeftTop = 1 << 0,
            RightTop = 1 << 1,
            LeftButtom = 1 << 2,
            RightButtom = 1 << 3,
        }

        public RectInFrustumKey IsRectInFrustum(RectTransform rectTransform)
        {
            Plane[] planes = GetFrustumPlanes();
            Vector3[] rectP = rectTransform.GetRect();
            RectInFrustumKey rectE = (RectInFrustumKey)(1 << 4 - 1);
            for (int i = 0, iMax = planes.Length; i < iMax; ++i)
            {
                switch (rectE)
                {
                    case RectInFrustumKey.LeftTop when !planes[i].GetSide(rectP[0]):
                        rectE -= RectInFrustumKey.LeftTop;
                        break;
                    case RectInFrustumKey.RightTop when !planes[i].GetSide(rectP[1]):
                        rectE -= RectInFrustumKey.RightTop;
                        break;
                    case RectInFrustumKey.LeftButtom when !planes[i].GetSide(rectP[2]):
                        rectE -= RectInFrustumKey.LeftButtom;
                        break;
                    case RectInFrustumKey.RightButtom when !planes[i].GetSide(rectP[3]):
                        rectE -= RectInFrustumKey.RightTop;
                        break;
                    default:
                        break;
                }
            }
            return rectE;
        }

        public bool IsPointInFrustum(Vector3 point)
        {
            Plane[] planes = GetFrustumPlanes();

            for (int i = 0, iMax = planes.Length; i < iMax; ++i)
            {
                //�ж�һ�����Ƿ���ƽ�����������
                if (!planes[i].GetSide(point))
                {
                    return false;
                }
            }
            return true;
        }

        Plane[] GetFrustumPlanes()
        {
            return GeometryUtility.CalculateFrustumPlanes(TargetCamera);
        }

        public List<ConeAllegationItem> Items = new();

        private void Update()
        {
            foreach (var item in Items)
            {
                if (item.OnEnCone.GetPersistentEventCount() + item.OnQuCone.GetPersistentEventCount() == 0)
                {
                    item.gameObject.SetActive(true);
                }
                item.IsOnCone = true;
            }

            Plane[] planes = GetFrustumPlanes();

            foreach(var plane in planes)
            {
                foreach (var item in Items)
                {
                    if (item.IsOnCone)
                        foreach (var point in item.Pointers)
                        {
                            if (!plane.GetSide(point))
                            {
                                if (item.OnEnCone.GetPersistentEventCount() + item.OnQuCone.GetPersistentEventCount() == 0)
                                {
                                    item.gameObject.SetActive(false);
                                }
                                item.IsOnCone = false;
                                break;
                            }
                        }
                }
            }
        }
    }
}
