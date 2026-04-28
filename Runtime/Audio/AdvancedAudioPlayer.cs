using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using WizardUtils.Audio;

namespace WizardUtils.Runtime.Audio
{
    public class AdvancedAudioPlayer : MonoBehaviour
    {
        [SerializeField]
        private AdvancedSoundEffect SoundEffect;
        [SerializeField]
        private bool Loop;
        public bool PlayOnAwake;

        private AudioLoopInstance? LoopInstance;

        private void Awake()
        {
            if (PlayOnAwake)
            {
                Play();
            }
        }

        public void Play()
        {
            if (SoundEffect == null) return;
            if (Loop)
            {
                PlayLoop();
            }
            else
            {
                GameManager.Instance.SoundService.PlaySound(SoundEffect, transform);
            }
        }

        public void StopLoop()
        {
            if (!Loop || LoopInstance == null) return;

            GameManager.Instance.SoundService.StopAudioLoop(LoopInstance.Value);
        }

        private void PlayLoop()
        {
            LoopInstance = GameManager.Instance.SoundService.StartAudioLoop(this, transform, SoundEffect);
        }
    }
}
