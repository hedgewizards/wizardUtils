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
        /// <summary>
        /// right handed axis of rotation
        /// </summary>
        public Vector3 Axis;
        /// <summary>
        /// angle in degrees
        /// </summary>
        public float Angle;

        /// <summary>
        /// Create an AxisAngle
        /// </summary>
        /// <param name="axis">right handed axis of rotation</param>
        /// <param name="angle">angle in degrees</param>
        public AxisAngle(Vector3 axis, float angle)
        {
            Axis = axis;
            Angle = angle;
        }

        public AxisAngle(Quaternion quaternion)
        {
            quaternion.ToAngleAxis(out Angle, out Axis);
        }

        public AxisAngle(Vector3 rawVector)
        {
            Angle = rawVector.magnitude;
            if (Angle == 0)
            {
                Axis = Vector3.forward;
                return;
            }
            Axis = rawVector / Angle;
        }

        public Vector3 ToScaledVector() => Axis * Angle;
        public Quaternion ToQuaternion() => Quaternion.AngleAxis(Angle, Axis);

        #region AxisAngle Math
        public static AxisAngle Add(AxisAngle a, AxisAngle b)
        {
            Vector3 rawResult = a.ToScaledVector() + b.ToScaledVector();
            return new AxisAngle(rawResult);
        }

        public static AxisAngle Zero => new AxisAngle(Vector3.forward, 0);
        #endregion

        #region Interpolation
        public static AxisAngle Lerp(AxisAngle a, AxisAngle b, float t)
        {
            Vector3 rawResult = Vector3.Lerp(a.ToScaledVector(), b.ToScaledVector(), t);
            return new AxisAngle(rawResult);
        }
        public static AxisAngle Slerp(AxisAngle a, AxisAngle b, float t)
        {
            Vector3 rawResult = Vector3.Slerp(a.ToScaledVector(), b.ToScaledVector(), t);
            return new AxisAngle(rawResult);
        }

        #endregion

        #region Rotate Operations
        public static Quaternion operator *(Quaternion left, AxisAngle right) => left * right.ToQuaternion();
        public static Quaternion operator *(AxisAngle left, Quaternion right) => left.ToQuaternion() * right;
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
