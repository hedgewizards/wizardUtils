using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WizardUtils.UI.Pages
{
    [CreateAssetMenu(fileName = "p_", menuName = "WizardUtils/UI/PageDescriptor")]
    public class PageDescriptor : ScriptableObject
    {
        public string Key;
        public GameObject Prefab;
    }
}
