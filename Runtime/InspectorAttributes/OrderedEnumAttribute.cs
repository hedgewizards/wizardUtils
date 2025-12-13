using UnityEngine;
using System;

namespace WizardUtils.InspectorAttributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class OrderedEnumAttribute : PropertyAttribute { }
}