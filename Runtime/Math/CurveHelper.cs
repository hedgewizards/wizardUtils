﻿using System;
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
        /// <param name="sharpness"></param>
        /// <returns></returns>
        public static float ApproachOutCurve(float t, float sharpness = 1f)
        {
            return t / (t + sharpness);
        }
    }
}
