using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WizardUtils.Math
{
    public static class InterpolationHelper
    {
        public static float ConvertRangeToTime(float value, float min, float max)
        {
            if (max == min) return max;
            return Mathf.Clamp01((value - min) / (max - min));
        }

        /// <summary>
        /// Interpolate from a to b by parametric t with a smooth start and stop
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static float SmoothInterpolate(float a, float b, float t)
        {
            return a + (b - a) * t * t * (3 - 2 * t);
        }
        public static Color SmoothInterpolate(Color a, Color b, float t)
        {
            for (int n = 0; n < 4; n++)
            {
                a[n] = SmoothInterpolate(a[n], b[n], t);
            }

            return a;
        }
        public static Vector2 SmoothInterpolate(Vector2 a, Vector2 b, float t)
        {
            for (int n = 0; n < 2; n++)
            {
                a[n] = SmoothInterpolate(a[n], b[n], t);
            }

            return a;
        }

        /// <summary>
        /// Interpolate from a to b by parametric t with a smooth start
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static float AccelerateInterpolate(float a, float b, float t)
        {
            return a + (b - a) * t * t;
        }
        public static Color AccelerateInterpolate(Color a, Color b, float t)
        {
            for (int n = 0; n < 4; n++)
            {
                a[n] = AccelerateInterpolate(a[n], b[n], t);
            }

            return a;
        }
        public static Vector2 AccelerateInterpolate(Vector2 a, Vector2 b, float t)
        {
            for (int n = 0; n < 2; n++)
            {
                a[n] = AccelerateInterpolate(a[n], b[n], t);
            }

            return a;
        }

    }
}
