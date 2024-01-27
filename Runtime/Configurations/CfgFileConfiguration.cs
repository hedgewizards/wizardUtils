using Platforms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardUtils.Configurations
{
    public class CfgFileConfiguration : IConfiguration
    {
        private IPlatformService PlatformService;
        
        private string FilePath => $"{PlatformService.PersistentDataPath}{Path.DirectorySeparatorChar}{FileName}.cfg";
        private string FileName;

        private readonly Dictionary<string, string> Data;

        public string this[string key] => Data[key];

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
                Data.Add(pair.Item1, pair.Item2);
            }
        }

        private static IEnumerable<Tuple<string,string>> ReadData(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return new Tuple<string, string>[0];
            }

            return CfgFileSerializationHelper.Deserialize(filePath);
        }

    }
}
