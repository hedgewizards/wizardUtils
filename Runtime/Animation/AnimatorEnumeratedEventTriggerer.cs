using UnityEngine;

namespace WizardUtils.Animations
{
    public class AnimatorEnumeratedEventTriggerer : MonoBehaviour
    {
        public Animator Target;
        public string EventIdParameterName;
        public string EventTriggerParameterName;

        public void PlayEnumeratedEvent(int eventId)
        {
            Target.SetInteger(EventIdParameterName, eventId);
            Target.SetTrigger(EventTriggerParameterName);
        }
    }
}
