using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WizardUtils.ManifestPattern;
using WizardUtils.UI.Styling;

[CreateAssetMenu(fileName = "p_", menuName = "WizardUtils/UI/StyleDescriptor")]
public class StyleDescriptor : ManifestedDescriptor<StyleManifest>
{
    public string Key;
    public Style Style;

    public override string GetKey() => Key;
}
