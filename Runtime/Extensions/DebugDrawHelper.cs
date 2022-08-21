using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WizardUtils.Extensions
{
    public static class DebugDrawHelper
    {
        public static void DrawCapsule(Vector3 center, float height, float radius, Color color, int resolution = 16)
        {
            Vector3 upHeight = Vector3.up * ( height - radius * 2 );

            DrawHalfCircle(center + upHeight / 2, Quaternion.LookRotation(Vector3.left, Vector3.down), radius,      color, resolution);
            DrawHalfCircle(center + upHeight / 2, Quaternion.LookRotation(Vector3.forward, Vector3.down), radius,   color, resolution);
            DrawCircle(center + upHeight / 2, Quaternion.LookRotation(Vector3.up, Vector3.left), radius,            color, resolution);

            DrawHalfCircle(center - upHeight / 2, Quaternion.LookRotation(Vector3.left, Vector3.up), radius,        color, resolution);
            DrawHalfCircle(center - upHeight / 2, Quaternion.LookRotation(Vector3.forward, Vector3.up), radius,     color, resolution);
            DrawCircle(center - upHeight / 2, Quaternion.LookRotation(Vector3.up, Vector3.left), radius,            color, resolution);

            Debug.DrawRay(center + radius * Vector3.forward + upHeight / 2, -1 * upHeight, color);
            Debug.DrawRay(center + radius * Vector3.left + upHeight / 2, -1 * upHeight, color);
            Debug.DrawRay(center + radius * Vector3.right + upHeight / 2, -1 * upHeight, color);
            Debug.DrawRay(center + radius * Vector3.back + upHeight / 2, -1 * upHeight, color);
        }

        public static void DrawSphere(Vector3 center, float radius, Color color)
        {
            DrawCircle(center, Quaternion.LookRotation(Vector3.up,      Vector3.left),  radius, color);
            DrawCircle(center, Quaternion.LookRotation(Vector3.left,    Vector3.up),    radius, color);
            DrawCircle(center, Quaternion.LookRotation(Vector3.forward, Vector3.up),    radius, color);
        }

        public static void DrawCircle(Vector3 center, Quaternion axis, float radius, Color color, int resolution = 16)
        {
            Vector3 spoke = axis * Vector3.left * radius;
            Vector3 pole = axis * Vector3.forward;
            Vector3 lastPoint = center + spoke;
            for (int n = 1; n <= resolution; n++)
            {
                Vector3 nextPoint = center + Quaternion.AngleAxis(n * 360f / resolution, pole) * spoke;

                Debug.DrawLine(lastPoint, nextPoint, color);
                lastPoint = nextPoint;
            }
        }

        public static void DrawHalfCircle(Vector3 center, Quaternion axis, float radius, Color color, int resolution = 16)
        {
            Vector3 spoke = axis * Vector3.left * radius;
            Vector3 pole = axis * Vector3.forward;
            Vector3 lastPoint = center + spoke;

            for (int n = 1; n <= (resolution / 2); n++)
            {
                Vector3 nextPoint = center + Quaternion.AngleAxis(n * 360f / resolution, pole) * spoke;

                Debug.DrawLine(lastPoint, nextPoint, color);
                lastPoint = nextPoint;
            }
        }
    }
}