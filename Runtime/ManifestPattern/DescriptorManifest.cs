using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WizardUtils.ManifestPattern;

namespace WizardUtils
{
    public class DescriptorManifest<T> : ScriptableObject, IDescriptorManifest<T>
        where T : ScriptableObject
    {
        public List<T> Descriptors;

        public void Add(T descriptor)
        {
            Descriptors.Add(descriptor);
        }

        public bool Contains(T descriptor)
        {
            return Descriptors.Contains(descriptor);
        }

        public void Remove(T descriptor)
        {
            Descriptors.Remove(descriptor);
        }
    }
}
