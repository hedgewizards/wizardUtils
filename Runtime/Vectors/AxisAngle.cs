using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WizardUtils.Vectors
{
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

        public Quaternion ToQuaternion() => Quaternion.AngleAxis(Angle, Axis);

        #region AxisAngle Math
        public static AxisAngle Add(AxisAngle a, AxisAngle b)
        {
            return new AxisAngle(a.ToQuaternion() * b.ToQuaternion());
        }

        public static AxisAngle Zero => new AxisAngle(Vector3.forward, 0);
        #endregion

        #region Interpolation
        public static AxisAngle Slerp(AxisAngle a, AxisAngle b, float t)
        {
            Quaternion rawResult = Quaternion.Slerp(a.ToQuaternion(), b.ToQuaternion(), t);
            return new AxisAngle(rawResult);
        }
        #endregion

        #region Rotate Operations
        public static Quaternion Rotate(Quaternion q, AxisAngle axisAngle)
        {
            return q * axisAngle.ToQuaternion();
        }

        public static Quaternion operator*(Quaternion left, AxisAngle right)
        {
            return Rotate(left, right);
        }
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
    }
}
