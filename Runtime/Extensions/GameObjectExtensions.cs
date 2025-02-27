using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WizardUtils
{
    public static class GameObjectExtensions
    {
        public static string GetHierarchyPath(this GameObject self)
        {
            if (self == null)
                return string.Empty;

            StringBuilder pathBuilder = new StringBuilder(self.name);
            Transform parent = self.transform.parent;

            while (parent != null)
            {
                pathBuilder.Insert(0, parent.name + "/");
                parent = parent.parent;
            }

            return pathBuilder.ToString();
        }
    }
}
