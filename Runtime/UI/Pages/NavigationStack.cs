using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WizardUtils.UI.Pages
{
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

        public bool IsTopPage(IPage page)
        {
            if (PageStack.Count == 0) return false;
            return page == PageStack.Peek();
        }

        public void Push(string pageKey, bool instant = false)
        {
            IPage page = PageSource.Get(pageKey);
            page.Disappear();
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
            }
            page.Disappear(true);
        }

        public void Pop(bool instant = false)
        {
            if (CurrentStackAction != null)
            {
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

            if (PageStack.TryPeek(out IPage newTopPage))
            {
                newTopPage.Appear(true);
            }
        }

        private IEnumerator PopAsync()
        {
            if (!PageStack.TryPop(out IPage popPage))
            {
                throw new InvalidOperationException("Tried to pop with no remaining pages");
            }

            popPage.Disappear();
            if (popPage.DisappearDurationSeconds > 0)
            {
                yield return new WaitForSecondsRealtime(popPage.DisappearDurationSeconds);
            }

            if (PageStack.TryPeek(out IPage newTopPage))
            {
                newTopPage.Appear(true);
                if (newTopPage.AppearDurationSeconds > 0)
                {
                    yield return new WaitForSecondsRealtime(newTopPage.AppearDurationSeconds);
                }
            }
        }

        private IEnumerator PushAsync(IPage newPage)
        {
            if (PageStack.TryPeek(out IPage topPage))
            {
                topPage.Disappear();
                if (topPage.DisappearDurationSeconds > 0)
                {
                    yield return new WaitForSecondsRealtime(topPage.DisappearDurationSeconds);
                }
            }

            PageStack.Push(newPage);
            newPage.Appear();
            if (newPage.AppearDurationSeconds > 0)
            {
                yield return new WaitForSecondsRealtime(newPage.AppearDurationSeconds);
            }
        }
    }
}
