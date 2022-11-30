using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WizardUtils.Extensions
{
    public static class PhysicsHelper
    {
        public static int MaskForLayer(int layer, int maxValidLayer = 31)
        {
            int currentMask = 0;

            for (int otherLayer = 0; otherLayer <= maxValidLayer; otherLayer++)
            {
                currentMask += Physics.GetIgnoreLayerCollision(layer, otherLayer) ? 0 : (1 << otherLayer);
            }

            return currentMask;
        }
    }
}
