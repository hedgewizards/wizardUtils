using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WizardUtils.Dialogs
{
    public class DialogScreen : MonoBehaviour
    {
        public bool IsShowing => gameObject.activeSelf;

        public virtual void ShowLoading()
        {
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
