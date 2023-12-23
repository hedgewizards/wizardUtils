using System.Collections;
using UnityEngine;

namespace WizardUtils.Saving
{
    public abstract class SaveEditor : MonoBehaviour
    {
        public SaveValueDescriptor Save;
        public bool LoadOnAwake;
        public bool SaveOnSet;

        private void Start()
        {
            GameManager.Instance?.SubscribeMainSave(Save, CallChangedEvent);
            if (LoadOnAwake) CallChangedEvent(new SaveValueChangedEventArgs()
            {
                OldValue = Save.DefaultValue,
                NewValue = Save.SerializedValue
            });
        }

        public void Reload()
        {
            CallChangedEvent(new SaveValueChangedEventArgs()
            {
                OldValue = Save.DefaultValue,
                NewValue = Save.SerializedValue
            });
        }

        bool isSaving;
        public void SetString(string value)
        {
            if (isSaving) return;
            isSaving = true;
            var oldValue = Save.SerializedValue;
            Save.SerializedValue = value;
            if (SaveOnSet) SaveHelper.SaveData();
            CallChangedEvent(new SaveValueChangedEventArgs(oldValue, value));
            isSaving = false;
        }

        public string GetString() => Save.SerializedValue;

        protected abstract void CallChangedEvent(SaveValueChangedEventArgs args);

    }
}