using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WizardUtils.Extensions
{
    public static class Matrix4x4TRSExtensions
    {
        public static Vector3 ExtractPosition(this Matrix4x4 trsMatrix)
        {
            return trsMatrix.GetColumn(3);
        }

        public static Quaternion ExtractRotation(this Matrix4x4 trsMatrix)
        {
            return Quaternion.LookRotation(trsMatrix.GetColumn(2), trsMatrix.GetColumn(1));
        }
    }
}
