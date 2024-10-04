using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

namespace WizardUtils.Tools
{
    public static class AssetEnumeration
    {
        public static IEnumerable<T> GetEnumerableAssetsOfType<T>()
            where T : UnityEngine.Object
        {
            return AssetDatabase.FindAssets($"t:{typeof(T)}")
                .Select(id => AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(id)));
        }
    }
}
