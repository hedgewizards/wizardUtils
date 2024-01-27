using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WizardUtils.Configurations
{
    [CreateAssetMenu(fileName = "New Config", menuName = "WizardUtils/Config File")]
    public class ExplicitConfigurationData : ScriptableObject
    {
        public ExplicitConfigEntry[] Entries;
    }
}
