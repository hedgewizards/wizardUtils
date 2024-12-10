using System;
using System.Collections;
using System.Collections.Generic;

namespace WizardUtils
{
    /// <summary>
    /// A class similar to a unity coroutine, but runs when you call Think().
    /// Only supports "yield return null" and "yield return (another IEnumerator)
    /// </summary>
    public class ManualCoroutine
    {
        private Stack<IEnumerator> Stack;

        public ManualCoroutine(IEnumerator enumerator)
        {
            Stack = new Stack<IEnumerator>();
            Stack.Push(enumerator);
        }

        /// <summary>
        /// Performs the next step in the enumerator, returning FALSE if finished
        /// </summary>
        /// <returns>TRUE if there's another step, else false</returns>
        public bool Think()
        {
            while (Stack.Count > 0)
            {
                var enumerator = Stack.Peek();
                if (!enumerator.MoveNext())
                {
                    Stack.Pop();
                    continue;
                }

                if (enumerator.Current is IEnumerator nestedEnumerator)
                {
                    Stack.Push(nestedEnumerator);
                    continue;
                }

                if (enumerator.Current != null)
                {
                    throw new InvalidOperationException($"ManualCoroutine yielded unsupported type {enumerator.Current.GetType()}. Only supports yield return null D:");
                }

                return true;
            }

            return false;
        }
    }
}
