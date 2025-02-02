using UnityEditor.Animations;
using UnityEngine;

namespace WizardUtils.Extensions
{
    public static class AnimatorStateMachineExtensions
    {
        public static AnimatorStateMachine FindChildStateMachine(this AnimatorStateMachine self, string name)
        {
            for (int n = 0; n < self.stateMachines.Length; n++)
            {
                if (self.stateMachines[n].stateMachine.name == name)
                {
                    return self.stateMachines[n].stateMachine;
                }
            }

            return null;
        }

        public static AnimatorState FindChildState(this AnimatorStateMachine self, string name)
        {
            for (int n = 0; n < self.states.Length; n++)
            {
                if (self.states[n].state.name == name)
                {
                    return self.states[n].state;
                }
            }

            return null;
        }

        public static AnimatorState FindChildState(this AnimatorStateMachine self, Motion motion)
        {
            for (int n = 0; n < self.states.Length; n++)
            {
                if (self.states[n].state.motion == motion)
                {
                    return self.states[n].state;
                }
            }

            return null;
        }

        public static int IndexOfChildState(this AnimatorStateMachine self, string name)
        {
            for (int n = 0; n < self.states.Length; n++)
            {
                if (self.states[n].state.name == name)
                {
                    return n;
                }
            }

            return -1;
        }

        public static int IndexOfChildState(this AnimatorStateMachine self, Motion motion)
        {
            for (int n = 0; n < self.states.Length; n++)
            {
                if (self.states[n].state.motion == motion)
                {
                    return n;
                }
            }

            return -1;
        }


        public static AnimatorTransition FindEntryTransition(this AnimatorStateMachine self, AnimatorState destinationState)
        {

            for (int n = 0; n < self.entryTransitions.Length; n++)
            {
                if (self.entryTransitions[n].destinationState == destinationState)
                {
                    return self.entryTransitions[n];
                }
            }

            return null;
        }

        public static AnimatorStateTransition FindAnyStateTransition(this AnimatorStateMachine self, AnimatorState destinationState)
        {

            for (int n = 0; n < self.anyStateTransitions.Length; n++)
            {
                if (self.anyStateTransitions[n].destinationState == destinationState)
                {
                    return self.anyStateTransitions[n];
                }
            }

            return null;
        }
    }
}
