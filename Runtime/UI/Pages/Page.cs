using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.Events;

namespace WizardUtils.UI.Pages
{
    public abstract class Page : MonoBehaviour, IPage
    {
        public event EventHandler OnNavigateBack;
        public event EventHandler<NavigateToEventArgs> OnNavigateTo;

        public void NavigateBack()
        {
            OnNavigateBack?.Invoke(this, EventArgs.Empty);
        }

        public void NavigateTo(PageDescriptor descriptor)
        {
            NavigateTo(descriptor.Key);
        }

        public void NavigateTo(string key)
        {
            OnNavigateTo?.Invoke(this, new NavigateToEventArgs(key));
        }

        public void NavigateTo(IPage page)
        {
            OnNavigateTo?.Invoke(this, new NavigateToEventArgs(page));
        }

        public abstract float AppearDurationSeconds { get; }
        public abstract float DisappearDurationSeconds { get; }

        public abstract void Appear(bool instant);
        public abstract void Disappear(bool instant);

        #region IPage
        float IPage.AppearDurationSeconds => AppearDurationSeconds;

        float IPage.DisappearDurationSeconds => DisappearDurationSeconds;
        #endregion

    }
}
