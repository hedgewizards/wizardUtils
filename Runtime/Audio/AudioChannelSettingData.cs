namespace WizardUtils.Audio
{
    [System.Serializable]
    public struct AudioChannelSettingData
    {
        public string MixerParamName;
        public string SettingKey;
        public float DefaultValue;

        public AudioChannelSettingData(string mixerParamName, string settingWatcherKey, float defaultValue)
        {
            MixerParamName = mixerParamName;
            SettingKey = settingWatcherKey;
            DefaultValue = defaultValue;
        }
    }
}
