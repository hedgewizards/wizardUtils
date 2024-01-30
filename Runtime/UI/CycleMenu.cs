using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using WizardUtils.Extensions;
using TMPro;
using UnityEngine.UI;

namespace WizardUtils.UI
{
    public class CycleMenu : MonoBehaviour
    {
        public UnityEvent<int> OnItemSelected;
        public TextMeshProUGUI DisplayNameText;
        public Button NextButton;
        public Button PreviousButton;

        private bool IsDirty;

        private SortedList<int, string> MenuItems;
        public string PlaceholderText;
        private int CurrentIndex;

        private void Awake()
        {
            if (MenuItems == null) Initialize();
            NextButton.onClick.AddListener(NextItem);
            PreviousButton.onClick.AddListener(PreviousItem);
        }

        public void Initialize()
        {
            MenuItems = new SortedList<int, string>();
            IsDirty = true;
        }

        public void AddMenuItem(int id, string displayText)
        {
            if (MenuItems.TryGetValue(id, out string existingDisplaytext))
            {
                throw new ArgumentException($"Tried to add CycleMenu item with duplicate key {id}. entries '{existingDisplaytext}' & '{displayText}'");
            }

            MenuItems.Add(id, displayText);

            if (MenuItems.Count == 1)
            {
                IsDirty = true;
            }
        }

        public void SelectItem(int id)
        {
            CurrentIndex = id;
            OnItemSelected?.Invoke(id);
            IsDirty = true;
        }

        private void LateUpdate()
        {
            if (IsDirty)
            {
                DisplayNameText.text = GetText();
                IsDirty = false;
            }
        }

        private void NextItem() => Move(1);
        private void PreviousItem() => Move(-1);
        private void Move(int offset)
        {
            if (MenuItems.Count == 0)
            {
                return;
            }
            SelectItem(IntExtensions.PositiveModulo(CurrentIndex + offset, MenuItems.Count));
        }

        private string GetText()
        {
            if (MenuItems.TryGetValue(CurrentIndex, out string text))
            {
                return text;
            }
            return PlaceholderText;
        }
    }
}
