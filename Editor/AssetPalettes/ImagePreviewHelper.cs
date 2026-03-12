using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UIElements;

namespace WizardUtils.AssetPalettes
{
    public static class ImagePreviewHelper
    {
        public static void LazyLoadAssetPreview(this Image self, UnityEngine.Object asset, int attempts = 50)
        {
            if (!InternalLazyLoadAssetPreview(self, asset) && attempts > 0)
            {
                self.schedule.Execute(() => LazyLoadAssetPreview(self, asset, attempts - 1)).StartingIn(100);
            }
        }

        private static bool InternalLazyLoadAssetPreview(Image image, UnityEngine.Object asset)
        {
            if (image == null) return true;

            Texture preview = AssetPreview.GetAssetPreview(asset);
            if (preview != null)
            {
                image.image = preview;
                return true;
            }

            image.image = AssetPreview.GetMiniThumbnail(asset) ?? Texture2D.grayTexture;
            return false;
        }
    }
}
