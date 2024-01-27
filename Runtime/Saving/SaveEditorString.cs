using UnityEngine;
using UnityEngine.Events;
using WizardUtils.Configurations;

namespace WizardUtils.Saving
{
    public class SaveEditorString : SaveEditor
    {
        public UnityEvent<string> OnStringChanged;

        protected override void CallChangedEvent(object sender, ValueChangedEventArgs args)
        {
            if (args.NewValue == args.OldValue) return;
            OnStringChanged?.Invoke(args.NewValue);
        }
    }
}
