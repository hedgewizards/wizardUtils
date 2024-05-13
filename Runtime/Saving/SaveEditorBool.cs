using UnityEngine;
using UnityEngine.Events;
using WizardUtils.Configurations;

namespace WizardUtils.Saving
{
    public class SaveEditorBool : SaveEditor
    {
        public UnityEvent OnSetFalse;
        public UnityEvent OnSetTrue;

        public void SetBool(bool newValue)
        {
            SetString(ConfigHelper.SerializeBool(newValue));
        }

        protected override void CallChangedEvent(object sender, ValueChangedEventArgs args)
        {
            if (args.NewValue == args.OldValue) return;
            if (ConfigHelper.ParseBool(args.NewValue, false))
            {
                OnSetTrue?.Invoke();
            }
            else
            {
                OnSetFalse?.Invoke();
            }
        }
    }
}
