﻿using UnityEngine.Audio;
using WizardUtils.GameSettings;

namespace WizardUtils.Audio
{
    public class AudioChannelController
    {
        AudioMixer mixer;
        string channelVolumeParamName;
        GameSettingFloat gameSetting;

        private bool muted;
        private float volume;

        public bool Muted
        {
            get => muted;
            set
            {
                muted = value;
                UpdateMixer();
            }
        }

        public float Volume
        {
            get => volume;
            set
            {
                volume = value;
                UpdateMixer();
            }
        }

        public AudioChannelController(AudioMixer mixer, GameSettingFloat gameSetting, string channelVolumeParamName)
        {
            this.mixer = mixer;
            this.gameSetting = gameSetting;
            this.channelVolumeParamName = channelVolumeParamName;

            gameSetting.OnChanged += GameSetting_OnChanged;
            Volume = gameSetting.Value;

            UpdateMixer();
        }

        private void GameSetting_OnChanged(object sender, GameSettingChangedEventArgs<float> e)
        {
            Volume = e.FinalValue;
        }

        private void UpdateMixer()
        {
            float rawVolume = Muted ? 0.001f : gameSetting.Value;
            mixer.SetFloat(channelVolumeParamName, AudioUtils.LinearAudioLevelToLog(rawVolume));

        }
    }

}
