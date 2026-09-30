using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace WizardUtils.AssetPalettes
{
    public class AssetPaletteWindow : EditorWindow
    {
        #region Constants

        #endregion

        #region Components
        private AssetGridWidget<AssetPalette> PalettesWidget;
        private AssetPaletteWidget AssetsWidget;
        private ScrollView scrollView;
        #endregion

        #region Variables
        public List<AssetPalette> AssetPalettes;
        public AssetPalette SelectedPalette;
        #endregion


        [MenuItem("Window/WizardUtils/Asset Palette")]
        private static void ShowWindow()
        {
            var window = GetWindow<AssetPaletteWindow>("Asset Palette");
            window.Show();
        }

        private void OnEnable()
        {
            if (AssetPalettes == null)
            {
                AssetPalettes = new List<AssetPalette>();
            }

            rootVisualElement.Clear();

            scrollView = new ScrollView();
            rootVisualElement.Add(scrollView);

            CreatePalettesWidget();
            CreateAssetsWidget();
        }

        public void AddPalette(AssetPalette palette, bool setSelected = false)
        {
            PalettesWidget.AddAsset(palette);
            if (setSelected)
            {
                PalettesWidget.SelectAsset(palette);
            }
        }

        #region Palettes Widget
        private void CreatePalettesWidget()
        {
            var label = new Label("Asset Palettes");
            label.style.unityFontStyleAndWeight = FontStyle.Bold;
            scrollView.Add(label);

            PalettesWidget = new AssetGridWidget<AssetPalette>(AssetPalettes, SelectedPalette);
            PalettesWidget.FormatName = PalettesWidget_FormatName;
            PalettesWidget.SetupImage = PalettesWidget_SetupImage;
            PalettesWidget.OnAssetsListChanged += PalettesWidget_AssetsChanged;
            PalettesWidget.OnSelectedAssetChanged += PalettesWidget_SelectedAssetChanged;
            scrollView.Add(PalettesWidget);
        }

        private void PalettesWidget_SelectedAssetChanged(AssetPalette obj)
        {
            AssetsWidget.SetAssetPalette(obj);
        }

        private void PalettesWidget_AssetsChanged()
        {
            AssetPalettes = new List<AssetPalette>();
            AssetPalettes.AddRange(PalettesWidget.GetAssets());
        }
        private static string PalettesWidget_FormatName(string arg)
        {
            return Regex.Replace(
                arg.Replace("palette", "", StringComparison.OrdinalIgnoreCase).Replace("_", " "),
                @"\s+",
                " "
            ).Trim();
        }

        private void PalettesWidget_SetupImage(Image image, AssetPalette palette)
        {
            if (palette.Entries != null && palette.Entries.Length > 0)
            {
                image.LazyLoadAssetPreview(palette.Entries[0].Asset);
            }
            else
            {
                image.LazyLoadAssetPreview(palette);
            }
        }
        #endregion

        #region Assets Widget
        public string Asset_FormatName(string arg)
        {
            if (arg.Length > 12)
            {
                return $"{arg[0..9]}...";
            }

            return arg;
        }
        #endregion

        private void CreateAssetsWidget()
        {
            var label = new Label("Assets");
            label.style.unityFontStyleAndWeight = FontStyle.Bold;
            scrollView.Add(label);

            AssetsWidget = new AssetPaletteWidget();
            AssetsWidget.FormatName = Asset_FormatName;
            scrollView.Add(AssetsWidget);

            PalettesWidget_SelectedAssetChanged(PalettesWidget.SelectedAsset);
        }
    }
}