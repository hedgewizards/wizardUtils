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
            SetString(SaveHelper.SerializeBool(newValue));
        }

        protected override void CallChangedEvent(object sender, ValueChangedEventArgs args)
        {
            if (args.NewValue == args.OldValue) return;
            if (SaveHelper.ParseBool(args.NewValue))
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
