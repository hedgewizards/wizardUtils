using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

namespace WizardUtils.UI
{
    public class HorizontalFlexLayout : LayoutGroup
    {
        public enum TextJustification
        {
            Top,
            Middle,
            Bottom,
        }

        public bool ControlChildWidth;
        public TextJustification RowAlignment;
        public float RowSpacing;
        public float Spacing;
        [NonSerialized]
        private List<RowInfo> RowInfos;

        public override void CalculateLayoutInputHorizontal()
        {
            base.CalculateLayoutInputHorizontal();

            float maxRowWidth = rectTransform.rect.width - padding.horizontal;
            var rectChildrenCount = rectChildren.Count;

            RowInfos = new List<RowInfo>();
            int lastBreak = 0;

            float biggestRowMin = 0, biggestRowPreferred = 0, biggestRowFlexible = 0;
            float currentRowMin = 0, currentRowPreferred = 0, currentRowFlexible = 0;

            for (int i = 0; i < rectChildrenCount; i++)
            {
                RectTransform child = rectChildren[i];
                float min, preferred, flexible;
                GetChildSizes(child, 0, ControlChildWidth, out min, out preferred, out flexible);

                if (currentRowMin + min > maxRowWidth
                    || currentRowPreferred + preferred > maxRowWidth
                    || currentRowFlexible + flexible > 1)
                {
                    RowInfos.Add(new RowInfo()
                    {
                        Start = lastBreak,
                        Length = i - lastBreak,
                        WidthMin = currentRowMin,
                        WidthPreferred = currentRowPreferred,
                        WidthFlexible = currentRowFlexible
                    });

                    biggestRowMin = Mathf.Max(biggestRowMin, currentRowMin);
                    biggestRowPreferred = Mathf.Max(biggestRowPreferred, currentRowPreferred);
                    biggestRowFlexible = Mathf.Max(biggestRowFlexible, currentRowFlexible);

                    currentRowMin = min;
                    currentRowPreferred = preferred;
                    currentRowFlexible = flexible;
                    lastBreak = i;
                }
                else
                {
                    currentRowMin += min;
                    currentRowPreferred += preferred;
                    currentRowFlexible += flexible;
                }
            }

            if (lastBreak < rectChildrenCount)
            {
                RowInfos.Add(new RowInfo()
                {
                    Start = lastBreak,
                    Length = rectChildrenCount - lastBreak,
                    WidthMin = currentRowMin,
                    WidthPreferred = currentRowPreferred,
                    WidthFlexible = currentRowFlexible
                });

                biggestRowMin = Mathf.Max(biggestRowMin, currentRowMin);
                biggestRowPreferred = Mathf.Max(biggestRowPreferred, currentRowPreferred);
                biggestRowFlexible = Mathf.Max(biggestRowFlexible, currentRowFlexible);
            }

            SetLayoutInputForAxis(biggestRowMin + padding.horizontal, biggestRowPreferred + padding.horizontal, biggestRowFlexible, 0);
        }

        public override void CalculateLayoutInputVertical()
        {
            for (int rowId = 0; rowId < RowInfos.Count; rowId++)
            {
                var rowInfo = RowInfos[rowId];
                rowInfo.HeightMin = 0;
                rowInfo.HeightPreferred = 0;
                rowInfo.HeightFlexible = 0;

                for (int rowEntryId = 0; rowEntryId < RowInfos[rowId].Length; rowEntryId++)
                {
                    int childId = RowInfos[rowId].Start + rowEntryId;
                    RectTransform child = rectChildren[childId];
                    float min, preferred, flexible;
                    GetChildSizes(child, 1, false, out min, out preferred, out flexible);

                    rowInfo.HeightMin = Mathf.Max(rowInfo.HeightMin, min);
                    rowInfo.HeightPreferred = Mathf.Max(rowInfo.HeightPreferred, preferred);
                    rowInfo.HeightFlexible = Mathf.Max(rowInfo.HeightFlexible, flexible);
                }

                RowInfos[rowId] = rowInfo;
            }

            float totalMin = padding.vertical;
            float totalPreferred = padding.vertical;
            float totalFlexible = 0;
            for (int n = 0; n < RowInfos.Count; n++)
            {
                totalMin += RowInfos[n].HeightMin + RowSpacing;
                totalPreferred += RowInfos[n].HeightPreferred + RowSpacing;
                totalFlexible += RowInfos[n].HeightFlexible;
            }

            if (RowInfos.Count > 0)
            {
                totalMin -= RowSpacing;
                totalPreferred -= RowSpacing;
            }

            SetLayoutInputForAxis(totalMin, totalPreferred, totalFlexible, 1);
        }

