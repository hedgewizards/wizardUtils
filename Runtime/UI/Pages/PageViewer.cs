using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WizardUtils.UI.Pages
{
    public class PageViewer : MonoBehaviour
    {
        private IPage CurrentPage;
        public PageManifest PageManifest;
        private PageSource PageSource;
        private Coroutine CurrentSwapAction;

        public void Awake()
        {
            PageSource = new PageSource(PageManifest, transform);
        }

        public void Open(PageDescriptor descriptor, bool instant = false) => Open(descriptor.Key, instant);
        public void Open(string pageKey, bool instant = false)
        {
            IPage page = PageSource.Get(pageKey);
            page.Disappear();
            Open(page, instant);
        }
        public void Open(IPage page, bool instant = false)
        {
        
        }
    }
}
