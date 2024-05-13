using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using WizardUtils.SettingWatchers;

namespace WizardUtils.Platforms.Portable
{
    public class PortablePlatformService : IPlatformService
    {
        public string PersistentDataPath => Application.persistentDataPath;

        public string PlatformURLName => "portable";

        public void OnEnable()
        {

        }

        public void OnDestroy()
        {

        }
    }
}
