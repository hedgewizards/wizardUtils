using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using WizardUtils.GlobalSounds;
using WizardUtils.Saving;

namespace WizardUtils
{
    [CreateAssetMenu( fileName = "GameManagerConfiguration", menuName = "WizardUtils/GameManagerConfiguration", order = 0)]
    public class GameManagerConfiguration : ScriptableObject
    {
        public GlobalSoundManifest GlobalSoundManifest;
        public SaveManifest MainSaveManifest;
    }
}
