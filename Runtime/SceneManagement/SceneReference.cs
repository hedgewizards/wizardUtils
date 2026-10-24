using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WizardUtils.SceneManagement
{
    [System.Serializable]
    public struct SceneReference
    {
#if UNITY_EDITOR
        [SerializeField]
        public UnityEditor.SceneAsset SceneAsset;
#endif
        public int SceneId;

        public bool HasReference()
        {
#if UNITY_EDITOR
            return SceneAsset != null;
#else
            return SceneId != -1;
#endif
        }
    }
}
