using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace WizardUtils.AssetPalettes
{
    public class AssetPaletteWidget : VisualElement
    {
        private const float cellWidth = 80f;
        private const float cellHeight = 80f;
        private const float padding = 4f;

        public Func<string, string> FormatName { get; set; }
        private AssetPalette assetPalette;
        private VisualElement grid;

        public AssetPaletteWidget()
        {
            grid = new VisualElement();
            grid.style.flexDirection = FlexDirection.Row;
            grid.style.flexWrap = Wrap.Wrap;
            grid.style.alignContent = Align.FlexStart;

            Add(grid);
        }

        public void SetAssetPalette(AssetPalette palette)
        {
            assetPalette = palette;
            UpdateGrid();
        }

        private void UpdateGrid()
        {
            grid.Clear();

            if (assetPalette == null || assetPalette.Entries == null)
                return;

            for (int i = 0; i < assetPalette.Entries.Length; i++)
            {
                var entry = assetPalette.Entries[i];
                var asset = entry.Asset;

                var cell = new VisualElement();
                cell.style.width = cellWidth;
                cell.style.height = cellHeight;
                cell.style.marginRight = padding;
                cell.style.marginBottom = padding;

                var image = new Image
                {
                    scaleMode = ScaleMode.ScaleToFit,
                    tooltip = entry.Tooltip
                };

                image.LazyLoadAssetPreview(asset);
                image.style.flexGrow = 1;

                var displayName = FormatName != null
                    ? FormatName(entry.DisplayName)
                    : entry.DisplayName;

                var label = new Label(displayName);
                label.style.maxWidth = cellWidth;
                label.style.overflow = Overflow.Hidden;
                label.style.unityTextAlign = TextAnchor.MiddleCenter;
                label.style.textOverflow = TextOverflow.Clip;

                cell.Add(image);
                cell.Add(label);

                grid.Add(cell);

                cell.RegisterCallback<MouseDownEvent>(evt =>
                {
                    if (evt.button == 0)
                    {
                        DragAndDrop.PrepareStartDrag();
                        DragAndDrop.StartDrag("Create From Palette");
                        DragAndDrop.objectReferences = new UnityEngine.Object[] { asset };
                    }
                    else if (evt.button == 1)
                    {
                        EditorGUIUtility.PingObject(asset);
                    }
                });

                cell.RegisterCallback<DragUpdatedEvent>(evt =>
                {
                    DragAndDrop.visualMode = DragAndDropVisualMode.Move;
                });
            }
        }
    }
}