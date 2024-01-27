using System;
using System.Collections;
using UnityEngine;
using WizardUtils.Configurations;

namespace WizardUtils.Saving
{
    public abstract class SaveEditor : MonoBehaviour
    {
        private GameManager gameManager;

        public SaveValueDescriptor Save;
        public bool LoadOnAwake;
        public bool SaveOnSet;

        private void Start()
        {
            gameManager = GameManager.Instance;

            gameManager.Configuration.AddListener(Save.Key, CallChangedEvent);
            if (LoadOnAwake) Reload();
        }

        public void Reload()
        {
            CallChangedEvent(this, new ValueChangedEventArgs(Save.Key, Save.DefaultValue, GetString()));
        }

        public void SetString(string value)
        {
            gameManager.Configuration.Write(Save.Key, value);
        }

        public string GetString() => gameManager.Configuration.Read(Save.Key, Save.DefaultValue);

        protected abstract void CallChangedEvent(object sender, ValueChangedEventArgs args);

    }
}