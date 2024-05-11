using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using WizardUtils.ManifestPattern;

namespace WizardUtils.UI.Pages
{
    [CreateAssetMenu(fileName = "PageManifest", menuName = "WizardUtils/UI/PageManifest", order = 100)]
    public class PageManifest : ScriptableObject, IDescriptorManifest<PageDescriptor>
    {
        public PageDescriptor[] Items;

        void Reset()
        {
            Items ??= new PageDescriptor[0];
        }

        public void Add(PageDescriptor descriptor)
        {
            ArrayHelper.InsertAndResize(ref Items, descriptor);
        }

        public bool Contains(PageDescriptor descriptor)
        {
            return Items.Contains(descriptor);
        }

        public void Remove(PageDescriptor descriptor)
        {
            ArrayHelper.DeleteAndResize(ref Items, descriptor);
        }

        public bool TryFindByKey(string key, out PageDescriptor item)
        {
            item = Items
                .Where(i => i.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase))
                .FirstOrDefault();

            return item != null;
        }
    }
}
