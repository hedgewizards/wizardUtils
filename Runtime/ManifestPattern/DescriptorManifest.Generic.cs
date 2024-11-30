using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WizardUtils.ManifestPattern;
using WizardUtils.UI.Pages;

namespace WizardUtils
{
    public class DescriptorManifest<T> : DescriptorManifest, IDescriptorManifest<T>
        where T : ManifestedDescriptor
    {
        public List<T> Items;

        void Reset()
        {
            Items ??= new List<T>();
        }

        public override void Add(object descriptor) => Add((T)descriptor);

        public override bool Contains(object descriptor) => Contains((T)descriptor);

        public override void Remove(object descriptor) => Remove((T)descriptor);

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
