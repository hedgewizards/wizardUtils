using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WizardUtils.UI.Pages
{
    /// <summary>
    /// Show a single page. No backwards navigation.<br/>
    /// need backwards navigation? try a <see cref="NavigationStack"/>
    /// </summary>
    public class PageViewer : MonoBehaviour
    {
        private IPage CurrentPage;
        public PageManifest PageManifest;
        private PageSource PageSource;
        private Coroutine CurrentAction;

        public void Awake()
        {
            PageSource = new PageSource(PageManifest, transform);
        }

        public void Open(PageDescriptor descriptor, bool instant = false) => Open(descriptor.Key, instant);
        public void Open(string pageKey, bool instant = false)
        {
            IPage page = PageSource.Get(pageKey);
            page.Disappear();
            Open(page, instant);
        }
        public void Open(IPage page, bool instant = false)
        {
            if (CurrentAction != null)
            {
                throw new InvalidOperationException($"Tried to open page '{page}' while already animating. this isn't supported yet.");
            }

            if (!instant)
            {
                CurrentAction = StartCoroutine(OpenAsync(page));
                return;
            }

            if (CurrentPage != null)
            {
                CurrentPage.Disappear(true);
                UnsubscribePage(CurrentPage);
            }
            page.Appear(true);
            SubscribePage(page);
        }

        public void Close(bool instant = false)
        {
            if (CurrentPage == null) return;

            if (CurrentAction != null)
            {
                throw new InvalidOperationException($"Tried to close page '{CurrentPage}' while already animating. this isn't supported yet.");
            }

            if (!instant)
            {
                CurrentAction = StartCoroutine(CloseAsync());
                return;
            }

            CurrentPage.Disappear(true);
            UnsubscribePage(CurrentPage);
            CurrentPage = null;
        }

        private IEnumerator CloseAsync()
        {
            UnsubscribePage(CurrentPage);
            CurrentPage.Disappear();
            if (CurrentPage.DisappearDurationSeconds > 0)
            {
                yield return new WaitForSecondsRealtime(CurrentPage.DisappearDurationSeconds);
            }
            CurrentPage = null;
        }

        private IEnumerator OpenAsync(IPage newPage)
        {
            if (CurrentPage != null)
            {
                UnsubscribePage(CurrentPage);
                CurrentPage.Disappear();
                if (CurrentPage.DisappearDurationSeconds > 0)
                {
                    yield return new WaitForSecondsRealtime(CurrentPage.DisappearDurationSeconds);
                }
            }

            SubscribePage(newPage);
            CurrentPage = newPage;
            newPage.Appear();
            if (newPage.AppearDurationSeconds > 0)
            {
                yield return new WaitForSecondsRealtime(newPage.AppearDurationSeconds);
            }

            CurrentAction = null;
        }


        private void SubscribePage(IPage page)
        {
            page.OnNavigateTo += CurrentPage_OnNavigateTo;
        }

        private void UnsubscribePage(IPage page)
        {
            page.OnNavigateTo -= CurrentPage_OnNavigateTo;
        }

        private void CurrentPage_OnNavigateTo(object sender, NavigateToEventArgs e)
        {
            if (e.PageKey != null)
            {
                Open(e.PageKey, e.Instant);
            }
            else
            {
                Open(e.Page, e.Instant);
            }
        }
    }
}
