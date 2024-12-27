using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WizardUtils.Dialogs
{
    [RequireComponent(typeof(Canvas))]
    public class DialogScreen : MonoBehaviour
    {
        public bool IsShowing => gameObject.activeSelf;

        public void Awake()
        {
            GetComponent<Canvas>().worldCamera = GameManager.Instance.UICamera;
        }

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
