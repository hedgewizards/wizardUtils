using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardUtils.GameSettings
{
    public class GameSettingChangedEventArgs<T>
    {
        public T InitialValue;
        public T FinalValue;

        public GameSettingChangedEventArgs(T initialValue, T finalValue)
        {
            InitialValue = initialValue;
            FinalValue = finalValue;
        }
    }
}
