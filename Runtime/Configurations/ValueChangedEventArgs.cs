using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardUtils.Configurations
{
    public class ValueChangedEventArgs : EventArgs
    {
        public readonly string Key;
        public readonly string OldValue;
        public readonly string NewValue;

        public ValueChangedEventArgs(string key, string oldValue, string newValue)
        {
            Key = key;
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}
