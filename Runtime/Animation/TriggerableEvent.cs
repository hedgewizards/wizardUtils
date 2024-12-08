using UnityEngine;
using UnityEngine.Events;

namespace WizardUtils.Animation
{
    public class TriggerableEvent : MonoBehaviour
    {
        public UnityEvent OnTriggered;

        public void Trigger()
        {
            OnTriggered.Invoke();
        }
    }
}
