using UnityEngine.Events;

namespace WizardUtils.UI.Basics
{
    public interface IListableButton
    {
        public void AddOnClickListener(UnityAction action);
        public void SetText(string text);
    }
}
