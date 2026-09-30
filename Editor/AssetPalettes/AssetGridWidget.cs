using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace WizardUtils.AssetPalettes
{
    public class AssetGridWidget<T> : VisualElement where T : UnityEngine.Object
    {
        #region Constants
        public Func<string, string> FormatName { get; set; }
        public Action<Image, T> SetupImage { get; set; }

        private const float CellWidth = 80f;
        private const float CellHeight = 80f;
        private const float ImageHeight = 56f;
        private const float RootPadding = 4f;
        #endregion

        #region Components
        private VisualElement MainGrid;

        private VisualElement AddCell;
        private Image AddCellImage;
        private Label AddCellLabel;

        private Image DragPreviewImage;
        private int ObjectPickerControlId;
        private T ObjectPickerResult;
        #endregion

        #region Variables
        public T SelectedAsset { get; private set; }
        private readonly List<T> Assets = new();

        private T DraggedAsset;
        private Vector2 DragStartPosition;
        private bool IsDragging;
        private bool IsTrashHovered;
        #endregion

        #region Events
        public event Action<T> OnSelectedAssetChanged;
        public event Action OnAssetsListChanged;
        #endregion

        public AssetGridWidget(IList<T> assets, T selectedAsset)
        {
            CreateGrid();

            var pickerHandler = new IMGUIContainer(HandleObjectPickerGUI);
            pickerHandler.style.width = 0;
            pickerHandler.style.height = 0;
            Add(pickerHandler);

            Assets = assets.ToList();
            if (Assets.Contains(selectedAsset))
            {
                SelectedAsset = selectedAsset;
            }

            UpdateGrid();

            DragPreviewImage = new Image
            {
                scaleMode = ScaleMode.ScaleToFit,
                pickingMode = PickingMode.Ignore
            };

            DragPreviewImage.style.position = Position.Absolute;
            DragPreviewImage.style.width = CellWidth * 0.5f;
            DragPreviewImage.style.height = CellHeight * 0.5f;
            DragPreviewImage.style.display = DisplayStyle.None;

            Add(DragPreviewImage);
        }

        public void SetAssets(IList<T> assets)
        {
            Assets.Clear();

            if (assets != null)
            {
                foreach (var asset in assets)
                {
                    if (asset != null)
                        Assets.Add(asset);
                }
            }

            if (SelectedAsset != null && !Assets.Contains(SelectedAsset))
                SelectedAsset = null;

            UpdateGrid();
            OnAssetsListChanged?.Invoke();
        }

        public IList<T> GetAssets() => Assets;

        public void AddAsset(T newAsset)
        {
            if (!Assets.Contains(newAsset))
            {
                Assets.Add(newAsset);
                UpdateGrid();
                OnAssetsListChanged?.Invoke();
            }
        }

        public void SelectAsset(T asset)
        {
            if (asset != null && !Assets.Contains(asset))
            {
                return;
            }

            SelectedAsset = asset;
            UpdateGrid();
            OnSelectedAssetChanged?.Invoke(asset);
        }

        private void RemoveAsset(T asset)
        {
            bool notifySelectedChanged = false;
            Assets.Remove(asset);

            if (SelectedAsset == asset)
            {
                notifySelectedChanged = true;
                SelectedAsset = null;
            }

            UpdateGrid();
            OnAssetsListChanged?.Invoke();

            if (notifySelectedChanged)
            {
                OnSelectedAssetChanged.Invoke(null);
            }
        }

        private void UpdateGrid()
        {
            MainGrid.Clear();

            foreach (var asset in Assets)
            {
                VisualElement cell = CreateAssetCell(asset);

                MainGrid.Add(cell);
            }

            VisualElement addCell = CreateAddCell();

            MainGrid.Add(addCell);
        }

        #region UI Creation
        private void CreateGrid()
        {
            MainGrid = new VisualElement();
            MainGrid.style.flexDirection = FlexDirection.Row;
            MainGrid.style.flexWrap = Wrap.Wrap;
            MainGrid.style.alignContent = Align.FlexStart;

            style.borderLeftWidth = 1;
            style.borderRightWidth = 1;
            style.borderTopWidth = 1;
            style.borderBottomWidth = 1;

            style.backgroundColor = new Color(0.16f, 0.16f, 0.16f);

            style.paddingLeft = RootPadding;
            style.paddingRight = RootPadding;
            style.paddingTop = RootPadding;
            style.paddingBottom = RootPadding;

            Add(MainGrid);
        }

        private VisualElement CreateAssetCell(T asset)
        {
            var cell = new VisualElement();
            cell.style.width = CellWidth;
            cell.style.height = CellHeight;
            cell.style.marginRight = RootPadding;
            cell.style.marginBottom = RootPadding;

            var image = new Image
            {
                scaleMode = ScaleMode.ScaleToFit
            };

            if (SetupImage != null)
                SetupImage(image, asset);
            else
                image.LazyLoadAssetPreview(asset);

            image.style.height = ImageHeight;
            image.style.flexShrink = 0;

            var label = new Label(FormatName != null ? FormatName(asset.name) : asset.name);
            label.style.maxWidth = CellWidth;
            label.style.overflow = Overflow.Hidden;
            label.style.unityTextAlign = TextAnchor.MiddleCenter;
            label.style.textOverflow = TextOverflow.Clip;

            cell.Add(image);
            cell.Add(label);

            UpdateCellSelectionStyle(cell, asset, false);

            var selectedAsset = asset;

            cell.RegisterCallback<MouseDownEvent>(evt =>
            {
                DraggedAsset = selectedAsset;
                DragStartPosition = evt.mousePosition;
                IsDragging = false;

                DragPreviewImage.image = image.image;
                DragPreviewImage.style.display = DisplayStyle.None;

                cell.CaptureMouse();
            });

            cell.RegisterCallback<MouseMoveEvent>(evt =>
            {
                if (DraggedAsset != selectedAsset)
                    return;

                if (!IsDragging && Vector2.Distance(DragStartPosition, evt.mousePosition) > 5f)
                {
                    IsDragging = true;
                    SetTrashMode(true);
                    DragPreviewImage.style.display = DisplayStyle.Flex;
                }

                if (!IsDragging)
                    return;

                DragPreviewImage.style.left = evt.mousePosition.x - CellWidth * 0.5f;
                DragPreviewImage.style.top = evt.mousePosition.y - CellHeight * 0.5f;

                bool hovered = AddCell != null && AddCell.worldBound.Contains(evt.mousePosition);

                if (hovered != IsTrashHovered)
                {
                    IsTrashHovered = hovered;
                    UpdateAddCellStyle();
                }
            });

            cell.RegisterCallback<MouseUpEvent>(evt =>
            {
                if (DraggedAsset != selectedAsset)
                    return;

                cell.ReleaseMouse();

                bool droppedOnTrash =
                    IsDragging &&
                    AddCell != null &&
                    AddCell.worldBound.Contains(evt.mousePosition);

                if (droppedOnTrash)
                {
                    RemoveAsset(selectedAsset);
                }
                else if (!IsDragging)
                {
                    SelectedAsset = selectedAsset;
                    UpdateGrid();
                    OnSelectedAssetChanged?.Invoke(selectedAsset);
                }

                DraggedAsset = null;
                IsDragging = false;
                IsTrashHovered = false;
                SetTrashMode(false);

                DragPreviewImage.style.display = DisplayStyle.None;
                DragPreviewImage.image = null;
            });

            cell.RegisterCallback<MouseEnterEvent>(_ =>
            {
                UpdateCellSelectionStyle(cell, selectedAsset, true);
            });

            cell.RegisterCallback<MouseLeaveEvent>(_ =>
            {
                UpdateCellSelectionStyle(cell, selectedAsset, false);
            });

            return cell;
        }


        private VisualElement CreateAddCell()
        {
            AddCell = new VisualElement();

            AddCell.style.width = CellWidth;
            AddCell.style.height = CellHeight;
            AddCell.style.marginRight = RootPadding;
            AddCell.style.marginBottom = RootPadding;

            AddCellImage = new Image
            {
                image = EditorGUIUtility.IconContent("Toolbar Plus").image,
                scaleMode = ScaleMode.ScaleToFit
            };
            AddCellImage.style.height = ImageHeight;
            AddCellImage.style.flexShrink = 0;

            AddCellLabel = new Label("Add");
            AddCellLabel.style.maxWidth = CellWidth;
            AddCellLabel.style.overflow = Overflow.Hidden;
            AddCellLabel.style.unityTextAlign = TextAnchor.MiddleCenter;
            AddCellLabel.style.textOverflow = TextOverflow.Clip;

            AddCell.Add(AddCellImage);
            AddCell.Add(AddCellLabel);

            AddCell.RegisterCallback<MouseDownEvent>(_ =>
            {
                if (!IsDragging)
                    OpenAddAssetWindow();
            });

            AddCell.style.borderLeftWidth = 1;
            AddCell.style.borderRightWidth = 1;
            AddCell.style.borderTopWidth = 1;
            AddCell.style.borderBottomWidth = 1;

            AddCell.style.borderLeftColor = Color.clear;
            AddCell.style.borderRightColor = Color.clear;
            AddCell.style.borderTopColor = Color.clear;
            AddCell.style.borderBottomColor = Color.clear;

            AddCell.RegisterCallback<MouseEnterEvent>(_ =>
            {
                if (IsDragging)
                {
                    IsTrashHovered = true;
                    UpdateAddCellStyle();
                }
                else
                {
                    AddCell.style.borderLeftColor = Color.gray;
                    AddCell.style.borderRightColor = Color.gray;
                    AddCell.style.borderTopColor = Color.gray;
                    AddCell.style.borderBottomColor = Color.gray;
                }
            });

            AddCell.RegisterCallback<MouseLeaveEvent>(_ =>
            {
                if (IsDragging)
                {
                    IsTrashHovered = false;
                    UpdateAddCellStyle();
                }
                else
                {
                    AddCell.style.borderLeftColor = Color.clear;
                    AddCell.style.borderRightColor = Color.clear;
                    AddCell.style.borderTopColor = Color.clear;
                    AddCell.style.borderBottomColor = Color.clear;
                }
            });

            return AddCell;
        }
        #endregion

        private void SetTrashMode(bool enabled)
        {
            IsTrashHovered = false;

            if (AddCell != null && enabled)
            {
                AddCellImage.image = EditorGUIUtility.IconContent("TreeEditor.Trash").image;
                AddCellLabel.text = "Delete";
            }
            else
            {
                AddCellImage.image = EditorGUIUtility.IconContent("Toolbar Plus").image;
                AddCellLabel.text = "Add";
            }

            UpdateAddCellStyle();
        }

        private void UpdateAddCellStyle()
        {
            Color borderColor = IsTrashHovered ? Color.white : Color.clear;

            AddCell.style.borderLeftColor = borderColor;
            AddCell.style.borderRightColor = borderColor;
            AddCell.style.borderTopColor = borderColor;
            AddCell.style.borderBottomColor = borderColor;
        }

        private void UpdateCellSelectionStyle(VisualElement cell, T asset, bool isHovered)
        {
            var selected = asset == SelectedAsset;
            var borderWidth = selected ? 2 : isHovered ? 1 : 0;
            var borderColor = selected ? Color.white : Color.gray;

            cell.style.borderLeftWidth = borderWidth;
            cell.style.borderRightWidth = borderWidth;
            cell.style.borderTopWidth = borderWidth;
            cell.style.borderBottomWidth = borderWidth;

            cell.style.borderLeftColor = borderColor;
            cell.style.borderRightColor = borderColor;
            cell.style.borderTopColor = borderColor;
            cell.style.borderBottomColor = borderColor;
        }

        #region Object Picker
        private void OpenAddAssetWindow()
        {
            ObjectPickerControlId = GUIUtility.GetControlID(FocusType.Passive);
            ObjectPickerResult = null;

            EditorGUIUtility.ShowObjectPicker<T>(
                null,
                false,
                "",
                ObjectPickerControlId
            );
        }
        private void HandleObjectPickerGUI()
        {
            var evt = Event.current;

            if (evt.type != EventType.ExecuteCommand)
                return;

            if (EditorGUIUtility.GetObjectPickerControlID() != ObjectPickerControlId)
                return;

            if (evt.commandName == "ObjectSelectorUpdated")
            {
                ObjectPickerResult = EditorGUIUtility.GetObjectPickerObject() as T;
                evt.Use();
            }
            else if (evt.commandName == "ObjectSelectorClosed")
            {
                if (ObjectPickerResult != null)
                {
                    AddAsset(ObjectPickerResult);
                }

                ObjectPickerResult = null;
                ObjectPickerControlId = 0;
                evt.Use();
            }
            else if (evt.commandName == "ObjectSelectorCanceled")
            {
                ObjectPickerResult = null;
                ObjectPickerControlId = 0;
                evt.Use();
            }
        }

        #endregion
    }
}