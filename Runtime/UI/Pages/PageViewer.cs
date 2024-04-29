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
        private Coroutine CurrentOpenAction;

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

            if (CurrentOpenAction != null)
            {
                throw new InvalidOperationException($"Tried to push page '{page}' to stack while already animating. this isn't supported yet.");
            }

            if (!instant)
            {
                CurrentOpenAction = StartCoroutine(OpenAsync(page));
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
