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
        private Stack<IPage> PageStack;
        private Coroutine CurrentStackAction;

        public NavigationStack(IPage rootPage)
        {
            PageStack = new Stack<IPage>();
            PageStack.Push(rootPage);
            rootPage.Appear(true);
        }

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
