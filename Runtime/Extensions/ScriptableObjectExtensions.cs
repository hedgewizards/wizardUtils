using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WizardUtils.Extensions
{
    public static class ScriptableObjectExtensions
    {
        public static T Clone<T>(this T scriptableObject) where T : ScriptableObject
        {
            T obj = UnityEngine.Object.Instantiate(scriptableObject);
            obj.name = scriptableObject.name;

            return obj;
        }
    }
}
