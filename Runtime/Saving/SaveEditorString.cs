using UnityEngine;
using UnityEngine.Events;

namespace WizardUtils.Saving
{
    public class SaveEditorString : SaveEditor
    {
        public UnityEvent<string> OnStringChanged;

        protected override void CallChangedEvent(SaveValueChangedEventArgs args)
        {
            if (args.NewValue == args.OldValue) return;
            OnStringChanged?.Invoke(args.NewValue);
        }
    }
}
