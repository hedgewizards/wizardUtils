using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WizardUtils.Vectors
{
    [System.Serializable]
    public struct AxisAngle
    {
        public Vector3 Axis;
        public float Angle;

        public AxisAngle(Vector3 axis, float angle)
        {
            Axis = axis;
            Angle = angle;
        }

        public AxisAngle(Quaternion quaternion)
        {
            quaternion.ToAngleAxis(out Angle, out Axis);
        }

        public Vector3 ToScaledVector() => Axis * Angle;
        public Quaternion ToQuaternion() => Quaternion.AngleAxis(Angle, Axis);

        #region AxisAngle Math
        public static AxisAngle Add(AxisAngle a, AxisAngle b)
        {
            return new AxisAngle(a.ToQuaternion() * b.ToQuaternion());
        }

        public static AxisAngle Zero => new AxisAngle(Vector3.forward, 0);
        #endregion

        #region Interpolation
        public static AxisAngle Lerp(AxisAngle a, AxisAngle b, float t)
        {
            Vector3 rawResult = a.ToScaledVector() * t + b.ToScaledVector() * (1 - t);
            float length = rawResult.magnitude;
            if (length == 0) return AxisAngle.Zero;
            return new AxisAngle(rawResult / length, length);
        }

        #endregion

        #region Rotate Operations
        public static Quaternion operator *(Quaternion left, AxisAngle right) => left * right.ToQuaternion();
        public static Quaternion operator *(AxisAngle left, Quaternion right) => right * left.ToQuaternion();
        public static Quaternion operator *(AxisAngle left, AxisAngle right) => left.ToQuaternion() * right.ToQuaternion();
        #endregion

        #region Scale Operations
        public static AxisAngle Scale(AxisAngle angle, float scale)
        {
            angle.Angle *= scale;
            return angle;
        }
        public static AxisAngle operator* (float left, AxisAngle right) => Scale(right, left);
        public static AxisAngle operator* (AxisAngle left, float right) => Scale(left, right);
        #endregion

        #region ToString
        public override string ToString()
        {
            return $"{Axis} @ {Angle}";
        }
        #endregion
    }
}
