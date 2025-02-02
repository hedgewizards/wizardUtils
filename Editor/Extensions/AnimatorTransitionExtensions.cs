using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Animations;

namespace WizardUtils.Extensions
{
    public static class AnimatorTransitionBaseExtensions
    {
        /// <summary>
        /// Update the properties of <see cref="self"/>'s first condition that has the same <see cref="parameter"/>,
        /// or creates it if it doesn't exist
        /// </summary>
        /// <param name="self"></param>
        /// <param name="parameter"></param>
        /// <param name="threshold"></param>
        /// <exception cref=""></exception>
        public static void AddOrUpdateCondition(
            this AnimatorTransitionBase self,
            AnimatorConditionMode mode,
            float threshold,
            string parameter)
        {

            for (int n = 0; n < self.conditions.Length; n++)
            {
                if (self.conditions[n].parameter == parameter)
                {
                    AnimatorCondition condition = self.conditions[n];
                    condition.threshold = threshold;
                    condition.mode = mode;
                    self.conditions[n] = condition;
                    return;
                }
            }

            self.AddCondition(mode, threshold, parameter);
        }
    }
}
