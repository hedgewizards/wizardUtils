using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using WizardUtils.Audio.GlobalSounds;
using WizardUtils.Saving;


namespace WizardUtils
{
    [CreateAssetMenu( fileName = "ManifestSet", menuName = "WizardUtils/ManifestSet", order = 100)]
    public class ManifestSet : ScriptableObject
    {
        public GlobalSoundManifest GlobalSound;
        public WizardUtils.Configurations.ConfigSettings.SettingManifest IndexedSettings;
    }
}
