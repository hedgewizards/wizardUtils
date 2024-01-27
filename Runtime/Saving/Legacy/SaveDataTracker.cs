using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WizardUtils.Saving
{
    [Obsolete]
    public abstract class SaveDataTracker
    {
        public Dictionary<SaveValueDescriptor, SaveValue> LoadedValues;
        public SaveManifest Manifest;
        protected SaveDataTracker(SaveManifest manifest)
        {
            Manifest = manifest;
        }

        public string Read(SaveValueDescriptor descriptor)
        {
            return GetSaveValue(descriptor).StringValue;
        }

        public SaveValue GetSaveValue(SaveValueDescriptor descriptor)
        {
            if (LoadedValues.TryGetValue(descriptor, out SaveValue value))
            {
                return value;
            }

#if UNITY_EDITOR
            _ = ValidateDescriptor(descriptor);
#endif

            SaveValue newValue = AddFromDescriptor(descriptor);

            return newValue;
        }

        public void Write(SaveValueDescriptor descriptor, string stringValue)
        {
            SaveValue saveValue = GetSaveValue(descriptor);
            var oldValue = saveValue.StringValue;
            saveValue.StringValue = stringValue;
            saveValue.OnValueChanged?.Invoke(new SaveValueChangedEventArgs(oldValue, stringValue));
        }

        public abstract void Save();

        public abstract void Load();

        private bool ValidateDescriptor(SaveValueDescriptor descriptor)
        {
            if (Manifest.ContainsDescriptor(descriptor))
            {
                return true;
            }
            else
            {
                Debug.LogWarning($"Missing SaveValueDescriptor {descriptor} for manifest {Manifest}");
                return false;
            }
        }

        private SaveValue AddFromDescriptor(SaveValueDescriptor descriptor)
        {
            var value = new SaveValue(descriptor);
            LoadedValues[descriptor] = value;

            return value;
        }
    }
}
