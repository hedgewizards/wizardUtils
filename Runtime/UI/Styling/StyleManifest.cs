using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using WizardUtils.ManifestPattern;

namespace WizardUtils.UI.Styling
{
    [CreateAssetMenu(fileName = "StyleManifest", menuName = "WizardUtils/UI/StyleManifest", order = 100)]
    public class StyleManifest : ScriptableObject, IDescriptorManifest<StyleDescriptor>
    {
        public StyleDescriptor[] Items;

        void Reset()
        {
            Items ??= new StyleDescriptor[0];
        }

        public void Add(StyleDescriptor descriptor)
        {
            ArrayHelper.InsertAndResize(ref Items, descriptor);
        }

        public bool Contains(StyleDescriptor descriptor)
        {
            return Items.Contains(descriptor);
        }

        public void Remove(StyleDescriptor descriptor)
        {
            ArrayHelper.DeleteAndResize(ref Items, descriptor);
        }

        public bool TryFindByKey(string key, out StyleDescriptor item)
        {
            item = Items
                .Where(i => i.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase))
                .FirstOrDefault();

            return item != null;
        }
    }
}
