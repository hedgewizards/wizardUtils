using System.Collections.Generic;
using UnityEngine;
using WizardUtils.Audio.GlobalSounds;

namespace WizardUtils.Audio
{
    public class SoundService
    {
        private readonly GameObject Parent;
        private readonly GlobalSoundManifest Manifest;

        Dictionary<GlobalSoundDescriptor, AudioSource> soundsDict = new Dictionary<GlobalSoundDescriptor, AudioSource>();

        public SoundService(GameObject parent, GlobalSoundManifest manifest)
        {
            Parent = parent;
            Manifest = manifest;
        }

        public void PlayGlobalSound(GlobalSoundDescriptor sound)
        {
            AudioSource obj;
            if (!soundsDict.TryGetValue(sound, out obj))
            {
                var newSoundObj = UnityEngine.Object.Instantiate(sound.Prefab, Parent.transform);
                obj = newSoundObj.GetComponent<AudioSource>();
                soundsDict.Add(sound, obj);
            }
            obj.Play();
        }
    }
}
