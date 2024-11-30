using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WizardUtils.ManifestPattern;
using WizardUtils.UI.Pages;

namespace WizardUtils
{
    public abstract class DescriptorManifest : ScriptableObject
    {
        public abstract void Add(object descriptor);
        public abstract bool Contains(object descriptor);
        public abstract void Remove(object descriptor);
    }
}
