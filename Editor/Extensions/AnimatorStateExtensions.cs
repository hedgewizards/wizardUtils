using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Animations;

namespace WizardUtils.Extensions
{
    public static class AnimatorStateExtensions
    {
        public static AnimatorStateTransition FindExitTransition(this AnimatorState self)
        {
            for (int n = 0; n < self.transitions.Length; n++)
            {
                if (self.transitions[n].destinationState == null)
                {
                    return self.transitions[n];
                }
            }

            return null;
        }

        public static AnimatorStateTransition FindTransition(this AnimatorState self, Func<AnimatorStateTransition, bool> filter)
        {
            for (int n = 0; n < self.transitions.Length; n++)
            {
                if (filter(self.transitions[n]))
                {
                    return self.transitions[n];
                }
            }

            return null;
        }
    }
}
