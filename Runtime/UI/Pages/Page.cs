using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.PackageManager.UI;
using UnityEngine;

namespace WizardUtils.UI.Pages
{
    public abstract class Page : MonoBehaviour, IPage
    {
        public NavigationStack NavigationStack { get; private set; }

        public abstract float AppearDurationSeconds { get; }
        public abstract float DisappearDurationSeconds { get; }

        public abstract void Appear(bool instant);
        public abstract void Disappear(bool instant);

        #region IPage
        float IPage.AppearDurationSeconds => AppearDurationSeconds;

        float IPage.DisappearDurationSeconds => DisappearDurationSeconds;
        #endregion

        void IPage.SetParent(NavigationStack newStack)
        {
            NavigationStack = newStack;
        }
    }
}
