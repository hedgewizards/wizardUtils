using UnityEngine;
using UnityEngine.Events;
using WizardUtils.Configurations;

namespace WizardUtils.Saving
{
    public class SaveEditorColor : SaveEditor
    {
        public UnityEvent<Color> OnColorChanged;

        public void SetColor(Color color)
        {
            SetString(SaveHelper.SerializeColor(color));
        }

        public Color GetColor()
        {
            return Save.ColorValue;
        }

        protected override void CallChangedEvent(object sender, ValueChangedEventArgs args)
        {
            if (SaveHelper.ParseColor(args.NewValue, out Color newColor))
            {
                OnColorChanged?.Invoke(newColor);
            }
        }
    }
}
