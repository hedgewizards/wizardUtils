using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardUtils.Math
{
    public interface ILerpable<T> where T : ILerpable<T>
    {
        T InterpolateTo(T b, float t);
    }

    public static class Lerpable
    {
        public static T Lerp<T>(T a, T b, float t) where T : ILerpable<T>
        {
            return a.InterpolateTo(b, t);
        }
    }
}
