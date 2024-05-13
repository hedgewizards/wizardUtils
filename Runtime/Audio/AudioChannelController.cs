using UnityEngine.Audio;
using WizardUtils.Configuration.SettingWatchers;
using WizardUtils.SettingWatchers;

namespace WizardUtils.Audio
{
    public class AudioChannelController
    {
        AudioMixer mixer;
        string channelVolumeParamName;
        SettingWatcherFloat settingWatcher;

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

        public AudioChannelController(AudioMixer mixer, SettingWatcherFloat settingWatcher, string channelVolumeParamName)
        {
            this.mixer = mixer;
            this.settingWatcher = settingWatcher;
            this.channelVolumeParamName = channelVolumeParamName;

            settingWatcher.OnChanged += SettingWatcher_OnChanged;
            Volume = settingWatcher.Value;

            UpdateMixer();
        }

        private void SettingWatcher_OnChanged(object sender, SettingChangedEventArgs<float> e)
        {
            Volume = e.FinalValue;
        }

        private void UpdateMixer()
        {
            float rawVolume = Muted ? 0.001f : settingWatcher.Value;
            mixer.SetFloat(channelVolumeParamName, AudioUtils.LinearAudioLevelToLog(rawVolume));

        }
    }

}
