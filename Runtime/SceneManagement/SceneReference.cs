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
#if DEBUG
        [SerializeField]
        public UnityEditor.SceneAsset SceneAsset;
#endif
        public int SceneId;

        public bool HasReference()
        {
#if DEBUG
            return SceneAsset != null;
#else
            return SceneId != -1;
#endif
        }
    }
}
