using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WizardUtils.AssetPalettes
{
    [CreateAssetMenu(fileName = "New Asset Palette", menuName = "WizardUtils/Asset Palette")]
    public class AssetPalette : ScriptableObject
    {
        public AssetPaletteEntry[] Entries;
    }
}
