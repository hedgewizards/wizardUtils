using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using WizardUtils.Audio;
using WizardUtils.Configurations;
using WizardUtils.GameSettings;

namespace WizardUtils
{
    [RequireComponent(typeof(GameManager))]
    public class AudioManager : MonoBehaviour
    {
        GameManager gameManager;
        private bool shouldMuteOnLoseFocus;
        private bool applicationHasFocus;
        private bool audioManagerSetUp = false;

        private const float DEFAULT_VOLUME_MASTER = 100;
        private const float DEFAULT_VOLUME_CATEGORY = 80;

        public AudioMixer mixer;
        public AudioChannelSettingData[] AdditionalChannels;
        private static AudioChannelSettingData[] BaseChannels => new AudioChannelSettingData[]
        {
            new AudioChannelSettingData("MasterVolume", "volume_master", DEFAULT_VOLUME_MASTER),
            new AudioChannelSettingData("EffectVolume", "volume_effects", DEFAULT_VOLUME_CATEGORY),
            new AudioChannelSettingData("AmbienceVolume", "volume_ambience", DEFAULT_VOLUME_CATEGORY),
            new AudioChannelSettingData("MusicVolume", "volume_music", DEFAULT_VOLUME_CATEGORY)
        };

        private AudioChannelController[] Controllers;
        private GameSettingBool TabMuteSetting;

        private void Start()
        {
            gameManager = GetComponent<GameManager>();
            Controllers = new AudioChannelController[BaseChannels.Length];

            var allChannels = GenerateAllChannels();
            Controllers = new AudioChannelController[allChannels.Length];
            for (int n = 0; n < allChannels.Length; n++)
            {
                var setting = new GameSettingFloat(gameManager.Configuration, allChannels[n].GameSettingKey, allChannels[n].DefaultValue);
                Controllers[n] = new AudioChannelController(mixer, setting, allChannels[n].MixerParamName);
            }

            TabMuteSetting = new GameSettingBool(gameManager.Configuration, "tabmute", false);
            TabMuteSetting.OnChanged += AltTabMuteSetting_OnChanged;
            shouldMuteOnLoseFocus = TabMuteSetting.Value;
            audioManagerSetUp = true;
        }

        private void AltTabMuteSetting_OnChanged(object sender, GameSettingChangedEventArgs<bool> e)
        {
            shouldMuteOnLoseFocus = e.FinalValue;
            RecalculateShouldMute();
        }

        private void OnApplicationFocus(bool focus)
        {
            if (!audioManagerSetUp) return;
            applicationHasFocus = focus;
            RecalculateShouldMute();
        }

        private void AltTabMuteSetting_OnChanged(object sender, ValueChangedEventArgs e)
        {
        }

        private void RecalculateShouldMute()
        {
            SetGameMuted(shouldMuteOnLoseFocus && !applicationHasFocus);
        }

        public void SetGameMuted(bool muted)
        {
            Controllers[0].Muted = muted;
        }

        private AudioChannelSettingData[] GenerateAllChannels() => BaseChannels.Concat(AdditionalChannels).ToArray();
    }
}
