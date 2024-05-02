using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WizardUtils.UI.Styling;

[CreateAssetMenu(fileName = "p_", menuName = "WizardUtils/UI/StyleDescriptor")]
public class StyleDescriptor : ScriptableObject
{
    public string Key;
    public Style Style;
}
