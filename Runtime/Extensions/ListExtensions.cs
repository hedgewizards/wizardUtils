using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardUtils.Extensions
{
    public static class ListExtensions
    {
        public static bool TryGetAtIndex<T>(this List<T> self, int index, out T result)
        {
            if (index >= self.Count)
            {
                result = default;
                return false;
            }
            result = self[index];
            return true;
        }

    }
}
