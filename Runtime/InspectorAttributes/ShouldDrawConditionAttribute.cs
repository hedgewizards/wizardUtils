using UnityEngine;

namespace WizardUtils.InspectorAttributes
{
    public class ShouldDrawConditionAttribute : PropertyAttribute
    {
        public string ShouldHideMethodName;

        public ShouldDrawConditionAttribute(string shouldHideMethodName)
        {
            ShouldHideMethodName = shouldHideMethodName;
        }
    }
}
