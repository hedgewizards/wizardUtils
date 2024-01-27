﻿using Platforms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using WizardUtils.GameSettings.Legacy;

namespace WizardUtils.GameSettings
{
    [Obsolete]
    public class ConfigFileGameSettingService : IGameSettingService
    {
        Dictionary<string, LegacyGameSettingFloat> GameSettings;
        private IPlatformService PlatformService;
        private string FilePath => $"{PlatformService.PersistentDataPath}{Path.DirectorySeparatorChar}{FileName}.cfg";
        private string FileName;
        private bool IsDirty;
        private bool IsLoading;

        public ConfigFileGameSettingService(IPlatformService platformService, string fileName, IEnumerable<LegacyGameSettingFloat> settings)
        {
            PlatformService = platformService;
            FileName = fileName;
            GameSettings = new Dictionary<string, LegacyGameSettingFloat>();
            foreach(var setting in settings)
            {
                RegisterGameSetting(setting);
            }
            Load();
        }

        public LegacyGameSettingFloat GetSetting(string key)
        {
            return GameSettings[key];
        }
        public void Save()
        {
            if (!IsDirty) return;

            Tuple<string, float>[] data = new Tuple<string, float>[GameSettings.Count];
            int offset = 0;
            foreach (var kv in GameSettings)
            {
                data[offset++] = new Tuple<string, float>(kv.Key, kv.Value.Value);
            }

            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(FilePath));
                GameSettingsSerializationHelper.SerializeSettings(FilePath, GameSettings.Select(x => new Tuple<string, float>(x.Value.Key, x.Value.Value)));
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogWarning($"Settings save ERROR Failed to write {FileName}.cfg: {e}");
                return;
            }
        }
        private void RegisterGameSetting(LegacyGameSettingFloat newSetting)
        {
            GameSettings.Add(newSetting.Key, newSetting);
            newSetting.OnChanged += (sender, e) => OnGameSettingChanged(newSetting, e);
        }

        private void OnGameSettingChanged(LegacyGameSettingFloat setting, GameSettingChangedEventArgs<float> e)
        {
            // we don't need to do any
            if (IsLoading) return;

            IsDirty = true;
        }

        private void Load()
        {
            IsLoading = true;
            IEnumerable<Tuple<string, float>> data = ReadData();

            foreach(var pair in data)
            {
                if (GameSettings.TryGetValue(pair.Item1, out var gameSetting))
                {
                    gameSetting.Value = pair.Item2;
                }
                else
                {
                    // register extra settings so they will get serialized when we re-save the config
                    RegisterGameSetting(new LegacyGameSettingFloat(pair.Item1, pair.Item2));
                }
            }
            IsLoading = false;
        }

        private IEnumerable<Tuple<string, float>> ReadData()
        {
            if (!File.Exists(FilePath))
            {
                IsDirty = true;
                return new Tuple<string, float>[0];
            }
            
            try
            {
                return GameSettingsSerializationHelper.DeserializeSettings(FilePath);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogWarning($"Settings load ERROR Failed to read {FileName}.cfg: {e}");
                IsDirty = true;
                return new Tuple<string, float>[0];
            }
        }
    }
}
