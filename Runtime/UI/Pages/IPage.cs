using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardUtils.UI.Pages
{
    public interface IPage
    {
        public float AppearDurationSeconds { get; }
        public float DisappearDurationSeconds { get; }
        public void Appear(bool instant = false);
        public void Disappear(bool instant = false);
    }
}
