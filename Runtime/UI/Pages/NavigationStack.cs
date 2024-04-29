﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WizardUtils.UI.Pages
{
    /// <summary>
    /// A hierarchy of pages working as a stack. listen to when pages want to navigate back and forward<br/>
    /// Don't need a stack? try a <see cref="PageViewer"/>
    /// </summary>
    public class NavigationStack : MonoBehaviour
    {
        public PageManifest PageManifest;
        private Stack<IPage> PageStack;
        private PageSource PageSource;
        private Coroutine CurrentStackAction;

        public void Awake()
        {
            PageStack = new Stack<IPage>();
            PageSource = new PageSource(PageManifest, transform);
        }

        public void Push(string pageKey, bool instant = false)
        {
            IPage page = PageSource.Get(pageKey);
            page.Disappear(true);
            Push(page, instant);
        }

        public void Push(PageDescriptor pageDescriptor, bool instant = false) => Push(pageDescriptor.Key, instant);

        public void Push(IPage page, bool instant = false)
        {
            if (CurrentStackAction != null)
            {
                throw new InvalidOperationException($"Tried to push page '{page}' to stack while already animating. this isn't supported yet.");
            }

            if (!instant)
            {
                CurrentStackAction = StartCoroutine(PushAsync(page));
                return;
            }

            if (PageStack.TryPeek(out IPage topPage))
            {
                topPage.Disappear(true);
                UnsubscribePage(topPage);
            }
            SubscribePage(page);
            PageStack.Push(page);
            page.Appear(true);
        }

        public void Pop(bool instant = false)
        {
            if (CurrentStackAction != null)
            {
                CurrentStackAction = null;
                throw new InvalidOperationException($"Tried to pop from stack while already animating. this isn't supported yet.");
            }

            if (!instant)
            {
                CurrentStackAction = StartCoroutine(PopAsync());
                return;
            }

            if (!PageStack.TryPop(out IPage popPage))
            {
                throw new InvalidOperationException("Tried to pop with no remaining pages");
            }

            popPage.Disappear(true);
            UnsubscribePage(popPage);

            if (PageStack.TryPeek(out IPage newTopPage))
            {
                newTopPage.Appear(true);
                SubscribePage(newTopPage);
            }
        }

        private void SubscribePage(IPage page)
        {
            page.OnNavigateBack += CurrentPage_OnNavigateBack;
            page.OnNavigateTo += CurrentPage_OnNavigateTo;
        }

        private void UnsubscribePage(IPage page)
        {
            page.OnNavigateBack -= CurrentPage_OnNavigateBack;
            page.OnNavigateTo -= CurrentPage_OnNavigateTo;
        }

        private void CurrentPage_OnNavigateTo(object sender, NavigateToEventArgs e)
        {
            if (e.PageKey != null)
            {
                Push(e.PageKey, e.Instant);
            }
            else
            {
                Push(e.Page, e.Instant);
            }
        }

        private void CurrentPage_OnNavigateBack(object sender, NavigateBackEventArgs e)
        {
            Pop(e.Instant);
        }

        private IEnumerator PopAsync()
        {
            if (!PageStack.TryPop(out IPage popPage))
            {
                throw new InvalidOperationException("Tried to pop with no remaining pages");
            }

            UnsubscribePage(popPage);
            popPage.Disappear();
            if (popPage.DisappearDurationSeconds > 0)
            {
                yield return new WaitForSecondsRealtime(popPage.DisappearDurationSeconds);
            }

            if (PageStack.TryPeek(out IPage newTopPage))
            {
                SubscribePage(newTopPage);
                newTopPage.Appear();
                if (newTopPage.AppearDurationSeconds > 0)
                {
                    yield return new WaitForSecondsRealtime(newTopPage.AppearDurationSeconds);
                }
            }

            CurrentStackAction = null;
        }

        private IEnumerator PushAsync(IPage newPage)
        {
            if (PageStack.TryPeek(out IPage topPage))
            {
                UnsubscribePage(topPage);
                topPage.Disappear();
                if (topPage.DisappearDurationSeconds > 0)
                {
                    yield return new WaitForSecondsRealtime(topPage.DisappearDurationSeconds);
                }
            }

            SubscribePage(newPage);
            PageStack.Push(newPage);
            newPage.Appear();
            if (newPage.AppearDurationSeconds > 0)
            {
                yield return new WaitForSecondsRealtime(newPage.AppearDurationSeconds);
            }

            CurrentStackAction = null;
        }
    }
}
