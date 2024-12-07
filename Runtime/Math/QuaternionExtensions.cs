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
            return CompressedFloatApproximately(a.w, b.w)
                && CompressedFloatApproximately(a.x, b.x)
                && CompressedFloatApproximately(a.y, b.y)
                && CompressedFloatApproximately(a.z, b.z);
        }

        private static bool CompressedFloatApproximately(float a, float b)
        {
            return Mathf.Abs(a - b) < 1 / (float)short.MaxValue;
        }

        public static bool IsUpright(this Quaternion quaternion)
        {
            return quaternion.x == 0 && quaternion.z == 0;
        }
    }
}
