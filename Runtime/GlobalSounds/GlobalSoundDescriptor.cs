using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WizardUtils.GlobalSounds
{
    [CreateAssetMenu( fileName = "gsound_", menuName = "WizardUtils/GlobalSounds/GlobalSoundDescriptor")]
    public class GlobalSoundDescriptor : ScriptableObject
    {
        public GameObject Prefab;
    }
}
