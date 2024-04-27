using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace WizardUtils.UI
{
    public class VersionText : MonoBehaviour
    {
        public TextMeshProUGUI Text;

        private void Awake()
        {
            Text.text = $"v{Application.version}";
        }
    }
}
