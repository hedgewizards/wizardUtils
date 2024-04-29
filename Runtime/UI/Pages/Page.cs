﻿using System;
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
        public event EventHandler<NavigateBackEventArgs> OnNavigateBack;
        public event EventHandler<NavigateToEventArgs> OnNavigateTo;

        public void NavigateBack(bool instant = false)
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

        public abstract void Appear(bool instant);
        public abstract void Disappear(bool instant);

        #region IPage
        float IPage.AppearDurationSeconds => AppearDurationSeconds;

        float IPage.DisappearDurationSeconds => DisappearDurationSeconds;
        #endregion

    }
}
