﻿using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using WizardUtils.UI;

namespace WizardUtils
{
    public class PauseMenuController : ToggleableUIElement
    {
        public UnityEvent OnMenuClosed;

        public ToggleableUIElement BaseMenu;

        ToggleableUIElement currentMenu;
        public ToggleableUIElement CurrentMenu
        {
            get
            {
                return currentMenu;
            }
            set
            {
                if (currentMenu == value) return;

                if (currentMenu != null)
                {
                    currentMenu.SetOpen(false);
                }
                if (value != null)
                {
                    value.SetOpen(true);
                }

                currentMenu = value;
            }
        }

        /// <summary>
        /// When the game is paused, start in this menu instead of the base menu
        /// </summary>
        public ToggleableUIElement OverrideMenu;

        public void ReturnToBaseMenu()
        {
            CurrentMenu = BaseMenu;
        }

        protected virtual void Start()
        {
            currentMenu = null;
            GameManager.Instance.OnPauseStateChanged += onPauseStateChanged;
        }

        protected virtual void onPauseStateChanged(object sender, bool nowPaused)
        {
            OnMenuClosed?.Invoke();
            Root?.SetActive(nowPaused);
            CurrentMenu = nowPaused ? (OverrideMenu ?? BaseMenu) : null;
        }

        public void Pause()
        {
            GameManager.Instance.Paused = true;
        }

        public void Resume()
        {
            GameManager.Instance.Paused = false;
        }

        public void ReturnToMainMenu()
        {
            Resume();
            GameManager.Instance?.Quit(false);
        }

    }

}
