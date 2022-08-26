namespace WizardUtils.Saving
{
    public class SaveValueChangedEventArgs
    {
        public string OldValue;
        public string NewValue;

        public SaveValueChangedEventArgs()
        {
        }

        public SaveValueChangedEventArgs(string oldValue, string newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}