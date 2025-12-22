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
        public string Title;

        public FlagBoolAttribute(string channel = "default", string title = null)
        {
            Channel = channel;
            Title = title;
        }
    }
}
