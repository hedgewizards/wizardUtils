using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WizardUtils.Extensions
{
    public static class TransformExtensions
    {
        /// <summary>
        /// Allocated and returns an array of all the current children of this transform
        /// </summary>
        /// <param name="transform"></param>
        /// <returns></returns>
        public static Transform[] GetChildren(this Transform transform)
        {
            Transform[] result = new Transform[transform.childCount];
            for (int n = 0; n < transform.childCount; n++)
            {
                result[n] = transform.GetChild(n);
            }
            return result;
        }
    }
}
