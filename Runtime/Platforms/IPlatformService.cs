using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WizardUtils.GameSettings;

namespace Platforms
{
    public interface IPlatformService
    {
        public string PlatformURLName { get; }
        public string PersistentDataPath { get; }
        public void OnEnable();
        public void OnDestroy();
    }
}
