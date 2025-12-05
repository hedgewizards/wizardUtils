using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace WizardUtils.Extensions
{
    public static class DebugDrawHelper
    {
        private const int CircleDefaultResolution = 16;
        private const int DefaultDrawDuration = 0;

        public static void DrawSquare(Vector3 center, float radius, Quaternion orientation, Color color, float duration = DefaultDrawDuration)
        {
            Vector3[,] corners = new Vector3[2, 2];
            for (int x = 0; x < 2; x++)
            {
                for (int y = 0; y < 2; y++)
                {
                    corners[x, y] = new Vector3()
                    {
                        x = radius * (x == 0 ? 1 : -1),
                        y = 0,
                        z = radius * (y == 0 ? 1 : -1),
                    };
                    corners[x, y] = center + orientation * corners[x, y];
                }
            }

            Debug.DrawLine(corners[0, 0], corners[0, 1], color, duration);
            Debug.DrawLine(corners[0, 0], corners[1, 0], color, duration);
            Debug.DrawLine(corners[1, 1], corners[0, 1], color, duration);
            Debug.DrawLine(corners[1, 1], corners[1, 0], color, duration);
        }

        public static void DrawBox(Vector3 center, Vector3 extents, Quaternion orientation, Color color, float duration = DefaultDrawDuration)
        {
            Vector3[,,] corners = new Vector3[2, 2, 2];
            for (int x = 0; x < 2; x++)
            {
                for (int y = 0; y < 2; y++)
                {
                    for (int z = 0; z < 2; z++)
                    {
                        corners[x, y, z] = new Vector3()
                        {
                            x = extents.x * (x == 0 ? 1 : -1),
                            y = extents.y * (y == 0 ? 1 : -1),
                            z = extents.z * (z == 0 ? 1 : -1),
                        };
                        corners[x, y, z] = center + orientation * corners[x, y, z];
                    }
                }
            }

            // bottom rectangle
            Debug.DrawLine(corners[0, 0, 0], corners[0, 0, 1], color, duration);
            Debug.DrawLine(corners[0, 0, 0], corners[0, 1, 0], color, duration);
            Debug.DrawLine(corners[0, 1, 1], corners[0, 0, 1], color, duration);
            Debug.DrawLine(corners[0, 1, 1], corners[0, 1, 0], color, duration);

            // top rectangle
            Debug.DrawLine(corners[1, 0, 0], corners[1, 0, 1], color, duration);
            Debug.DrawLine(corners[1, 0, 0], corners[1, 1, 0], color, duration);
            Debug.DrawLine(corners[1, 1, 1], corners[1, 0, 1], color, duration);
            Debug.DrawLine(corners[1, 1, 1], corners[1, 1, 0], color, duration);

            // pillars
            Debug.DrawLine(corners[0, 0, 0], corners[1, 0, 0], color, duration);
            Debug.DrawLine(corners[0, 0, 1], corners[1, 0, 1], color, duration);
            Debug.DrawLine(corners[0, 1, 0], corners[1, 1, 0], color, duration);
            Debug.DrawLine(corners[0, 1, 1], corners[1, 1, 1], color, duration);
        }
        public static void DrawCapsule(
            Vector3 p1,
            Vector3 p2,
            float radius,
            Color color,
            int resolution = CircleDefaultResolution,
            float duration = DefaultDrawDuration)
        {
#if UNITY_EDITOR
            Vector3 up = (p1 - p2).normalized;
            float height = Vector3.Distance(p1, p2);

            // Orthonormal basis
            Vector3 forward = Vector3.Cross(up, Vector3.right);
            if (forward.sqrMagnitude < 0.001f)
                forward = Vector3.Cross(up, Vector3.forward);
            forward.Normalize();
            Vector3 right = Vector3.Cross(forward, up);

            // Top and bottom centers
            Vector3 top = p1;
            Vector3 bottom = p2;

            DrawHalfCircle(top, Quaternion.LookRotation(right, -up), radius, color, resolution, duration);
            DrawHalfCircle(top, Quaternion.LookRotation(forward, -up), radius, color, resolution, duration);
            DrawCircle(top, Quaternion.LookRotation(up, right), radius, color, resolution, duration);

            DrawHalfCircle(bottom, Quaternion.LookRotation(right, up), radius, color, resolution, duration);
            DrawHalfCircle(bottom, Quaternion.LookRotation(forward, up), radius, color, resolution, duration);
            DrawCircle(bottom, Quaternion.LookRotation(up, right), radius, color, resolution, duration);

            Debug.DrawRay(top + radius * forward, -up * height, color, duration);
            Debug.DrawRay(top + radius * right, -up * height, color, duration);
            Debug.DrawRay(top + radius * -right, -up * height, color, duration);
            Debug.DrawRay(top + radius * -forward, -up * height, color, duration);
#endif
        }


        public static void DrawCapsule(Vector3 center, float height, float radius, Color color, int resolution = CircleDefaultResolution, float duration = DefaultDrawDuration)
        {
#if UNITY_EDITOR
            Vector3 upHeight = Vector3.up * ( height - radius * 2 );

            DrawHalfCircle(center + upHeight / 2, Quaternion.LookRotation(Vector3.left, Vector3.down), radius, color, resolution, duration);
            DrawHalfCircle(center + upHeight / 2, Quaternion.LookRotation(Vector3.forward, Vector3.down), radius, color, resolution, duration);
            DrawCircle(center + upHeight / 2, Quaternion.LookRotation(Vector3.up, Vector3.left), radius, color, resolution, duration);

            DrawHalfCircle(center - upHeight / 2, Quaternion.LookRotation(Vector3.left, Vector3.up), radius, color, resolution, duration);
            DrawHalfCircle(center - upHeight / 2, Quaternion.LookRotation(Vector3.forward, Vector3.up), radius, color, resolution, duration);
            DrawCircle(center - upHeight / 2, Quaternion.LookRotation(Vector3.up, Vector3.left), radius, color, resolution, duration);

            Debug.DrawRay(center + radius * Vector3.forward + upHeight / 2, -1 * upHeight, color, duration);
            Debug.DrawRay(center + radius * Vector3.left + upHeight / 2, -1 * upHeight, color, duration);
            Debug.DrawRay(center + radius * Vector3.right + upHeight / 2, -1 * upHeight, color, duration);
            Debug.DrawRay(center + radius * Vector3.back + upHeight / 2, -1 * upHeight, color, duration);
#endif
        }

        public static void DrawSphere(Vector3 center, float radius, Color color, int resolution = CircleDefaultResolution, float duration = DefaultDrawDuration)
        {
#if UNITY_EDITOR
            DrawCircle(center, Quaternion.LookRotation(Vector3.up, Vector3.left), radius, color, resolution, duration);
            DrawCircle(center, Quaternion.LookRotation(Vector3.left, Vector3.up), radius, color, resolution, duration);
            DrawCircle(center, Quaternion.LookRotation(Vector3.forward, Vector3.up), radius, color, resolution, duration);

            var view = SceneView.currentDrawingSceneView;
            var currentCam = Camera.current;
            if (view != null)
            {
                Vector3 camForward = view.camera.transform.forward;
                DrawCircle(center, Quaternion.LookRotation(camForward, Vector3.up), radius, color);
            }
            else if (currentCam != null)
            {
                Vector3 camForward = currentCam.transform.forward;
                DrawCircle(center, Quaternion.LookRotation(camForward, Vector3.up), radius, color);
            }
#endif
        }

        public static void DrawCircle(Vector3 center, Quaternion axis, float radius, Color color, int resolution = CircleDefaultResolution, float duration = DefaultDrawDuration)
        {
#if UNITY_EDITOR
            Vector3 spoke = axis * Vector3.left * radius;
            Vector3 pole = axis * Vector3.forward;
            Vector3 lastPoint = center + spoke;
            for (int n = 1; n <= resolution; n++)
            {
                Vector3 nextPoint = center + Quaternion.AngleAxis(n * 360f / resolution, pole) * spoke;

                Debug.DrawLine(lastPoint, nextPoint, color, duration);
                lastPoint = nextPoint;
            }
#endif
        }

        public static void DrawHalfCircle(Vector3 center, Quaternion axis, float radius, Color color, int resolution = CircleDefaultResolution, float duration = DefaultDrawDuration)
        {
#if UNITY_EDITOR
            Vector3 spoke = axis * Vector3.left * radius;
            Vector3 pole = axis * Vector3.forward;
            Vector3 lastPoint = center + spoke;

            for (int n = 1; n <= (resolution / 2); n++)
            {
                Vector3 nextPoint = center + Quaternion.AngleAxis(n * 360f / resolution, pole) * spoke;

                Debug.DrawLine(lastPoint, nextPoint, color, duration);
                lastPoint = nextPoint;
            }
#endif
        }
    }
}