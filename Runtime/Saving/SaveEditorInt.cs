using UnityEngine;
using UnityEngine.Events;
using WizardUtils.Configurations;

namespace WizardUtils.Saving
{
    public class SaveEditorInt : SaveEditor
    {
        public UnityEvent<int> OnIntChanged;

        public void SetInt(int value)
        {
            SetString(ConfigHelper.SerializeInt(value));
        }

        public int GetInt()
        {
            return Save.IntValue;
        }

        protected override void CallChangedEvent(object sender, ValueChangedEventArgs args)
        {
            if (ConfigHelper.TryParseInt(args.NewValue, out int newValue))
            {
                OnIntChanged?.Invoke(newValue);
            }
        }
    }
}
