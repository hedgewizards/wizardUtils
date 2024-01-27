using Platforms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WizardUtils.GameSettings;

namespace WizardUtils.Configurations
{
    public class CfgFileConfiguration : ITwoWayConfiguration
    {
        private IPlatformService PlatformService;
        
        private string FilePath => $"{PlatformService.PersistentDataPath}{Path.DirectorySeparatorChar}{FileName}.cfg";
        private string FileName;

        private readonly Dictionary<string, string> Data;

        public string Read(string key) => Data[key];
        public void Write(string key, string value) => Data[key] = value;

        public IEnumerable<KeyValuePair<string, string>> Values => Data;

        public event EventHandler<ValueChangedEventArgs> OnValueChanged;

        public CfgFileConfiguration(IPlatformService platformService, string fileName)
        {
            PlatformService = platformService;
            FileName = fileName;
            var rawData = ReadData(FilePath);
            Data = new Dictionary<string, string>();
            foreach(var pair in rawData)
            {
                Data.Add(pair.Key, pair.Value);
            }
        }

        public void Save()
        {
            WriteData(FilePath, Data);
        }

        private static void WriteData(string filePath, IEnumerable<KeyValuePair<string, string>> data)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                CfgFileSerializationHelper.Serialize(filePath, data);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogWarning($"Settings save ERROR Failed to write to {filePath}: {e}");
                return;
            }
        }


        private static IEnumerable<KeyValuePair<string,string>> ReadData(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return new KeyValuePair<string, string>[0];
            }

            return CfgFileSerializationHelper.Deserialize(filePath);
        }

    }
}
