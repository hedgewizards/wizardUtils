using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WizardUtils.ManifestPattern;
using WizardUtils.UI.Pages;

namespace WizardUtils
{
    public class DescriptorManifest<T> : ScriptableObject, IDescriptorManifest<T>
        where T : ManifestedDescriptor
    {
        public List<T> Items;

        void Reset()
        {
            Items ??= new List<T>();
        }

        public void Add(T descriptor)
        {
            Items.Add(descriptor);
        }

        public bool Contains(T descriptor)
        {
            return Items.Contains(descriptor);
        }

        public void Remove(T descriptor)
        {
            Items.Remove(descriptor);
        }

        public bool TryFindByKey(string key, out T item)
        {
            item = Items
                .Where(i => i.GetKey().Equals(key, StringComparison.InvariantCultureIgnoreCase))
                .FirstOrDefault();

            return item != null;
        }
    }
}
