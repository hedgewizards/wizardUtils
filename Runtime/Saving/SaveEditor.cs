using System.Collections;
using UnityEngine;

namespace WizardUtils.Saving
{
    public abstract class SaveEditor : MonoBehaviour
    {
        public SaveValueDescriptor Save;
        public bool LoadOnAwake;

        private void Start()
        {
            GameManager.GameInstance?.SubscribeMainSave(Save, CallChangedEvent);
            if (LoadOnAwake) CallChangedEvent(new SaveValueChangedEventArgs()
            {
                OldValue = Save.DefaultValue,
                NewValue = Save.SerializedValue
            });
        }

        protected abstract void CallChangedEvent(SaveValueChangedEventArgs args);

        protected void SaveData()
        {
            GameManager.GameInstance?.SaveData();
        }
    }
}