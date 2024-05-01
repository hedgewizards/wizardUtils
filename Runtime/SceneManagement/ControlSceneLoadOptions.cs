using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardUtils.SceneManagement
{
    public class ControlSceneLoadOptions
    {
        /// <summary>
        /// If loading finishes early, keep the loading screen open and delay the callback until at least this many seconds pass
        /// </summary>
        public float MinimumLoadDurationSeconds = 0;
        /// <summary>
        /// If true, show loading screen with <see cref="Dialogs.DialogScreen.ShowLoading"/> and <see cref="Dialogs.DialogScreen.Hide"/><br/>
        /// If false, the caller should handle showing and hiding themself
        /// </summary>
        public bool DoDefaultLoadingScreenBehavior = true;
    }
}
