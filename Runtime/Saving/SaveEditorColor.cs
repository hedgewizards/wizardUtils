using UnityEngine;
using UnityEngine.Events;

namespace WizardUtils.Saving
{
    public class SaveEditorColor : SaveEditor
    {
        public UnityEvent<Color> OnColorChanged;

        public void SetColor(Color color)
        {
            Save.ColorValue = color;
            SaveData();
        }

        public Color GetColor()
        {
            return Save.ColorValue;
        }

        protected override void CallChangedEvent(SaveValueChangedEventArgs args)
        {
            if (SaveValueHelper.ParseColor(args.NewValue, out Color newColor))
            {
                OnColorChanged?.Invoke(newColor);
            }
        }
    }
}
