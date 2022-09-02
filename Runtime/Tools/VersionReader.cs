using UnityEngine;
using UnityEngine.Events;

namespace WizardUtils
{
    public class VersionReader : MonoBehaviour
    {
        public UnityEvent<string> OnReadVersion;

        private void Awake()
        {
            OnReadVersion?.Invoke($"v{Application.version}");
        }
    }
}
