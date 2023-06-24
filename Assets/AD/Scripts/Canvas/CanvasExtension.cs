using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace AD.UI
{

    static public class CanvasExtension
    {
        public enum CanvasAdaptive
        {
            horizontal = 0,
            vertical = 1,
            horizontalAndVertical = 2,
        }

        public static void Adaptive(this Canvas me, Camera camera, CanvasAdaptive mode = CanvasAdaptive.horizontalAndVertical)
        {
            var canvas = me.GetComponent<RectTransform>();
            Vector3[] cam_c, can_c;
            GetRect(camera, canvas, out cam_c, out can_c);

            if (mode == CanvasAdaptive.vertical)
            {
                float t = VerticalAdaptive(cam_c, can_c) * canvas.sizeDelta.y;
                canvas.sizeDelta = new Vector2(t, t);
                Debug.Log("ADinstance.UI.CanvasExtension.Adaptive(Vertical) : " + t);
            }
            else if (mode == CanvasAdaptive.horizontal)
            {
                float t = HorizontalAdaptive(cam_c, can_c) * canvas.sizeDelta.x; 
                canvas.sizeDelta = new Vector2(t, t);
                Debug.Log("ADinstance.UI.CanvasExtension.Adaptive(Horizontal) : " + t);
            }
            else if (mode == CanvasAdaptive.horizontalAndVertical)
            {
                float x_t = VerticalAdaptive(cam_c, can_c), y_t = HorizontalAdaptive(cam_c, can_c);
                canvas.sizeDelta = new Vector2(canvas.sizeDelta.x * x_t, canvas.sizeDelta.y * y_t);
                Debug.Log("ADinstance.UI.CanvasExtension.Adaptive : " + x_t + " " + y_t);
            } 
        }

        public static void GetRect(Camera camera, RectTransform canvas, out Vector3[] cameraRect, out Vector3[] canvasRect)
        {
            cameraRect = camera.GetCameraFovPositionByDistance(Vector3.Distance(camera.transform.position, canvas.transform.position));
            canvasRect = canvas.GetRect();
        }

        public static float HorizontalAdaptive(Vector3[] camera, Vector3[] canvas)
        {
            return Vector3.Distance(camera[1], camera[2]) / Vector3.Distance(canvas[1], canvas[2]);
        }

        public static float VerticalAdaptive(Vector3[] camera, Vector3[] canvas)
        {
            return Vector3.Distance(camera[0], camera[1]) / Vector3.Distance(canvas[0], canvas[1]);
        }
    }

    static public class CameraExtension
    {
        public static Vector3[] GetCameraFovPositionByDistance(this Camera cam, float distance)
        {
            Vector3[] corners = new Vector3[4];
            Vector3[] corners_ = new Vector3[4];

            cam.CalculateFrustumCorners(new Rect(0, 0, 1, 1), distance, Camera.MonoOrStereoscopicEye.Mono, corners_);

            corners[0] = cam.transform.TransformPoint(corners_[0]);
            corners[1] = cam.transform.TransformPoint(corners_[1]);
            corners[2] = cam.transform.TransformPoint(corners_[2]);
            corners[3] = cam.transform.TransformPoint(corners_[3]);
            return corners;
        }
    }

    static public class RectTransformExtension
    {
        public static Vector3[] GetRect(this RectTransform rect)
        {
            Vector3[] corners = new Vector3[4];
            rect.GetWorldCorners(corners);
            return corners;
        }
    }

}