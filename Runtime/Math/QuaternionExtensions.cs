using UnityEngine;

namespace WizardUtils.Math
{
    public static class QuaternionExtensions
    {
        public static float CalculateMagnitude(this Quaternion quaternion)
        {
            return Mathf.Sqrt(
                quaternion.w * quaternion.w
                + quaternion.x * quaternion.x
                + quaternion.y * quaternion.y
                + quaternion.z * quaternion.z
                );
        }

        public static bool Approximately(this Quaternion a, Quaternion b)
        {
            return Mathf.Approximately(a.w, b.w)
                && Mathf.Approximately(a.x, b.x)
                && Mathf.Approximately(a.y, b.y)
                && Mathf.Approximately(a.z, b.z);
        }
    }
}
