using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardUtils.Configuration.SettingWatchers
{
    public class SettingChangedEventArgs<T>
    {
        public T InitialValue;
        public T FinalValue;

        public SettingChangedEventArgs(T initialValue, T finalValue)
        {
            InitialValue = initialValue;
            FinalValue = finalValue;
        }
    }
}
