using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardUtils.UI.Pages
{
    public class NavigateToEventArgs
    {
        public NavigateToEventArgs(IPage page)
        {
            Page = page;
        }

        public NavigateToEventArgs(string key)
        {
            PageKey = key;
        }

        public IPage Page { get; private set; }
        public string PageKey { get; private set; }
    }
}