        public override void SetLayoutHorizontal()
        {
            int axis = 0;
            float size = rectTransform.rect.size[axis];
            float alignmentOnAxis = GetAlignmentOnAxis(axis);

            for (int rowId = 0; rowId < RowInfos.Count; rowId++)
            {
                float innerSize = size - (axis == 0 ? padding.horizontal : padding.vertical);

                float pos = (axis == 0 ? padding.left : padding.top);
                float itemFlexibleMultiplier = 0;
                float surplusSpace = size - RowInfos[rowId].WidthPreferred;

                if (surplusSpace > 0)
                {
                    if (RowInfos[rowId].WidthFlexible == 0)
                        pos = GetStartOffset(axis, RowInfos[rowId].WidthPreferred);
                    else if (RowInfos[rowId].WidthFlexible > 0)
                        itemFlexibleMultiplier = surplusSpace / RowInfos[rowId].WidthFlexible;
                }

                float minMaxLerp = 0;
                if (RowInfos[rowId].WidthMin != RowInfos[rowId].WidthPreferred)
                    minMaxLerp = Mathf.Clamp01((size - RowInfos[rowId].WidthMin) / (RowInfos[rowId].WidthPreferred - RowInfos[rowId].WidthMin));

                for (int rowEntryId = 0; rowEntryId < RowInfos[rowId].Length; rowEntryId++)
                {
                    int childId = RowInfos[rowId].Start + rowEntryId;
                    RectTransform child = rectChildren[childId];
                    float min, preferred, flexible;
                    GetChildSizes(child, axis, ControlChildWidth, out min, out preferred, out flexible);
                    float childSize = Mathf.Lerp(min, preferred, minMaxLerp);
                    childSize += flexible * itemFlexibleMultiplier;

                    if (ControlChildWidth)
                    {
                        SetChildAlongAxis(child, axis, pos);
                    }
                    else
                    {
                        float offsetInCell = (childSize - child.sizeDelta[axis]) * alignmentOnAxis;
                        SetChildAlongAxis(child, axis, pos + offsetInCell);
                    }
                    pos += childSize + Spacing;
                }
            }
        }

        public override void SetLayoutVertical()
        {
            int axis = 1;
            float size = rectTransform.rect.size[axis];
            float alignmentOnAxis = GetAlignmentOnAxis(axis);
            float rowAlignmentOnAxis = axis == 1 ? GetVerticalRowAlignment() : 0;

            float innerSize = size - (axis == 0 ? padding.horizontal : padding.vertical);

            float pos = (axis == 0 ? padding.left : padding.top);
            float itemFlexibleMultiplier = 0;
            float surplusSpace = size - GetTotalPreferredSize(axis);

            if (surplusSpace > 0)
            {
                if (GetTotalFlexibleSize(axis) == 0)
                    pos = GetStartOffset(axis, GetTotalPreferredSize(axis) - (axis == 0 ? padding.horizontal : padding.vertical));
                else if (GetTotalFlexibleSize(axis) > 0)
                    itemFlexibleMultiplier = surplusSpace / GetTotalFlexibleSize(axis);
            }

            for (int rowId = 0; rowId < RowInfos.Count; rowId++)
            {
                float minMaxLerp = 0;
                if (RowInfos[rowId].HeightMin != RowInfos[rowId].HeightPreferred)
                    minMaxLerp = Mathf.Clamp01((size - RowInfos[rowId].HeightMin) / (RowInfos[rowId].HeightPreferred - RowInfos[rowId].HeightMin));
                float rowSize = Mathf.Lerp(RowInfos[rowId].HeightMin, RowInfos[rowId].HeightPreferred, minMaxLerp);
                rowSize += RowInfos[rowId].HeightFlexible * itemFlexibleMultiplier;

                for (int rowEntryId = 0; rowEntryId < RowInfos[rowId].Length; rowEntryId++)
                {
                    int childId = RowInfos[rowId].Start + rowEntryId;
                    RectTransform child = rectChildren[childId];
                    float min, preferred, flexible;
                    GetChildSizes(child, axis, false, out min, out preferred, out flexible);
                    float childSize = Mathf.Lerp(min, preferred, minMaxLerp);
                    childSize += flexible * itemFlexibleMultiplier;

                    float offsetInRow = (rowSize - child.sizeDelta[axis]) * rowAlignmentOnAxis;
                    SetChildAlongAxis(child, axis, pos + offsetInRow);
                    rowSize = Mathf.Max(childSize, rowSize);
                }
                pos += rowSize + RowSpacing;
            }
        }

        private float GetVerticalRowAlignment()
        {
            return ((int)RowAlignment % 3) * 0.5f;
        }

        private void GetChildSizes(RectTransform child, int axis, bool controlSize, out float min, out float preferred, out float flexible)
        {
            if (!controlSize)
            {
                min = child.sizeDelta[axis];
                preferred = min;
                flexible = 0;
            }
            else
            {
                min = LayoutUtility.GetMinSize(child, axis);
                preferred = LayoutUtility.GetPreferredSize(child, axis);
                flexible = LayoutUtility.GetFlexibleSize(child, axis);
            }
        }

        private struct RowInfo
        {
            public int Start;
            public int Length;
            public float WidthMin;
            public float WidthPreferred;
            public float WidthFlexible;
            public float HeightMin;
            public float HeightPreferred;
            public float HeightFlexible;
        }
    }
}