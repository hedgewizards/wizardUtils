using UnityEngine;

namespace WizardUtils.InspectorAttributes
{
    public class ShouldDrawConditionAttribute : PropertyAttribute
    {
        public string ShouldDrawMethodName;

        public ShouldDrawConditionAttribute(string shouldDrawMethodName)
        {
            ShouldDrawMethodName = shouldDrawMethodName;
        }
    }
}
