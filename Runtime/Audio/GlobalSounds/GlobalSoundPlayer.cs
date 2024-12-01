using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WizardUtils.Audio.GlobalSounds
{
    public class GlobalSoundPlayer : MonoBehaviour
    {
        public GlobalSoundDescriptor Sound;
        public void Play()
        {
            GameManager.Instance.SoundService.PlayGlobalSound(Sound);
        }

        public void PlayOneShot(GlobalSoundDescriptor sound)
        {
            GameManager.Instance.SoundService.PlayGlobalSound(sound);
        }
    }
}
