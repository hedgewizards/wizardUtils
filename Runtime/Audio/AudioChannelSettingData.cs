namespace WizardUtils.Audio
{
    [System.Serializable]
    public struct AudioChannelSettingData
    {
        public string MixerParamName;
        public string GameSettingKey;
        public float DefaultValue;

        public AudioChannelSettingData(string mixerParamName, string gameSettingKey, float defaultValue)
        {
            MixerParamName = mixerParamName;
            GameSettingKey = gameSettingKey;
            DefaultValue = defaultValue;
        }
    }
}
