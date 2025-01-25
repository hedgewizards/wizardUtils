using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;

namespace WizardUtils.UI
{
    public static class TextMeshProExtensions
    {
        public static float GetHorizontalMarginSize(this TextMeshProUGUI self)
        {
            return self.margin.x + self.margin.z;
        }
        public static float GetVerticalMarginSize(this TextMeshProUGUI self)
        {
            return self.margin.y + self.margin.w;
        }
    }
}
