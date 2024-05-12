using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WizardUtils.UI.Pages
{
    public class PageSource
    {
        private Transform Parent;
        private PageManifest Manifest;
        private Dictionary<string, PageData> ExistingPages;

        public PageSource(PageManifest manifest, Transform parent)
        {
            Manifest = manifest;
            ExistingPages = new Dictionary<string, PageData>();
            Parent = parent;
        }

        public IPage SpawnNew(PageDescriptor descriptor)
        {
            GameObject newObj = UnityEngine.Object.Instantiate(descriptor.Prefab, Parent.transform);
            return newObj.GetComponent<IPage>();
        }

        public IPage Get(string key)
        {
            // grab an existing page if it exists
            if (ExistingPages.TryGetValue(key, out PageData pageData))
            {
                return pageData.Page;
            }

            if (!Manifest.TryFindByKey(key, out PageDescriptor descriptor))
            {
                throw new KeyNotFoundException($"Missing page with key {key} in manifest {Manifest}");
            }

            PageData data = new PageData()
            {
                Descriptor = descriptor,
                Page = SpawnNew(descriptor)
            };
#if DEBUG
            if (data.Page == null) throw new MissingComponentException($"Page {descriptor}'s prefab missing a toplevel IPage component");
#endif
            ExistingPages[key] = data;

            return data.Page;
        }

        public IPage Get(PageDescriptor descriptor) => Get(descriptor.Key);

        private struct PageData
        {
            public IPage Page;
            public PageDescriptor Descriptor;
        }
    }
}
