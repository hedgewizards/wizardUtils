using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WizardUtils.GlobalSounds
{
    public class GlobalSoundPlayer : MonoBehaviour
    {
        public GlobalSoundDescriptor Sound;
        public void Play()
        {
            GameManager.Instance.GlobalSoundService.Play(Sound);
        }

        public void PlayOneShot(GlobalSoundDescriptor sound)
        {
            GameManager.Instance.GlobalSoundService.Play(sound);
        }
    }
}
