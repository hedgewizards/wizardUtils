using System;
using UnityEngine;

namespace WizardUtils.InspectorAttributes
{
    /// <summary>
    /// Hides this field, to be drawn by a FlagBoolDropdown in this object's editor script
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class FlagBoolAttribute : PropertyAttribute
    {
        public string Channel;

        public FlagBoolAttribute(string channel = "default")
        {
            Channel = channel;
        }
    }
}
