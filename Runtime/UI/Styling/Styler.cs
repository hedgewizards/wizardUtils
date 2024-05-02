using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardUtils.UI.Styling
{
    public class Styler<T>
    {
        private Dictionary<string, Action<string, T>> Setters;
        private Action<T> Callback;

        public Styler()
        {

        }

        public void Apply(Style style, T target)
        {
            foreach (var entry in style.Entries)
            {
                if (Setters.TryGetValue(entry.Key, out var action))
                {
                    action(entry.Value, target);
                }
            }
            Callback?.Invoke(target);
        }

        public Styler<T> Add(string key, Action<string, T> action)
        {
            Setters.Add(key, action);
            return this;
        }

        public Styler<T> AddCallback(Action<T> action)
        {
            Callback = action;
            return this;
        }
    }
}
