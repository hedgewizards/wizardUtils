using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardUtils.GameSettings
{
    public interface IGameSetting<T>
    {
        public T Value { get; set; }

        public event EventHandler<GameSettingChangedEventArgs<T>> OnChanged;
    }
}
