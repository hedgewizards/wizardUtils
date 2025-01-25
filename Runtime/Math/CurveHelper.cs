using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardUtils.Math
{
    public static class CurveHelper
    {
        /// <summary>
        /// start at 0, and end at 1 as t approaches infinity
        /// </summary>
        /// <param name="t"></param>
        /// <param name="dullness"></param>
        /// <returns></returns>
        public static float ApproachOutCurve(float t, float dullness = 1f)
        {
            return t / (t + dullness);
        }

        public static float EaseInAndOut(float t)
        {
            return t * t * (3 - 2 * t);
        }

        public static float EaseIn(float t)
        {
            return t * t;
        }

        public static float EaseOut(float t)
        {
            return 1 - (1 - t) * (1 - t);
        }
    }
}
