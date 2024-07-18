using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace WizardUtils.Extensions
{
    public static class DebugDrawHelper
    {
        private const int CircleDefaultResolution = 16;
        private const int DefaultDrawDuration = 0;

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

        public static void DrawCapsule(Vector3 center, float height, float radius, Color color, int resolution = CircleDefaultResolution)
        {
#if UNITY_EDITOR
            Vector3 upHeight = Vector3.up * ( height - radius * 2 );

            DrawHalfCircle(center + upHeight / 2, Quaternion.LookRotation(Vector3.left, Vector3.down), radius, color, resolution);
            DrawHalfCircle(center + upHeight / 2, Quaternion.LookRotation(Vector3.forward, Vector3.down), radius, color, resolution);
            DrawCircle(center + upHeight / 2, Quaternion.LookRotation(Vector3.up, Vector3.left), radius, color, resolution);

            DrawHalfCircle(center - upHeight / 2, Quaternion.LookRotation(Vector3.left, Vector3.up), radius, color, resolution);
            DrawHalfCircle(center - upHeight / 2, Quaternion.LookRotation(Vector3.forward, Vector3.up), radius, color, resolution);
            DrawCircle(center - upHeight / 2, Quaternion.LookRotation(Vector3.up, Vector3.left), radius, color, resolution);

            Debug.DrawRay(center + radius * Vector3.forward + upHeight / 2, -1 * upHeight, color);
            Debug.DrawRay(center + radius * Vector3.left + upHeight / 2, -1 * upHeight, color);
            Debug.DrawRay(center + radius * Vector3.right + upHeight / 2, -1 * upHeight, color);
            Debug.DrawRay(center + radius * Vector3.back + upHeight / 2, -1 * upHeight, color);
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