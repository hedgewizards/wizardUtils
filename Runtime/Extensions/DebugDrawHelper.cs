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
        public static void DrawSphere(Vector3 center, float radius, Color color)
        {
            DrawCircle(center, Vector3.up, radius, color);
            DrawCircle(center, Vector3.left, radius, color);
            DrawCircle(center, Vector3.forward, radius, color);
        }

        public static void DrawCircle(Vector3 center, Vector3 axis, float radius, Color color, int resolution = 16)
        {
            Vector3 spoke = Vector3.Cross(center, axis).normalized * radius;
            Vector3 lastPoint = center + spoke;
            for (int n = 1; n <= resolution; n++)
            {
                Vector3 nextPoint = center + Quaternion.AngleAxis(n * 360f / resolution, axis) * spoke;

                Debug.DrawLine(lastPoint, nextPoint, color);
                lastPoint = nextPoint;
            }
        }
    }
}