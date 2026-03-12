using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace WizardUtils.AssetPalettes
{
    public class AssetPaletteWindow : EditorWindow
    {
        public AssetPalette[] Palettes;

        private ScrollView scrollView;
        private readonly Dictionary<AssetPalette, VisualElement> paletteContainers = new();

        private const float cellWidth = 80f;
        private const float cellHeight = 80f;
        private const float padding = 4f;

        [MenuItem("Window/WizardUtils/Asset Palette")]
        private static void ShowWindow()
        {
            var window = GetWindow<AssetPaletteWindow>("Asset Palette");
            window.Show();
        }

        private void OnEnable()
        {
            rootVisualElement.Clear();

            rootVisualElement.Add(new IMGUIContainer(() =>
            {
                SerializedObject so = new SerializedObject(this);
                SerializedProperty palettesProp = so.FindProperty(nameof(Palettes));

                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(palettesProp, true);
                if (EditorGUI.EndChangeCheck())
                {
                    so.ApplyModifiedProperties();
                    SyncGridsWithPaletteArray();
                }
            }));
            
            scrollView = new ScrollView();
            rootVisualElement.Add(scrollView);

            SyncGridsWithPaletteArray();
        }

        public void AddPalette(AssetPalette palette)
        {
            if (palette == null)
                return;

            if (Palettes != null && Palettes.Contains(palette))
                return;

            var list = Palettes != null ? Palettes.ToList() : new List<AssetPalette>();
            list.Add(palette);
            Palettes = list.ToArray();

            AddGrid(palette);
        }

        public void RemovePalette(AssetPalette palette)
        {
            if (palette == null || Palettes == null)
                return;

            if (!Palettes.Contains(palette))
                return;

            Palettes = Palettes.Where(p => p != palette).ToArray();
            RemoveGrid(palette);
        }
        private void SyncGridsWithPaletteArray()
        {
            var desired = new HashSet<AssetPalette>(
                (Palettes ?? Array.Empty<AssetPalette>()).Where(p => p != null)
            );

            foreach (var existing in paletteContainers.Keys.ToList())
            {
                if (!desired.Contains(existing))
                    RemoveGrid(existing);
            }

            if (Palettes != null)
            {
                foreach (var palette in Palettes)
                {
                    if (palette == null)
                        continue;

                    if (!paletteContainers.ContainsKey(palette))
                        AddGrid(palette);
                }
            }

            ReorderGrids();
        }

        private void AddGrid(AssetPalette palette)
        {
            if (palette == null || paletteContainers.ContainsKey(palette))
                return;

            var container = new VisualElement();
            container.style.flexDirection = FlexDirection.Column;

            var label = new Label(palette.name);
            label.style.unityFontStyleAndWeight = FontStyle.Bold;
            container.Add(label);

            var grid = CreateGrid();
            container.Add(grid);

            paletteContainers.Add(palette, container);
            scrollView.Add(container);

            UpdatePalette(grid, palette);
        }
        private void RemoveGrid(AssetPalette palette)
        {
            if (!paletteContainers.TryGetValue(palette, out var container))
                return;

            container.RemoveFromHierarchy();
            paletteContainers.Remove(palette);
        }
        private void ReorderGrids()
        {
            if (Palettes == null)
                return;

            for (int i = 0; i < Palettes.Length; i++)
            {
                var palette = Palettes[i];
                if (palette == null)
                    continue;

                if (paletteContainers.TryGetValue(palette, out var container))
                {
                    container.RemoveFromHierarchy();
                    scrollView.Add(container);
                }
            }
        }

        private VisualElement CreateGrid()
        {
            var grid = new VisualElement();
            grid.style.flexDirection = FlexDirection.Row;
            grid.style.flexWrap = Wrap.Wrap;
            grid.style.alignContent = Align.FlexStart;
            grid.style.marginBottom = 10;
            return grid;
        }

        private void UpdatePalette(VisualElement paletteGrid, AssetPalette palette)
        {
            paletteGrid.Clear();

            if (palette == null)
                return;

            for (int i = 0; i < palette.Entries.Length; i++)
            {
                var cell = new VisualElement();
                cell.style.width = cellWidth;
                cell.style.height = cellHeight;
                cell.style.marginRight = padding;
                cell.style.marginBottom = padding;

                var image = new Image
                {
                    scaleMode = ScaleMode.ScaleToFit,
                    tooltip = palette.Entries[i].Tooltip
                };

                image.LazyLoadAssetPreview(palette.Entries[i].Asset);
                image.style.flexGrow = 1;

                var label = new Label(palette.Entries[i].DisplayName);
                label.style.maxWidth = cellWidth;
                label.style.overflow = Overflow.Hidden;
                label.style.unityTextAlign = TextAnchor.MiddleCenter;
                label.style.textOverflow = TextOverflow.Clip;

                cell.Add(image);
                cell.Add(label);

                paletteGrid.Add(cell);

                var asset = palette.Entries[i].Asset;

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