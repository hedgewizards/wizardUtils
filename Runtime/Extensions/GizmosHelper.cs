using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace WizardUtils.Extensions
{
    public static class GizmosHelper
    {
        private const int CircleDefaultResolution = 16;
        private const int ArcDefaultResolution = 8;

        public static void DrawSphereConnection(Vector3 center1, Vector3 center2, float radius1, float radius2)
        {
            Quaternion along = Quaternion.LookRotation(center2 - center1, Vector3.up);

            for(int n = 0; n < 4; n++)
            {
                Quaternion around = Quaternion.Euler(0, 90 * n, 0);
                Vector3 start = center1 + around * along * Vector3.right * radius1;
                Vector3 end = center2 + around * along * Vector3.right * radius2;

                Gizmos.DrawLine(start, end);
            }
        }

        public static void DrawCubeFlush(Vector3 point, Vector3 up, Vector3 size)
        {
            Vector3 center = point + up * size.y / 2;
            Quaternion rotation = Quaternion.LookRotation(Vector3.Cross(Vector3.right, up), up);
            Gizmos.matrix = Matrix4x4.TRS(center, rotation, Vector3.one);
            Gizmos.DrawCube(Vector3.zero, size);
            Gizmos.matrix = Matrix4x4.identity;
        }

        public static void DrawWireMeshFlush(Mesh mesh, Vector3 point, Vector3 up, Quaternion rotation)
        {
            DrawWireMeshFlush(mesh, point, up, rotation, Vector3.one);
        }
        public static void DrawWireMeshFlush(Mesh mesh, Vector3 point, Vector3 up, Quaternion rotation, Vector3 scale)
        {
            Quaternion finalRotation = Quaternion.LookRotation(Vector3.Cross(Vector3.right, up), up) * rotation;
            Gizmos.DrawWireMesh(mesh, point, finalRotation, scale);
        }

        public static void DrawMeshFlush(Mesh mesh, Vector3 point, Vector3 up, Quaternion rotation)
        {
            DrawMeshFlush(mesh, point, up, rotation, Vector3.one);
        }
        public static void DrawMeshFlush(Mesh mesh, Vector3 point, Vector3 up, Quaternion rotation, Vector3 scale)
        {
            Quaternion finalRotation = Quaternion.LookRotation(Vector3.Cross(Vector3.right, up), up) * rotation;
            Gizmos.DrawWireMesh(mesh, point, finalRotation, scale);
        }

        public static void DrawCapsule(Vector3 center, float height, float radius, int resolution = CircleDefaultResolution)
        {
#if UNITY_EDITOR
            Vector3 upHeight = Vector3.up * (height - radius * 2);

            DrawHalfCircle(center + upHeight / 2, Quaternion.LookRotation(Vector3.left, Vector3.down), radius, resolution);
            DrawHalfCircle(center + upHeight / 2, Quaternion.LookRotation(Vector3.forward, Vector3.down), radius, resolution);
            DrawCircle(center + upHeight / 2, Quaternion.LookRotation(Vector3.up, Vector3.left), radius, resolution);

            DrawHalfCircle(center - upHeight / 2, Quaternion.LookRotation(Vector3.left, Vector3.up), radius, resolution);
            DrawHalfCircle(center - upHeight / 2, Quaternion.LookRotation(Vector3.forward, Vector3.up), radius, resolution);
            DrawCircle(center - upHeight / 2, Quaternion.LookRotation(Vector3.up, Vector3.left), radius, resolution);

            Gizmos.DrawRay(new Ray(center + radius * Vector3.forward + upHeight / 2, -1 * upHeight));
            Gizmos.DrawRay(new Ray(center + radius * Vector3.left + upHeight / 2, -1 * upHeight));
            Gizmos.DrawRay(new Ray(center + radius * Vector3.right + upHeight / 2, -1 * upHeight));
            Gizmos.DrawRay(new Ray(center + radius * Vector3.back + upHeight / 2, -1 * upHeight));
#endif
        }

        public static void DrawSphere(Vector3 center, float radius, int resolution = CircleDefaultResolution)
        {
#if UNITY_EDITOR
            DrawCircle(center, Quaternion.LookRotation(Vector3.up, Vector3.left), radius, resolution);
            DrawCircle(center, Quaternion.LookRotation(Vector3.left, Vector3.up), radius, resolution);
            DrawCircle(center, Quaternion.LookRotation(Vector3.forward, Vector3.up), radius, resolution);

            var view = SceneView.currentDrawingSceneView;
            var currentCam = Camera.current;
            if (view != null)
            {
                Vector3 camForward = view.camera.transform.forward;
                DrawCircle(center, Quaternion.LookRotation(camForward, Vector3.up), radius);
            }
            else if (currentCam != null)
            {
                Vector3 camForward = currentCam.transform.forward;
                DrawCircle(center, Quaternion.LookRotation(camForward, Vector3.up), radius);
            }
#endif
        }

        public static void DrawCircle(Vector3 center, Quaternion axis, float radius, int resolution = CircleDefaultResolution)
        {
#if UNITY_EDITOR
            Vector3 spoke = axis * Vector3.left * radius;
            Vector3 pole = axis * Vector3.forward;
            Vector3 lastPoint = center + spoke;
            for (int n = 1; n <= resolution; n++)
            {
                Vector3 nextPoint = center + Quaternion.AngleAxis(n * 360f / resolution, pole) * spoke;

                Gizmos.DrawLine(lastPoint, nextPoint);
                lastPoint = nextPoint;
            }
#endif
        }

        public static void DrawWedge(
    Vector3 center,
    Vector3 forward,
    float radius,
    float yawSize,
    float pitchSize,
    int yawResolution = ArcDefaultResolution,
    int pitchResolution = ArcDefaultResolution)
        {
#if UNITY_EDITOR
            Quaternion basis = Quaternion.LookRotation(forward.normalized, Vector3.up);

            Vector3[] tips = new Vector3[4];
            int i = 0;

            //spokes
            for (int y = -1; y <= 1; y += 2)
            {
                for (int p = -1; p <= 1; p += 2)
                {
                    Quaternion rot =
                        basis *
                        Quaternion.AngleAxis(y * yawSize, Vector3.up) *
                        Quaternion.AngleAxis(p * pitchSize, Vector3.right);

                    Vector3 dir = rot * Vector3.forward;
                    tips[i] = center + dir * radius;

                    Gizmos.DrawLine(center, tips[i]);
                    i++;
                }
            }

            // horizontal arcs
            for (int p = -1; p <= 1; p += 2)
            {
                Vector3 last = Vector3.zero;

                for (int n = 0; n <= yawResolution; n++)
                {
                    float t = (float)n / yawResolution;
                    float yaw = Mathf.Lerp(-yawSize, yawSize, t);

                    Quaternion rot =
                        basis *
                        Quaternion.AngleAxis(yaw, Vector3.up) *
                        Quaternion.AngleAxis(p * pitchSize, Vector3.right);

                    Vector3 point = center + (rot * Vector3.forward) * radius;

                    if (n != 0)
                        Gizmos.DrawLine(last, point);

                    last = point;
                }
            }

            // vertical arcs
            for (int y = -1; y <= 1; y += 2)
            {
                Vector3 last = Vector3.zero;

                for (int n = 0; n <= pitchResolution; n++)
                {
                    float t = (float)n / pitchResolution;
                    float pitch = Mathf.Lerp(-pitchSize, pitchSize, t);

                    Quaternion rot =
                        basis *
                        Quaternion.AngleAxis(y * yawSize, Vector3.up) *
                        Quaternion.AngleAxis(pitch, Vector3.right);

                    Vector3 point = center + (rot * Vector3.forward) * radius;

                    if (n != 0)
                        Gizmos.DrawLine(last, point);

                    last = point;
                }
            }
#endif
        }

        public static void DrawHalfCircle(Vector3 center, Quaternion axis, float radius, int resolution = CircleDefaultResolution)
        {
#if UNITY_EDITOR
            Vector3 spoke = axis * Vector3.left * radius;
            Vector3 pole = axis * Vector3.forward;
            Vector3 lastPoint = center + spoke;

            for (int n = 1; n <= (resolution / 2); n++)
            {
                Vector3 nextPoint = center + Quaternion.AngleAxis(n * 360f / resolution, pole) * spoke;

                Gizmos.DrawLine(lastPoint, nextPoint);
                lastPoint = nextPoint;
            }
#endif
        }
    }
}
