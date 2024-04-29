using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardUtils.UI.Pages
{
    public class NavigateToEventArgs
    {
        public NavigateToEventArgs(IPage page, bool instant = false)
        {
            Page = page;
            Instant = instant;
        }

        public NavigateToEventArgs(string key, bool instant = false)
        {
            PageKey = key;
            Instant = instant;
        }

        public IPage Page { get; private set; }
        public string PageKey { get; private set; }
        public bool Instant { get; private set; }
    }
}
