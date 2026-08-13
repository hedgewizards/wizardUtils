using System;
using UnityEngine;

namespace WizardUtils.UI.Pages
{
    public abstract class Page : MonoBehaviour, IPage
    {
        public event EventHandler<NavigateBackEventArgs> OnNavigateBack;
        public event EventHandler<NavigateToEventArgs> OnNavigateTo;
        public event EventHandler OnAppearing;
        public event EventHandler OnDisappearing;

        public void NavigateBack() => NavigateBack(false);
        public void NavigateBack(bool instant)
        {
            OnNavigateBack?.Invoke(this, new NavigateBackEventArgs(instant));
        }

        public void NavigateTo(PageDescriptor descriptor, bool instant = false)
        {
            NavigateTo(descriptor.Key, instant);
        }

        public void NavigateTo(string key, bool instant = false)
        {
            OnNavigateTo?.Invoke(this, new NavigateToEventArgs(key, instant));
        }

        public void NavigateTo(IPage page, bool instant = false)
        {
            OnNavigateTo?.Invoke(this, new NavigateToEventArgs(page, instant));
        }

        public abstract float AppearDurationSeconds { get; }
        public abstract float DisappearDurationSeconds { get; }

        public virtual void Appear(bool instant)
        {
            OnAppearing?.Invoke(this, null);
        }
        public virtual void Disappear(bool instant)
        {
            OnDisappearing?.Invoke(this, null);
        }

        #region IPage
        float IPage.AppearDurationSeconds => AppearDurationSeconds;

        float IPage.DisappearDurationSeconds => DisappearDurationSeconds;
        #endregion

    }
}
