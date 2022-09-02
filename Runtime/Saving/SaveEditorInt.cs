using UnityEngine;
using UnityEngine.Events;

namespace WizardUtils.Saving
{
    public class SaveEditorInt : SaveEditor
    {
        public UnityEvent<int> OnIntChanged;

        public void SetInt(int value)
        {
            SetString(SaveHelper.SerializeInt(value));
        }

        public int GetInt()
        {
            return Save.IntValue;
        }

        protected override void CallChangedEvent(SaveValueChangedEventArgs args)
        {
            if (SaveHelper.ParseInt(args.NewValue, out int newValue))
            {
                OnIntChanged?.Invoke(newValue);
            }
        }
    }
}
